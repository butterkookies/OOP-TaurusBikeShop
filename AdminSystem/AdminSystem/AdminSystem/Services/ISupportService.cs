using System.Collections.Generic;
using AdminSystem.Models;

namespace AdminSystem.Services
{
    public interface ISupportService
    {
        IEnumerable<SupportTicket> GetAllTickets();
        IEnumerable<SupportTicket> GetTicketsByStatus(string status);
        SupportTicket GetTicketById(int ticketId);
        void AssignTicket(int ticketId, int assignedToUserId);
        void UpdateTicketStatus(int ticketId, string newStatus);
        void AddReply(int ticketId, string message, bool isAdminReply);
        void AddTask(SupportTask task);
        void UpdateTaskStatus(int taskId, string newStatus);
        void ResolveTicket(int ticketId);
        // NOTE: No RefundTask method — Taurus does not offer refunds.
    }
}
