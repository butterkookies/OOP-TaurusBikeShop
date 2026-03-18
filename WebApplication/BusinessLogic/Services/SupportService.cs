// WebApplication/BusinessLogic/Services/SupportService.cs

using Microsoft.AspNetCore.Http;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;
using WebApplication.Utilities;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="ISupportService"/> — customer support ticket creation
/// and retrieval for the WebApplication.
/// <para>
/// The WebApplication is responsible only for creating tickets and letting
/// customers read their own. Replies, status transitions, and resolution
/// are handled exclusively by the AdminSystem.
/// </para>
/// </summary>
public sealed class SupportService : ISupportService
{
    private readonly SupportRepository       _supportRepo;
    private readonly FileUploadHelper        _fileUpload;
    private readonly ILogger<SupportService> _logger;

    /// <inheritdoc/>
    public SupportService(
        SupportRepository       supportRepo,
        FileUploadHelper        fileUpload,
        ILogger<SupportService> logger)
    {
        _supportRepo = supportRepo ?? throw new ArgumentNullException(nameof(supportRepo));
        _fileUpload  = fileUpload  ?? throw new ArgumentNullException(nameof(fileUpload));
        _logger      = logger      ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<SupportTicketListItemViewModel>> GetUserTicketsAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<SupportTicket> tickets =
            await _supportRepo.GetByUserAsync(userId, cancellationToken);

        return tickets.Select(t => new SupportTicketListItemViewModel
        {
            TicketId    = t.TicketId,
            Category    = t.TicketCategory,
            Subject     = t.Subject,
            Status      = t.TicketStatus,
            OrderNumber = t.Order?.OrderNumber,
            CreatedAt   = t.CreatedAt,
            UpdatedAt   = t.UpdatedAt
        }).ToList().AsReadOnly();
    }

    /// <inheritdoc/>
    public async Task<SupportTicketDetailViewModel?> GetTicketDetailAsync(
        int ticketId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        SupportTicket? ticket =
            await _supportRepo.GetWithRepliesAsync(ticketId, cancellationToken);

        // Return null when the ticket does not exist or does not belong to this user
        if (ticket is null || ticket.UserId != userId)
            return null;

        IReadOnlyList<SupportReplyViewModel> replyVms = ticket.Replies
            .OrderBy(r => r.CreatedAt)
            .Select(r => new SupportReplyViewModel
            {
                ReplyId       = r.ReplyId,
                AuthorName    = r.IsAdminReply ? "Support Team" : (r.User?.FirstName ?? "You"),
                IsAdminReply  = r.IsAdminReply,
                Message       = r.Message,
                AttachmentUrl = r.AttachmentUrl,
                CreatedAt     = r.CreatedAt
            })
            .ToList()
            .AsReadOnly();

        return new SupportTicketDetailViewModel
        {
            TicketId      = ticket.TicketId,
            Category      = ticket.TicketCategory,
            Subject       = ticket.Subject,
            Description   = ticket.Description,
            AttachmentUrl = ticket.AttachmentUrl,
            Status        = ticket.TicketStatus,
            OrderId       = ticket.OrderId,
            OrderNumber   = ticket.Order?.OrderNumber,
            CreatedAt     = ticket.CreatedAt,
            ResolvedAt    = ticket.ResolvedAt,
            Replies       = replyVms
        };
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<int>> CreateTicketAsync(
        int userId,
        string category,
        string subject,
        string? description,
        int? orderId,
        IFormFile? attachment,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(category))
            return ServiceResult<int>.Fail("Please select a category.");

        if (string.IsNullOrWhiteSpace(subject))
            return ServiceResult<int>.Fail("Please provide a subject.");

        if (subject.Length > 200)
            return ServiceResult<int>.Fail("Subject must be 200 characters or fewer.");

        try
        {
            // Upload attachment to GCS if provided
            string? attachmentUrl    = null;
            string? attachmentBucket = null;
            string? attachmentPath   = null;

            if (attachment is { Length: > 0 })
            {
                string folder = $"support-attachments/user-{userId}";

                UploadResult upload = await _fileUpload.UploadSupportAttachmentAsync(
                    attachment, folder, cancellationToken);

                attachmentUrl    = upload.ImageUrl;
                attachmentBucket = upload.StorageBucket;
                attachmentPath   = upload.StoragePath;
            }

            SupportTicket ticket = new()
            {
                UserId           = userId,
                OrderId          = orderId,
                TicketSource     = TicketSources.Customer,
                TicketCategory   = category.Trim(),
                Subject          = subject.Trim(),
                Description      = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                AttachmentUrl    = attachmentUrl,
                AttachmentBucket = attachmentBucket,
                AttachmentPath   = attachmentPath,
                TicketStatus     = TicketStatuses.Open,
                CreatedAt        = DateTime.UtcNow
            };

            SupportTicket created = await _supportRepo.CreateTicketAsync(ticket, cancellationToken);

            return ServiceResult<int>.Ok(created.TicketId);
        }
        catch (InvalidOperationException ex)
        {
            // File validation failures from FileUploadHelper (MIME type / size)
            _logger.LogWarning(ex,
                "CreateTicketAsync: attachment validation failed for user {UserId}.", userId);
            return ServiceResult<int>.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "CreateTicketAsync failed for user {UserId}.", userId);
            return ServiceResult<int>.Fail("Unable to create ticket. Please try again.");
        }
    }
}
