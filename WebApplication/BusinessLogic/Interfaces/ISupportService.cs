// WebApplication/BusinessLogic/Interfaces/ISupportService.cs

using Microsoft.AspNetCore.Http;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for customer support ticket operations available in the WebApplication.
/// </summary>
public interface ISupportService
{
    Task<IReadOnlyList<SupportTicketListItemViewModel>> GetUserTicketsAsync(
        int userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SupportTicketViewModel>> GetByUserAsync(
        int userId, CancellationToken cancellationToken = default);

    Task<SupportTicketDetailViewModel?> GetTicketDetailAsync(
        int ticketId, int userId, CancellationToken cancellationToken = default);

    Task<SupportTicketViewModel?> GetDetailAsync(
        int ticketId, int userId, CancellationToken cancellationToken = default);

    Task<ServiceResult<int>> CreateTicketAsync(
        int userId, string category, string subject, string? description,
        int? orderId, IFormFile? attachment,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<int>> CreateAsync(
        int userId, SupportCreateViewModel vm,
        CancellationToken cancellationToken = default);

    Task<ServiceResult> AddReplyAsync(
        int ticketId, int userId, string message,
        CancellationToken cancellationToken = default);
}
