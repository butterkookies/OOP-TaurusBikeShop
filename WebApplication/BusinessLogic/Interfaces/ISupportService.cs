using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Interfaces
{
    public interface ISupportService
    {
        Task<SupportTicket> CreateTicketAsync(int userId, int? orderId, string subject, string message);
        Task<IEnumerable<SupportTicket>> GetUserTicketsAsync(int userId);
        Task<IEnumerable<SupportTicket>> GetAllTicketsAsync(); // admin
        Task<SupportTicket?> GetByIdAsync(int ticketId);
        Task RespondAsync(int ticketId, string response, string status);
    }
}
