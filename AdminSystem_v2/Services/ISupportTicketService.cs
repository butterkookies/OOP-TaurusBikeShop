using AdminSystem_v2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminSystem_v2.Services
{
    public interface ISupportTicketService
    {
        Task<IEnumerable<SupportTicket>> GetAllTicketsAsync();
        Task<SupportTicket?> GetTicketByIdAsync(int ticketId);
        Task<IEnumerable<SupportTicketReply>> GetRepliesAsync(int ticketId);
        Task UpdateStatusAsync(int ticketId, string status, int? assignedToUserId);
        Task ResolveTicketAsync(int ticketId, int resolvedByUserId);
        Task AddReplyAsync(int ticketId, int userId, string message);
    }
}
