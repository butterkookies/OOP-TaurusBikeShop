using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminSystem_v2.Services
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly ISupportTicketRepository _supportRepo;

        public SupportTicketService(ISupportTicketRepository supportRepo)
        {
            _supportRepo = supportRepo;
        }

        public Task<IEnumerable<SupportTicket>> GetAllTicketsAsync()
        {
            return _supportRepo.GetAllTicketsAsync();
        }

        public Task<SupportTicket?> GetTicketByIdAsync(int ticketId)
        {
            return _supportRepo.GetTicketByIdAsync(ticketId);
        }

        public Task<IEnumerable<SupportTicketReply>> GetRepliesAsync(int ticketId)
        {
            return _supportRepo.GetRepliesAsync(ticketId);
        }

        public Task UpdateStatusAsync(int ticketId, string status, int? assignedToUserId)
        {
            return _supportRepo.UpdateTicketStatusAsync(ticketId, status, assignedToUserId);
        }

        public Task ResolveTicketAsync(int ticketId, int resolvedByUserId)
        {
            return _supportRepo.ResolveTicketAsync(ticketId, resolvedByUserId);
        }

        public Task AddReplyAsync(int ticketId, int userId, string message)
        {
            var reply = new SupportTicketReply
            {
                TicketId = ticketId,
                UserId = userId,
                IsAdminReply = true, // We are admin
                Message = message,
                // Attachments are null by default for now
            };
            return _supportRepo.AddReplyAsync(reply);
        }
    }
}
