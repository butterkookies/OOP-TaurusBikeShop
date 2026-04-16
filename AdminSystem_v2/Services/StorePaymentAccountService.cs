using System.Text.RegularExpressions;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class StorePaymentAccountService : IStorePaymentAccountService
    {
        // GCash: 11-digit PH mobile number, must start with "09".
        private static readonly Regex GCashNumberPattern = new(@"^09\d{9}$", RegexOptions.Compiled);
        // BPI deposit/savings: exactly 10 digits.
        private static readonly Regex BpiNumberPattern   = new(@"^\d{10}$",  RegexOptions.Compiled);

        private readonly IStorePaymentAccountRepository _repo;

        public StorePaymentAccountService(IStorePaymentAccountRepository repo)
            => _repo = repo;

        private static string CallerRole => App.CurrentUser?.Role ?? string.Empty;

        public Task<IEnumerable<StorePaymentAccount>> GetAllAsync()
            => _repo.GetAllAsync();

        public async Task<(bool ok, string error)> CreateAsync(
            string  paymentMethod,
            string  accountName,
            string  accountNumber,
            string? bankName,
            string? qrImageUrl,
            string? instructions,
            bool    isActive,
            int     displayOrder)
        {
            RoleGuard.RequireAdminOrManager(CallerRole);

            var (valid, err) = Validate(paymentMethod, accountName, accountNumber, bankName);
            if (!valid) return (false, err);

            int newId = await _repo.InsertAsync(
                paymentMethod, accountName, accountNumber,
                bankName, qrImageUrl, instructions, isActive, displayOrder);

            // Enforce single-active-per-method invariant.
            if (isActive)
                await _repo.DeactivateOtherActiveAsync(paymentMethod, newId);

            return (true, string.Empty);
        }

        public async Task<(bool ok, string error)> UpdateAsync(
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
            RoleGuard.RequireAdminOrManager(CallerRole);

            var (valid, err) = Validate(paymentMethod, accountName, accountNumber, bankName);
            if (!valid) return (false, err);

            // Deactivate any other active row for this method BEFORE updating,
            // so the unique-active index doesn't collide.
            if (isActive)
                await _repo.DeactivateOtherActiveAsync(paymentMethod, storePaymentAccountId);

            await _repo.UpdateAsync(
                storePaymentAccountId, paymentMethod, accountName, accountNumber,
                bankName, qrImageUrl, instructions, isActive, displayOrder);

            return (true, string.Empty);
        }

        public Task DeleteAsync(int storePaymentAccountId)
        {
            RoleGuard.RequireAdminOrManager(CallerRole);
            return _repo.DeleteAsync(storePaymentAccountId);
        }

        // ── Validation ────────────────────────────────────────────────────────

        private static (bool ok, string error) Validate(
            string  paymentMethod,
            string  accountName,
            string  accountNumber,
            string? bankName)
        {
            if (paymentMethod != "GCash" && paymentMethod != "BankTransfer")
                return (false, "Payment method must be GCash or BankTransfer.");

            if (string.IsNullOrWhiteSpace(accountName))
                return (false, "Account name is required.");
            if (accountName.Trim().Length > 150)
                return (false, "Account name cannot exceed 150 characters.");

            if (string.IsNullOrWhiteSpace(accountNumber))
                return (false, "Account number is required.");

            string trimmedNumber = accountNumber.Trim();

            if (paymentMethod == "GCash")
            {
                if (!GCashNumberPattern.IsMatch(trimmedNumber))
                    return (false, "GCash account number must be exactly 11 digits and start with 09 (e.g. 09123456789).");
            }
            else // BankTransfer
            {
                if (!BpiNumberPattern.IsMatch(trimmedNumber))
                    return (false, "BPI account number must be exactly 10 digits, numbers only.");

                if (string.IsNullOrWhiteSpace(bankName))
                    return (false, "Bank name is required for Bank Transfer accounts.");
            }

            return (true, string.Empty);
        }
    }
}
