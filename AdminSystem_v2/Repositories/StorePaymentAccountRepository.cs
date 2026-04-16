using AdminSystem_v2.Models;
using Dapper;

namespace AdminSystem_v2.Repositories
{
    public class StorePaymentAccountRepository
        : Repository<StorePaymentAccount>, IStorePaymentAccountRepository
    {
        public async Task<IEnumerable<StorePaymentAccount>> GetAllAsync()
            => await QueryAsync<StorePaymentAccount>(
                @"SELECT
                      StorePaymentAccountId,
                      PaymentMethod,
                      AccountName,
                      AccountNumber,
                      BankName,
                      QrImageUrl,
                      Instructions,
                      IsActive,
                      DisplayOrder,
                      CreatedAt,
                      UpdatedAt
                  FROM StorePaymentAccount
                  ORDER BY PaymentMethod, DisplayOrder, StorePaymentAccountId");

        public async Task<int> InsertAsync(
            string  paymentMethod,
            string  accountName,
            string  accountNumber,
            string? bankName,
            string? qrImageUrl,
            string? instructions,
            bool    isActive,
            int     displayOrder)
        {
            await using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO StorePaymentAccount
                      (PaymentMethod, AccountName, AccountNumber, BankName,
                       QrImageUrl, Instructions, IsActive, DisplayOrder,
                       CreatedAt, UpdatedAt)
                  VALUES
                      (@PaymentMethod, @AccountName, @AccountNumber, @BankName,
                       @QrImageUrl, @Instructions, @IsActive, @DisplayOrder,
                       SYSUTCDATETIME(), SYSUTCDATETIME());
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new
                {
                    PaymentMethod = paymentMethod,
                    AccountName   = accountName.Trim(),
                    AccountNumber = accountNumber.Trim(),
                    BankName      = string.IsNullOrWhiteSpace(bankName)     ? null : bankName.Trim(),
                    QrImageUrl    = string.IsNullOrWhiteSpace(qrImageUrl)   ? null : qrImageUrl.Trim(),
                    Instructions  = string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim(),
                    IsActive      = isActive,
                    DisplayOrder  = displayOrder
                });
        }

        public async Task UpdateAsync(
            int     storePaymentAccountId,
            string  paymentMethod,
            string  accountName,
            string  accountNumber,
            string? bankName,
            string? qrImageUrl,
            string? instructions,
            bool    isActive,
            int     displayOrder)
        {
            await using var conn = GetConnection();
            await conn.ExecuteAsync(
                @"UPDATE StorePaymentAccount SET
                      PaymentMethod = @PaymentMethod,
                      AccountName   = @AccountName,
                      AccountNumber = @AccountNumber,
                      BankName      = @BankName,
                      QrImageUrl    = @QrImageUrl,
                      Instructions  = @Instructions,
                      IsActive      = @IsActive,
                      DisplayOrder  = @DisplayOrder,
                      UpdatedAt     = SYSUTCDATETIME()
                  WHERE StorePaymentAccountId = @Id",
                new
                {
                    Id            = storePaymentAccountId,
                    PaymentMethod = paymentMethod,
                    AccountName   = accountName.Trim(),
                    AccountNumber = accountNumber.Trim(),
                    BankName      = string.IsNullOrWhiteSpace(bankName)     ? null : bankName.Trim(),
                    QrImageUrl    = string.IsNullOrWhiteSpace(qrImageUrl)   ? null : qrImageUrl.Trim(),
                    Instructions  = string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim(),
                    IsActive      = isActive,
                    DisplayOrder  = displayOrder
                });
        }

        public async Task DeleteAsync(int storePaymentAccountId)
        {
            await using var conn = GetConnection();
            await conn.ExecuteAsync(
                "DELETE FROM StorePaymentAccount WHERE StorePaymentAccountId = @Id",
                new { Id = storePaymentAccountId });
        }

        public async Task DeactivateOtherActiveAsync(string paymentMethod, int excludeId)
        {
            await using var conn = GetConnection();
            await conn.ExecuteAsync(
                @"UPDATE StorePaymentAccount
                     SET IsActive  = 0,
                         UpdatedAt = SYSUTCDATETIME()
                   WHERE PaymentMethod = @PaymentMethod
                     AND IsActive      = 1
                     AND StorePaymentAccountId <> @ExcludeId",
                new { PaymentMethod = paymentMethod, ExcludeId = excludeId });
        }
    }
}
