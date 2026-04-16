// WebApplication/BusinessLogic/Interfaces/IPaymentService.cs

using Microsoft.AspNetCore.Http;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for payment submission and status management.
/// Payment methods: GCash and BankTransfer only — no Cash, no Card, no COD.
/// Flowchart: Part 5 — Payment Processing.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Submits a GCash payment proof screenshot for an order.
    /// Creates a <c>Payment</c> row (Status = VerificationPending) and a
    /// <c>GCashPayment</c> subtype row, then uploads the screenshot to GCS.
    /// Queues a <c>PaymentReceived</c> notification.
    /// </summary>
    /// <param name="orderId">The order being paid.</param>
    /// <param name="userId">The authenticated user submitting payment.</param>
    /// <param name="gcashRefNumber">The GCash reference number from the transaction.</param>
    /// <param name="screenshot">The uploaded proof-of-payment screenshot file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> indicating success or the specific error.
    /// </returns>
    Task<ServiceResult> SubmitGCashPaymentAsync(
        int orderId,
        int userId,
        string gcashRefNumber,
        IFormFile screenshot,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Submits a BPI bank transfer payment proof for an order.
    /// Creates a <c>Payment</c> row (Status = VerificationPending),
    /// a <c>BankTransferPayment</c> subtype row with a 24-hour verification
    /// deadline, and uploads the proof to GCS.
    /// Queues a <c>PaymentReceived</c> notification.
    /// </summary>
    /// <param name="orderId">The order being paid.</param>
    /// <param name="userId">The authenticated user submitting payment.</param>
    /// <param name="bpiReferenceNumber">The BPI transaction reference number.</param>
    /// <param name="proofFile">The uploaded proof-of-payment file (image or PDF).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<ServiceResult> SubmitBankTransferPaymentAsync(
        int orderId,
        int userId,
        string bpiReferenceNumber,
        IFormFile proofFile,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the payment details for a given order.
    /// Used by the payment page to show what has already been submitted.
    /// </summary>
    /// <param name="orderId">The order to retrieve payment details for.</param>
    /// <param name="userId">Used to verify the order belongs to this user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<PaymentDetailDto?> GetPaymentDetailAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Data transfer object carrying payment status information for the payment page.
/// <para>
/// The <c>StoreAccount*</c> fields carry the active store-side GCash / BPI
/// account the customer should send money to, so the payment view can show
/// "Pay to: …" without the controller touching the repo directly.
/// </para>
/// </summary>
public sealed record PaymentDetailDto(
    int     OrderId,
    string  OrderNumber,
    string  OrderStatus,
    decimal GrandTotal,
    string  PaymentMethod,
    string? PaymentStatus,
    string? ProofImageUrl,
    bool    HasExistingPayment,
    DateTime? VerificationDeadline,
    string? StoreAccountName,
    string? StoreAccountNumber,
    string? StoreAccountBankName,
    string? StoreAccountQrImageUrl,
    string? StoreAccountInstructions);