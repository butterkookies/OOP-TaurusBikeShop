using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Services
{
    public class SupportService : ISupportService
    {
        readonly AppDbContext _ctx;

        public SupportService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<SupportTicket> CreateTicketAsync(int userId, int? orderId, string subject, string message)
        {
            var ticket = new SupportTicket
            {
                UserId    = userId,
                OrderId   = orderId,
                Subject   = subject,
                Message   = message,
                Status    = "Open",
                CreatedAt = DateTime.UtcNow
            };

            _ctx.SupportTickets.Add(ticket);
            await _ctx.SaveChangesAsync();
            return ticket;
        }

        public async Task<IEnumerable<SupportTicket>> GetUserTicketsAsync(int userId)
        {
            return await _ctx.SupportTickets
                .Include(t => t.Order)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<SupportTicket>> GetAllTicketsAsync()
        {
            return await _ctx.SupportTickets
                .Include(t => t.User)
                .Include(t => t.Order)
                .OrderBy(t => t.Status)
                .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<SupportTicket?> GetByIdAsync(int ticketId)
        {
            return await _ctx.SupportTickets
                .Include(t => t.User)
                .Include(t => t.Order)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }

        public async Task RespondAsync(int ticketId, string response, string status)
        {
            var ticket = await _ctx.SupportTickets.FindAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket {ticketId} not found.");

            ticket.AdminResponse = response;
            ticket.Status        = status;
            ticket.UpdatedAt     = DateTime.UtcNow;

            await _ctx.SaveChangesAsync();
        }
    }
}
