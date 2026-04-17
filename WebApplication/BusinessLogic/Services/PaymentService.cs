// WebApplication/BusinessLogic/Services/PaymentService.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;


namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="IPaymentService"/> — GCash and BankTransfer
/// proof submission with Cloudinary upload, status transitions, and notification queuing.
/// Flowchart: Part 5 — Payment Processing.
/// </summary>
public sealed class PaymentService : IPaymentService
{
    private readonly AppDbContext                    _context;
    private readonly PaymentRepository               _paymentRepo;
    private readonly OrderRepository                 _orderRepo;
    private readonly StorePaymentAccountRepository   _storeAccountRepo;
    private readonly IPhotoService                   _photoService;
    private readonly INotificationService            _notifications;
    private readonly ILogger<PaymentService>         _logger;

    private const int BankTransferVerificationHours = 24;

    /// <inheritdoc/>
    public PaymentService(
        AppDbContext                   context,
        PaymentRepository              paymentRepo,
        OrderRepository                orderRepo,
        StorePaymentAccountRepository  storeAccountRepo,
        IPhotoService                  photoService,
        INotificationService           notifications,
        ILogger<PaymentService>        logger)
    {
        _context          = context          ?? throw new ArgumentNullException(nameof(context));
        _paymentRepo      = paymentRepo      ?? throw new ArgumentNullException(nameof(paymentRepo));
        _orderRepo        = orderRepo        ?? throw new ArgumentNullException(nameof(orderRepo));
        _storeAccountRepo = storeAccountRepo ?? throw new ArgumentNullException(nameof(storeAccountRepo));
        _photoService     = photoService     ?? throw new ArgumentNullException(nameof(photoService));
        _notifications    = notifications    ?? throw new ArgumentNullException(nameof(notifications));
        _logger           = logger           ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> SubmitGCashPaymentAsync(
        int orderId,
        int userId,
        string gcashRefNumber,
        Microsoft.AspNetCore.Http.IFormFile screenshot,
        CancellationToken cancellationToken = default)
    {
        // Guard: order exists and belongs to this user
        ServiceResult? guardResult = await ValidateOrderForPaymentAsync(
            orderId, userId, cancellationToken);
        if (guardResult != null) return guardResult;

        // Guard: no existing payment for this order
        bool alreadyPaid = await _context.Payments
            .AnyAsync(p => p.OrderId == orderId
                        && p.PaymentStatus != PaymentStatuses.Failed,
                cancellationToken);
        if (alreadyPaid)
            return ServiceResult.Fail("A payment has already been submitted for this order.");

        if (string.IsNullOrWhiteSpace(gcashRefNumber))
            return ServiceResult.Fail("GCash reference number is required.");

        try
        {
            // Upload screenshot to Cloudinary
            string screenshotUrl = await _photoService.UploadPaymentProofAsync(screenshot, orderId);

            // Build amount from order
            decimal amount = await GetOrderGrandTotalAsync(orderId, cancellationToken);

            // Snapshot the store GCash account the customer was told to pay to.
            StorePaymentAccount? storeAccount = await _storeAccountRepo
                .GetActiveForMethodAsync(PaymentMethods.GCash, cancellationToken);

            // Create Payment + GCashPayment subtype in one transaction
            Payment payment = new()
            {
                OrderId             = orderId,
                PaymentMethod       = PaymentMethods.GCash,
                PaymentStage        = PaymentStages.Upfront,
                PaymentStatus       = PaymentStatuses.VerificationPending,
                Amount              = amount,
                CreatedAt           = DateTime.UtcNow,
                PaidToAccountName   = storeAccount?.AccountName,
                PaidToAccountNumber = storeAccount?.AccountNumber,
                PaidToBankName      = storeAccount?.BankName
            };

            GCashPayment gcash = new()
            {
                GcashTransactionId = gcashRefNumber.Trim(),
                ScreenshotUrl      = screenshotUrl,
                StorageBucket      = "cloudinary",
                StoragePath        = string.Empty,
                SubmittedAt        = DateTime.UtcNow
            };

            await _paymentRepo.CreateWithSubtypeAsync(payment, gcash, cancellationToken);

            // Transition order to PendingVerification
            await _orderRepo.UpdateStatusAsync(
                orderId, OrderStatuses.PendingVerification, cancellationToken);

            // SystemLog
            await WriteSystemLogAsync(userId,
                SystemLogEvents.PaymentProcessed,
                $"GCash payment submitted for Order #{orderId}. Ref: {gcashRefNumber}",
                cancellationToken);

            // Queue notification
            await QueuePaymentNotificationAsync(
                userId, orderId, NotifTypes.PaymentReceived,
                "GCash Payment Received",
                "We received your GCash payment proof. Our team will verify it shortly.",
                cancellationToken);

            return ServiceResult.Ok();
        }
        catch (InvalidOperationException ex)
        {
            // File validation failures from IPhotoService
            return ServiceResult.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "SubmitGCashPaymentAsync failed for order {OrderId}.", orderId);
            return ServiceResult.Fail("Unable to submit payment. Please try again.");
        }
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> SubmitBankTransferPaymentAsync(
        int orderId,
        int userId,
        string bpiReferenceNumber,
        Microsoft.AspNetCore.Http.IFormFile proofFile,
        CancellationToken cancellationToken = default)
    {
        ServiceResult? guardResult = await ValidateOrderForPaymentAsync(
            orderId, userId, cancellationToken);
        if (guardResult != null) return guardResult;

        bool alreadyPaid = await _context.Payments
            .AnyAsync(p => p.OrderId == orderId
                        && p.PaymentStatus != PaymentStatuses.Failed,
                cancellationToken);
        if (alreadyPaid)
            return ServiceResult.Fail("A payment has already been submitted for this order.");

        // BPI reference number is optional — the customer may not have it at the time of
        // submission.  Presence is not enforced here; admin can follow up during verification.

        try
        {
            // Upload proof to Cloudinary
            string proofUrl = await _photoService.UploadPaymentProofAsync(proofFile, orderId);

            decimal amount = await GetOrderGrandTotalAsync(orderId, cancellationToken);
            DateTime deadline = DateTime.UtcNow.AddHours(BankTransferVerificationHours);

            // Snapshot the store BPI account the customer was told to pay to.
            StorePaymentAccount? storeAccount = await _storeAccountRepo
                .GetActiveForMethodAsync(PaymentMethods.BankTransfer, cancellationToken);

            Payment payment = new()
            {
                OrderId             = orderId,
                PaymentMethod       = PaymentMethods.BankTransfer,
                PaymentStage        = PaymentStages.Upfront,
                PaymentStatus       = PaymentStatuses.VerificationPending,
                Amount              = amount,
                CreatedAt           = DateTime.UtcNow,
                PaidToAccountName   = storeAccount?.AccountName,
                PaidToAccountNumber = storeAccount?.AccountNumber,
                PaidToBankName      = storeAccount?.BankName
            };

            BankTransferPayment bankTransfer = new()
            {
                BpiReferenceNumber   = bpiReferenceNumber.Trim(),
                ProofUrl             = proofUrl,
                ProofStorageBucket   = "cloudinary",
                ProofStoragePath     = string.Empty,
                VerificationDeadline = deadline,
                SubmittedAt          = DateTime.UtcNow
            };

            await _paymentRepo.CreateWithSubtypeAsync(payment, bankTransfer, cancellationToken);

            await _orderRepo.UpdateStatusAsync(
                orderId, OrderStatuses.PendingVerification, cancellationToken);

            await WriteSystemLogAsync(userId,
                SystemLogEvents.PaymentProcessed,
                $"Bank transfer submitted for Order #{orderId}. Ref: {bpiReferenceNumber}. Deadline: {deadline:u}",
                cancellationToken);

            await QueuePaymentNotificationAsync(
                userId, orderId, NotifTypes.PaymentReceived,
                "Bank Transfer Proof Received",
                $"We received your bank transfer proof. Our team will verify it within 24 hours (by {deadline:MMMM d, yyyy h:mm tt} UTC).",
                cancellationToken);

            return ServiceResult.Ok();
        }
        catch (InvalidOperationException ex)
        {
            // File validation failures from IPhotoService
            return ServiceResult.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "SubmitBankTransferPaymentAsync failed for order {OrderId}.", orderId);
            return ServiceResult.Fail("Unable to submit payment. Please try again.");
        }
    }

    /// <inheritdoc/>
    public async Task<PaymentDetailDto?> GetPaymentDetailAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        Order? order = await _orderRepo.GetOrderWithDetailsAsync(orderId, cancellationToken);
        if (order is null || order.UserId != userId)
            return null;

        // Compute grand total from items
        decimal subTotal = order.Items.Sum(i => i.UnitPrice * i.Quantity);
        decimal grandTotal = subTotal - order.DiscountAmount + order.ShippingFee;

        Payment? existingPayment = order.Payments
            .FirstOrDefault(p => p.PaymentStatus != PaymentStatuses.Failed);

        string? proofUrl        = null;
        string? paymentStatus   = existingPayment?.PaymentStatus;
        DateTime? verDeadline   = null;

        if (existingPayment?.GCashPayment != null)
            proofUrl = existingPayment.GCashPayment.ScreenshotUrl;
        else if (existingPayment?.BankTransferPayment != null)
        {
            proofUrl    = existingPayment.BankTransferPayment.ProofUrl;
            verDeadline = existingPayment.BankTransferPayment.VerificationDeadline;
        }

        // Carry the active store account info so the view can show
        // "Pay to: …". For already-submitted payments, prefer the snapshot on
        // Payment itself so the customer sees what they were actually asked
        // to pay to even if admin later rotates the account. Before a Payment
        // row exists we fall back to the method the customer chose at checkout,
        // persisted on Order.PaymentMethod (schema v9.3).
        string method = existingPayment?.PaymentMethod
                        ?? order.PaymentMethod
                        ?? string.Empty;

        string? storeName   = existingPayment?.PaidToAccountName;
        string? storeNumber = existingPayment?.PaidToAccountNumber;
        string? storeBank   = existingPayment?.PaidToBankName;
        string? storeQr     = null;
        string? storeInstr  = null;

        if (storeNumber is null && !string.IsNullOrEmpty(method))
        {
            StorePaymentAccount? active = await _storeAccountRepo
                .GetActiveForMethodAsync(method, cancellationToken);
            if (active is not null)
            {
                storeName   = active.AccountName;
                storeNumber = active.AccountNumber;
                storeBank   = active.BankName;
                storeQr     = active.QrImageUrl;
                storeInstr  = active.Instructions;
            }
        }

        return new PaymentDetailDto(
            OrderId:                 order.OrderId,
            OrderNumber:             order.OrderNumber,
            OrderStatus:             order.OrderStatus,
            GrandTotal:              grandTotal,
            PaymentMethod:           method,
            PaymentStatus:           paymentStatus,
            ProofImageUrl:           proofUrl,
            HasExistingPayment:      existingPayment != null,
            VerificationDeadline:    verDeadline,
            StoreAccountName:        storeName,
            StoreAccountNumber:      storeNumber,
            StoreAccountBankName:    storeBank,
            StoreAccountQrImageUrl:  storeQr,
            StoreAccountInstructions: storeInstr);
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    /// <summary>
    /// Returns null when the order is valid for payment submission,
    /// or a <see cref="ServiceResult"/> failure if validation fails.
    /// </summary>
    private async Task<ServiceResult?> ValidateOrderForPaymentAsync(
        int orderId, int userId, CancellationToken ct)
    {
        Order? order = await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderId == orderId, ct);

        if (order is null || order.UserId != userId)
            return ServiceResult.Fail("Order not found.");

        if (order.OrderStatus == OrderStatuses.Cancelled)
            return ServiceResult.Fail("This order has been cancelled.");

        if (order.OrderStatus == OrderStatuses.Delivered)
            return ServiceResult.Fail("This order has already been delivered.");

        return null;
    }

    private async Task<decimal> GetOrderGrandTotalAsync(int orderId, CancellationToken ct)
    {
        Order? order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderId == orderId, ct);

        if (order is null) return 0m;
        decimal sub = order.Items.Sum(i => i.UnitPrice * i.Quantity);
        return sub - order.DiscountAmount + order.ShippingFee;
    }

    // GetGCSFolder removed — Cloudinary folder is built inside IPhotoService.UploadPaymentProofAsync.

    private async Task WriteSystemLogAsync(
        int userId, string eventType, string details, CancellationToken ct)
    {
        SystemLog log = new()
        {
            UserId    = userId,
            EventType = eventType,
            EventDescription = details,
            CreatedAt = DateTime.UtcNow
        };
        await _context.SystemLogs.AddAsync(log, ct);
        await _context.SaveChangesAsync(ct);
    }

    private async Task QueuePaymentNotificationAsync(
        int userId, int orderId,
        string notifType, string subject, string body,
        CancellationToken ct)
    {
        User? user = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId, ct);
        if (user?.Email is null) return;

        await _notifications.QueueAsync(
            channel:   NotifChannels.Email,
            notifType: notifType,
            recipient: user.Email,
            subject:   subject,
            body:      body,
            userId:    userId,
            orderId:   orderId,
            cancellationToken: ct);
    }
}