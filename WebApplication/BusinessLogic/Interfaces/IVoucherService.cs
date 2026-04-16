// WebApplication/BusinessLogic/Interfaces/IVoucherService.cs

using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for voucher validation and redemption.
/// Validation is a 5-step chain — all steps must pass in order.
/// Redemption writes a <c>VoucherUsage</c> row at order creation.
/// </summary>
public interface IVoucherService
{
    /// <summary>
    /// Validates a voucher code against the 5-step chain:
    /// <list type="number">
    ///   <item>Code format (alphanumeric + hyphens, 3–50 chars).</item>
    ///   <item>Code exists in database and <c>IsActive = true</c>.</item>
    ///   <item>Current date is within <c>StartDate</c>–<c>EndDate</c> window.</item>
    ///   <item>Global usage count &lt; <c>MaxUses</c> (when MaxUses is set).</item>
    ///   <item>Per-user usage count &lt; <c>MaxUsesPerUser</c> (when set).</item>
    /// </list>
    /// Also calculates the discount amount against the provided order subtotal.
    /// </summary>
    /// <param name="code">The voucher code submitted by the customer.</param>
    /// <param name="userId">
    /// The authenticated user's ID — used for per-user usage check.
    /// </param>
    /// <param name="orderSubTotal">
    /// The current cart subtotal — used to calculate the discount amount
    /// and to check the minimum order value requirement if applicable.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="VoucherValidationResult"/> describing whether the voucher
    /// is valid and what the calculated discount amount is.
    /// </returns>
    Task<VoucherValidationResult> ValidateAsync(
        string code,
        int userId,
        decimal orderSubTotal,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Records a successful voucher redemption by inserting a
    /// <c>VoucherUsage</c> row. Must be called inside the
    /// <c>OrderService.CreateOrderAsync</c> transaction.
    /// </summary>
    /// <param name="code">The voucher code that was redeemed.</param>
    /// <param name="userId">The user who redeemed it.</param>
    /// <param name="orderId">The order the voucher was applied to.</param>
    /// <param name="discountAmount">The actual discount amount applied.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RedeemAsync(
        string code,
        int userId,
        int orderId,
        decimal discountAmount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetches all active vouchers assigned to the user.
    /// </summary>
    Task<IReadOnlyList<WebApplication.Models.Entities.UserVoucher>> GetActiveAssignedVouchersAsync(
        int userId,
        CancellationToken cancellationToken = default);
}