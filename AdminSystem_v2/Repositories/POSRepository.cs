using AdminSystem_v2.Models;
using Dapper;

namespace AdminSystem_v2.Repositories
{
    public class POSRepository : Repository<Order>, IPOSRepository
    {
        // ── Product search ────────────────────────────────────────────────────

        public async Task<IEnumerable<POSProductItem>> SearchProductsAsync(string search)
        {
            const string sql =
                @"SELECT TOP 30
                      p.ProductId,
                      pv.ProductVariantId,
                      p.Name                              AS ProductName,
                      pv.VariantName,
                      p.Price + pv.AdditionalPrice        AS UnitPrice,
                      pv.StockQuantity,
                      ISNULL(pv.SKU, '')                  AS SKU
                  FROM Product         p
                  INNER JOIN ProductVariant pv
                      ON p.ProductId = pv.ProductId
                      AND pv.IsActive = 1
                  WHERE p.IsActive = 1
                    AND (
                        p.Name            LIKE @Search
                        OR pv.VariantName LIKE @Search
                        OR pv.SKU         LIKE @Search
                    )
                  ORDER BY p.Name, pv.VariantName";

            await using var conn = GetConnection();
            return await conn.QueryAsync<POSProductItem>(sql, new { Search = $"%{search}%" });
        }

        // ── Customer search ───────────────────────────────────────────────────

        public async Task<IEnumerable<POSCustomer>> SearchCustomersAsync(string search)
        {
            const string sql =
                @"SELECT TOP 20
                      UserId,
                      FirstName + ' ' + LastName  AS Name,
                      ISNULL(Email, '')            AS Email,
                      ISNULL(PhoneNumber, '')      AS Phone
                  FROM [User]
                  WHERE IsWalkIn = 0
                    AND IsActive  = 1
                    AND (
                        FirstName + ' ' + LastName LIKE @Search
                        OR Email                   LIKE @Search
                        OR PhoneNumber             LIKE @Search
                    )
                  ORDER BY FirstName, LastName";

            await using var conn = GetConnection();
            return await conn.QueryAsync<POSCustomer>(sql, new { Search = $"%{search}%" });
        }

        // ── Walk-in user ──────────────────────────────────────────────────────

        public async Task<int> GetWalkInUserIdAsync()
        {
            const string sql = "SELECT TOP 1 UserId FROM [User] WHERE IsWalkIn = 1";
            await using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql);
        }

        // ── Voucher validation ────────────────────────────────────────────────

        public async Task<POSVoucherResult> ValidateVoucherAsync(string code, int userId, decimal subtotal)
        {
            if (string.IsNullOrWhiteSpace(code))
                return new POSVoucherResult { IsValid = false, Error = "Please enter a voucher code." };

            string trimmedCode = code.Trim().ToUpperInvariant();

            await using var conn = GetConnection();

            // Step 1 — Exists and is active
            var voucher = await conn.QueryFirstOrDefaultAsync<dynamic>(
                @"SELECT VoucherId, Code, DiscountType, DiscountValue,
                         MinimumOrderAmount, MaxUses, MaxUsesPerUser,
                         StartDate, EndDate, IsActive
                  FROM Voucher
                  WHERE Code = @Code",
                new { Code = trimmedCode });

            if (voucher == null || !(bool)voucher.IsActive)
                return new POSVoucherResult { IsValid = false, Error = "This voucher code does not exist or has been deactivated." };

            // Step 2 — Date window
            DateTime now = DateTime.UtcNow;
            if (now < (DateTime)voucher.StartDate)
                return new POSVoucherResult { IsValid = false, Error = "This voucher is not yet active." };
            if (voucher.EndDate != null && now > (DateTime)voucher.EndDate)
                return new POSVoucherResult { IsValid = false, Error = "This voucher has expired." };

            // Step 3 — Minimum order amount
            if (voucher.MinimumOrderAmount != null && subtotal < (decimal)voucher.MinimumOrderAmount)
                return new POSVoucherResult
                {
                    IsValid = false,
                    Error   = $"This voucher requires a minimum order of \u20b1{(decimal)voucher.MinimumOrderAmount:N2}."
                };

            // Step 4 — Global usage cap
            if (voucher.MaxUses != null)
            {
                int globalCount = await conn.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM VoucherUsage WHERE VoucherId = @Id",
                    new { Id = (int)voucher.VoucherId });
                if (globalCount >= (int)voucher.MaxUses)
                    return new POSVoucherResult { IsValid = false, Error = "This voucher has reached its maximum number of uses." };
            }

            // Step 5 — Per-user usage cap
            if (voucher.MaxUsesPerUser != null)
            {
                int userCount = await conn.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM VoucherUsage WHERE VoucherId = @Id AND UserId = @UserId",
                    new { Id = (int)voucher.VoucherId, UserId = userId });
                if (userCount >= (int)voucher.MaxUsesPerUser)
                    return new POSVoucherResult { IsValid = false, Error = "This customer has already used this voucher the maximum number of times." };
            }

            // Calculate discount
            string discountType  = (string)voucher.DiscountType;
            decimal discountValue = (decimal)voucher.DiscountValue;

            decimal discountAmount = discountType == "Percentage"
                ? Math.Round(subtotal * (discountValue / 100m), 2)
                : discountValue;

            discountAmount = Math.Min(discountAmount, subtotal);

            string formatted = discountType == "Percentage"
                ? $"{discountValue:0.##}% off"
                : $"\u20b1{discountAmount:N2} off";

            return new POSVoucherResult
            {
                IsValid           = true,
                VoucherId         = (int)voucher.VoucherId,
                VoucherCode       = (string)voucher.Code,
                DiscountType      = discountType,
                DiscountValue     = discountValue,
                DiscountAmount    = discountAmount,
                FormattedDiscount = formatted
            };
        }

        // ── Atomic POS sale ───────────────────────────────────────────────────

        public async Task<POSOrderResult> CreatePOSSaleAsync(
            int    userId,
            int    cashierId,
            string customerName,
            List<POSCartItem> items,
            string  paymentMethod,
            decimal cashReceived,
            decimal discountAmount,
            string? voucherCode)
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                // 1 — Validate stock for every line item
                foreach (var item in items)
                {
                    var stock = await conn.ExecuteScalarAsync<int>(
                        "SELECT StockQuantity FROM ProductVariant WHERE ProductVariantId = @Id",
                        new { Id = item.ProductVariantId }, tx);

                    if (stock < item.Quantity)
                        throw new InvalidOperationException(
                            $"Insufficient stock for \"{item.DisplayName}\". " +
                            $"Available: {stock}, requested: {item.Quantity}.");
                }

                // 2 — Generate unique OrderNumber (TBS-YYYY-NNNNN)
                int year   = DateTime.UtcNow.Year;
                int maxSeq = await conn.ExecuteScalarAsync<int>(
                    @"SELECT ISNULL(MAX(CAST(SUBSTRING(OrderNumber, 10, 5) AS INT)), 0)
                      FROM [Order]
                      WHERE OrderNumber LIKE @Pattern",
                    new { Pattern = $"TBS-{year}-%" }, tx);

                string orderNumber = $"TBS-{year}-{maxSeq + 1:D5}";

                // 3 — Compute totals
                decimal subtotal   = items.Sum(i => i.LineTotal);
                decimal grandTotal = Math.Max(0m, subtotal - discountAmount);

                // 4 — Insert Order row
                int orderId = await conn.ExecuteScalarAsync<int>(
                    @"INSERT INTO [Order]
                          (UserId, OrderNumber, OrderDate, OrderStatus,
                           SubTotal, DiscountAmount, ShippingFee,
                           ContactPhone, DeliveryInstructions, IsWalkIn,
                           CreatedAt, UpdatedAt)
                      VALUES
                          (@UserId, @OrderNumber, GETUTCDATE(), @Status,
                           @SubTotal, @Discount, 0,
                           NULL, NULL, 1,
                           GETUTCDATE(), GETUTCDATE());
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    new
                    {
                        UserId      = userId,
                        OrderNumber = orderNumber,
                        Status      = OrderStatuses.Delivered,
                        SubTotal    = subtotal,
                        Discount    = discountAmount
                    }, tx);

                // 5 — Insert OrderItems + deduct stock + log inventory
                foreach (var item in items)
                {
                    await conn.ExecuteAsync(
                        @"INSERT INTO OrderItem
                              (OrderId, ProductId, ProductVariantId, Quantity, UnitPrice)
                          VALUES
                              (@OrderId, @ProductId, @VariantId, @Qty, @Price)",
                        new
                        {
                            OrderId   = orderId,
                            ProductId = item.ProductId,
                            VariantId = item.ProductVariantId,
                            Qty       = item.Quantity,
                            Price     = item.UnitPrice
                        }, tx);

                    await conn.ExecuteAsync(
                        @"UPDATE ProductVariant
                          SET StockQuantity = StockQuantity - @Qty,
                              UpdatedAt     = GETUTCDATE()
                          WHERE ProductVariantId = @Id",
                        new { Qty = item.Quantity, Id = item.ProductVariantId }, tx);

                    await conn.ExecuteAsync(
                        @"INSERT INTO InventoryLog
                              (ProductId, ProductVariantId, OrderId,
                               ChangeQuantity, ChangeType, ChangedByUserId, Notes, CreatedAt)
                          VALUES
                              (@ProductId, @VariantId, @OrderId,
                               @Qty, @ChangeType, @CashierId, @Notes, GETUTCDATE())",
                        new
                        {
                            ProductId  = item.ProductId,
                            VariantId  = item.ProductVariantId,
                            OrderId    = orderId,
                            Qty        = -item.Quantity,
                            ChangeType = InventoryChangeTypes.Sale,
                            CashierId  = cashierId,
                            Notes      = $"POS Sale — {orderNumber}"
                        }, tx);
                }

                // 6 — Insert Payment record (Completed immediately for POS)
                await conn.ExecuteAsync(
                    @"INSERT INTO Payment
                          (OrderId, PaymentMethod, PaymentStage, PaymentStatus,
                           Amount, PaymentDate, CreatedAt)
                      VALUES
                          (@OrderId, @Method, 'Upfront', 'Completed',
                           @Amount, GETUTCDATE(), GETUTCDATE())",
                    new
                    {
                        OrderId = orderId,
                        Method  = paymentMethod,
                        Amount  = grandTotal
                    }, tx);

                // 7 — Record VoucherUsage when a voucher was applied
                if (!string.IsNullOrWhiteSpace(voucherCode) && discountAmount > 0)
                {
                    int? voucherId = await conn.ExecuteScalarAsync<int?>(
                        "SELECT VoucherId FROM Voucher WHERE Code = @Code",
                        new { Code = voucherCode.Trim().ToUpperInvariant() }, tx);

                    if (voucherId.HasValue)
                    {
                        await conn.ExecuteAsync(
                            @"INSERT INTO VoucherUsage
                                  (VoucherId, UserId, OrderId, DiscountAmount, UsedAt)
                              VALUES
                                  (@VoucherId, @UserId, @OrderId, @Discount, GETUTCDATE())",
                            new
                            {
                                VoucherId = voucherId.Value,
                                UserId    = userId,
                                OrderId   = orderId,
                                Discount  = discountAmount
                            }, tx);
                    }
                }

                await tx.CommitAsync();

                return new POSOrderResult
                {
                    OrderId        = orderId,
                    OrderNumber    = orderNumber,
                    Subtotal       = subtotal,
                    DiscountAmount = discountAmount,
                    GrandTotal     = grandTotal,
                    VoucherCode    = voucherCode ?? string.Empty,
                    PaymentMethod  = paymentMethod,
                    CashReceived   = cashReceived,
                    Change         = paymentMethod == POSPaymentMethods.Cash
                                         ? Math.Max(0m, cashReceived - grandTotal)
                                         : 0m,
                    CustomerName   = customerName,
                    CompletedAt    = DateTime.UtcNow,
                    Items          = new List<POSCartItem>(items)
                };
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
