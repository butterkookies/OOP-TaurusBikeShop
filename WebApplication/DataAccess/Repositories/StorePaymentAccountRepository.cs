// WebApplication/DataAccess/Repositories/StorePaymentAccountRepository.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="StorePaymentAccount"/> — the store-owned
/// GCash / BPI account rows displayed to customers on the payment page.
/// </summary>
public sealed class StorePaymentAccountRepository : Repository<StorePaymentAccount>
{
    /// <inheritdoc/>
    public StorePaymentAccountRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns the single active account for the given payment method, or
    /// <c>null</c> when admin has not configured one. Enforced by filtered
    /// unique index <c>UX_StorePaymentAccount_ActivePerMethod</c>.
    /// </summary>
    public async Task<StorePaymentAccount?> GetActiveForMethodAsync(
        string paymentMethod,
        CancellationToken cancellationToken = default)
    {
        return await Context.StorePaymentAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(
                a => a.PaymentMethod == paymentMethod && a.IsActive,
                cancellationToken);
    }
}
