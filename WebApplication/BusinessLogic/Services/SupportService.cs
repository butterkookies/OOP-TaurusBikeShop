// WebApplication/BusinessLogic/Services/SupportService.cs

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;
using WebApplication.Utilities;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="ISupportService"/> — customer support ticket creation
/// and retrieval for the WebApplication.
/// </summary>
public sealed class SupportService : ISupportService
{
    private readonly SupportRepository       _supportRepo;
    private readonly FileUploadHelper?       _fileUpload;
    private readonly INotificationService    _notifications;
    private readonly AppDbContext            _context;
    private readonly ILogger<SupportService> _logger;

    public SupportService(
        SupportRepository supportRepo, FileUploadHelper? fileUpload,
        INotificationService notifications, AppDbContext context,
        ILogger<SupportService> logger)
    {
        _supportRepo   = supportRepo   ?? throw new ArgumentNullException(nameof(supportRepo));
        _fileUpload    = fileUpload; // null when GCS credentials are not configured locally
        _notifications = notifications ?? throw new ArgumentNullException(nameof(notifications));
        _context       = context       ?? throw new ArgumentNullException(nameof(context));
        _logger        = logger        ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IReadOnlyList<SupportTicketListItemViewModel>> GetUserTicketsAsync(
        int userId, CancellationToken cancellationToken = default)
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

    public async Task<SupportTicketDetailViewModel?> GetTicketDetailAsync(
        int ticketId, int userId, CancellationToken cancellationToken = default)
    {
        SupportTicket? ticket = await _supportRepo.GetWithRepliesAsync(ticketId, cancellationToken);
        if (ticket is null || ticket.UserId != userId) return null;

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
            Replies       = ticket.Replies
                .OrderBy(r => r.CreatedAt)
                .Select(r => new SupportReplyViewModel
                {
                    ReplyId       = r.ReplyId,
                    AuthorName    = r.IsAdminReply ? "Support Team" : (r.User?.FirstName ?? "You"),
                    IsAdminReply  = r.IsAdminReply,
                    Message       = r.Message,
                    AttachmentUrl = r.AttachmentUrl,
                    CreatedAt     = r.CreatedAt
                }).ToList().AsReadOnly()
        };
    }

    public async Task<IReadOnlyList<SupportTicketViewModel>> GetByUserAsync(
        int userId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<SupportTicket> tickets =
            await _supportRepo.GetByUserAsync(userId, cancellationToken);

        return tickets.Select(t => new SupportTicketViewModel
        {
            TicketId    = t.TicketId,
            Category    = t.TicketCategory,
            Subject     = t.Subject,
            Status      = t.TicketStatus,
            OrderId     = t.OrderId,
            OrderNumber = t.Order?.OrderNumber,
            CreatedAt   = t.CreatedAt,
            UpdatedAt   = t.UpdatedAt
        }).ToList().AsReadOnly();
    }

    public async Task<SupportTicketViewModel?> GetDetailAsync(
        int ticketId, int userId, CancellationToken cancellationToken = default)
    {
        SupportTicket? ticket = await _supportRepo.GetWithRepliesAsync(ticketId, cancellationToken);
        if (ticket is null || ticket.UserId != userId) return null;

        return new SupportTicketViewModel
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
            UpdatedAt     = ticket.UpdatedAt,
            ResolvedAt    = ticket.ResolvedAt,
            Replies       = ticket.Replies
                .OrderBy(r => r.CreatedAt)
                .Select(r => new SupportReplyViewModel
                {
                    ReplyId       = r.ReplyId,
                    AuthorName    = r.IsAdminReply ? "Support Team" : (r.User?.FirstName ?? "You"),
                    IsAdminReply  = r.IsAdminReply,
                    Message       = r.Message,
                    AttachmentUrl = r.AttachmentUrl,
                    CreatedAt     = r.CreatedAt
                }).ToList().AsReadOnly()
        };
    }

    public async Task<ServiceResult<int>> CreateAsync(
        int userId, SupportCreateViewModel vm,
        CancellationToken cancellationToken = default)
    {
        return await CreateTicketAsync(
            userId, vm.Category ?? string.Empty, vm.Subject ?? string.Empty,
            vm.Description, vm.OrderId, null, cancellationToken);
    }

    public async Task<ServiceResult> AddReplyAsync(
        int ticketId, int userId, string message,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(message))
            return ServiceResult.Fail("Reply message cannot be empty.");

        try
        {
            SupportTicket? ticket = await _supportRepo.GetWithRepliesAsync(ticketId, cancellationToken);
            if (ticket is null || ticket.UserId != userId)
                return ServiceResult.Fail("Ticket not found.");

            if (ticket.TicketStatus is TicketStatuses.Resolved or TicketStatuses.Closed)
                return ServiceResult.Fail("This ticket is closed and cannot receive new replies.");

            SupportTicketReply reply = new()
            {
                TicketId     = ticketId,
                UserId       = userId,
                IsAdminReply = false,
                Message      = message.Trim(),
                CreatedAt    = DateTime.UtcNow
            };

            await _supportRepo.AddReplyAsync(reply, cancellationToken);

            // Queue SupportTicketReply notification (non-critical)
            try
            {
                User? user = await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
                if (user?.Email != null)
                {
                    await _notifications.QueueAsync(
                        channel:   NotifChannels.Email,
                        notifType: NotifTypes.SupportTicketReply,
                        recipient: user.Email,
                        subject:   $"Reply sent — Ticket #{ticketId}: {ticket.Subject}",
                        body:      $"Hi {user.FirstName}, your reply to support ticket \"" +
                                   $"{ticket.Subject}\" has been recorded. Our team will " +
                                   $"respond as soon as possible.\n\n— Taurus Bike Shop",
                        userId:    userId,
                        ticketId:  ticketId,
                        cancellationToken: cancellationToken);
                }
            }
            catch (Exception notifEx)
            {
                _logger.LogWarning(notifEx,
                    "Failed to queue SupportTicketReply notification for ticket {TicketId}.",
                    ticketId);
            }

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddReplyAsync failed for ticket {TicketId}, user {UserId}.", ticketId, userId);
            return ServiceResult.Fail("Unable to send reply. Please try again.");
        }
    }

    public async Task<ServiceResult<int>> CreateTicketAsync(
        int userId, string category, string subject, string? description,
        int? orderId, IFormFile? attachment, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(category))
            return ServiceResult<int>.Fail("Please select a category.");
        if (string.IsNullOrWhiteSpace(subject))
            return ServiceResult<int>.Fail("Please provide a subject.");
        if (subject.Length > 200)
            return ServiceResult<int>.Fail("Subject must be 200 characters or fewer.");

        try
        {
            string? attachmentUrl = null, attachmentBucket = null, attachmentPath = null;

            if (attachment is { Length: > 0 })
            {
                if (_fileUpload is null)
                    return ServiceResult<int>.Fail("File uploads are unavailable — Google Cloud Storage credentials are not configured.");

                UploadResult upload = await _fileUpload.UploadSupportAttachmentAsync(
                    attachment, $"support-attachments/user-{userId}", cancellationToken);
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

            // Queue SupportTicketCreated notification (non-critical)
            try
            {
                User? user = await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
                if (user?.Email != null)
                {
                    await _notifications.QueueAsync(
                        channel:   NotifChannels.Email,
                        notifType: NotifTypes.SupportTicketCreated,
                        recipient: user.Email,
                        subject:   $"Support ticket created — #{created.TicketId}: {subject.Trim()}",
                        body:      $"Hi {user.FirstName}, your support ticket has been created " +
                                   $"and our team will review it shortly.\n\n" +
                                   $"Category: {category.Trim()}\n" +
                                   $"Subject: {subject.Trim()}\n\n" +
                                   $"— Taurus Bike Shop",
                        userId:    userId,
                        ticketId:  created.TicketId,
                        cancellationToken: cancellationToken);
                }
            }
            catch (Exception notifEx)
            {
                _logger.LogWarning(notifEx,
                    "Failed to queue SupportTicketCreated notification for ticket {TicketId}.",
                    created.TicketId);
            }

            return ServiceResult<int>.Ok(created.TicketId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "CreateTicketAsync: attachment validation failed for user {UserId}.", userId);
            return ServiceResult<int>.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateTicketAsync failed for user {UserId}.", userId);
            return ServiceResult<int>.Fail("Unable to create ticket. Please try again.");
        }
    }
}
