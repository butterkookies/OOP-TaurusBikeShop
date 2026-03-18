// WebApplication/BusinessLogic/Interfaces/ISupportService.cs

using Microsoft.AspNetCore.Http;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for customer support ticket operations available in the WebApplication.
/// <para>
/// The WebApplication creates tickets and allows customers to read their own.
/// The AdminSystem handles replies, task assignment, status updates, and resolution
/// via its own service layer.
/// </para>
/// </summary>
public interface ISupportService
{
    /// <summary>
    /// Returns all support tickets raised by the specified user, most recent first.
    /// </summary>
    /// <param name="userId">The authenticated customer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A read-only list of <see cref="SupportTicketListItemViewModel"/> ordered
    /// most recent first. Empty list when the user has no tickets.
    /// </returns>
    Task<IReadOnlyList<SupportTicketListItemViewModel>> GetUserTicketsAsync(
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the full detail view model for a single support ticket,
    /// including its reply thread.
    /// </summary>
    /// <param name="ticketId">The ticket ID to load.</param>
    /// <param name="userId">
    /// The authenticated customer — used to verify ownership.
    /// Returns <c>null</c> when the ticket does not belong to this user.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A populated <see cref="SupportTicketDetailViewModel"/>, or <c>null</c>
    /// when the ticket does not exist or does not belong to <paramref name="userId"/>.
    /// </returns>
    Task<SupportTicketDetailViewModel?> GetTicketDetailAsync(
        int ticketId,
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new support ticket on behalf of the authenticated customer.
    /// Optionally uploads an attachment to Google Cloud Storage.
    /// </summary>
    /// <param name="userId">The authenticated customer submitting the ticket.</param>
    /// <param name="category">Ticket category — must be a value from <c>TicketCategories</c>.</param>
    /// <param name="subject">Brief subject line (max 200 chars).</param>
    /// <param name="description">Full description of the issue. Optional.</param>
    /// <param name="orderId">
    /// FK to the related order. Pass <c>null</c> for general inquiries
    /// not tied to a specific order.
    /// </param>
    /// <param name="attachment">
    /// Optional file attachment (image or PDF, max 5 MB).
    /// When provided it is uploaded to GCS before the ticket row is inserted.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// <see cref="ServiceResult{T}"/> containing the new <c>TicketId</c> on success,
    /// or a failure result with an error message when validation fails or
    /// the upload/insert throws.
    /// </returns>
    Task<ServiceResult<int>> CreateTicketAsync(
        int userId,
        string category,
        string subject,
        string? description,
        int? orderId,
        IFormFile? attachment,
        CancellationToken cancellationToken = default);
}
