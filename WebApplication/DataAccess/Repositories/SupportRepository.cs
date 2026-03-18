// WebApplication/DataAccess/Repositories/SupportRepository.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="SupportTicket"/>, <see cref="SupportTicketReply"/>,
/// and <see cref="SupportTask"/> entities.
/// The WebApplication creates tickets and allows customers to read their own.
/// The AdminSystem handles replies, task assignment, and resolution
/// via its own Dapper-based repository.
/// </summary>
public sealed class SupportRepository : Repository<SupportTicket>
{
    /// <inheritdoc/>
    public SupportRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns all support tickets raised by a specific user,
    /// ordered most recent first.
    /// </summary>
    /// <param name="userId">The user whose tickets to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user's tickets, most recent first.</returns>
    public async Task<IReadOnlyList<SupportTicket>> GetByUserAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.SupportTickets
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns a single ticket with its full reply thread and tasks loaded.
    /// Replies are ordered chronologically (oldest first) for thread display.
    /// Returns <c>null</c> if the ticket does not exist.
    /// </summary>
    /// <param name="ticketId">The ticket ID to load.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The ticket with <c>Replies</c> (ordered by CreatedAt ascending)
    /// and <c>Tasks</c> included, or <c>null</c>.
    /// </returns>
    public async Task<SupportTicket?> GetWithRepliesAsync(
        int ticketId,
        CancellationToken cancellationToken = default)
    {
        return await Context.SupportTickets
            .AsNoTracking()
            .Include(t => t.User)
            .Include(t => t.Order)
            .Include(t => t.AssignedTo)
            .Include(t => t.Replies.OrderBy(r => r.CreatedAt))
                .ThenInclude(r => r.User)
            .Include(t => t.Tasks)
                .ThenInclude(task => task.AssignedTo)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId, cancellationToken);
    }

    /// <summary>
    /// Inserts a new support ticket and returns it with its generated
    /// <c>TicketId</c> populated. Used by <c>SupportService.CreateTicketAsync</c>.
    /// </summary>
    /// <param name="ticket">The ticket to insert.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The inserted ticket with <c>TicketId</c> set.</returns>
    public async Task<SupportTicket> CreateTicketAsync(
        SupportTicket ticket,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ticket);

        await Context.SupportTickets.AddAsync(ticket, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return ticket;
    }
}