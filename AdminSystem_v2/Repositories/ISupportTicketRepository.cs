namespace AdminSystem_v2.Repositories
{
    using AdminSystem_v2.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISupportTicketRepository
    {
        Task<IEnumerable<SupportTicket>> GetAllTicketsAsync();
        Task<SupportTicket?> GetTicketByIdAsync(int ticketId);
        Task<IEnumerable<SupportTicketReply>> GetRepliesAsync(int ticketId);
        Task UpdateTicketStatusAsync(int ticketId, string status, int? assignedToUserId);
        Task ResolveTicketAsync(int ticketId, int resolvedByUserId);
        Task<long> AddReplyAsync(SupportTicketReply reply);
    }
}
