using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminSystem_v2.Repositories
{
    public class SupportTicketRepository : Repository<SupportTicket>, ISupportTicketRepository
    {
        public async Task<IEnumerable<SupportTicket>> GetAllTicketsAsync()
        {
            await using var conn = GetConnection();
            string sql = @"
                SELECT t.TicketId, t.UserId, t.OrderId, t.TicketSource, 
                       t.TicketCategory, t.Subject, t.Description, 
                       t.TicketStatus, t.AssignedToUserId, t.ResolvedAt, t.CreatedAt, t.UpdatedAt,
                       (u.FirstName + ' ' + u.LastName) AS UserFullName,
                       u.Email AS UserEmail,
                       (a.FirstName + ' ' + a.LastName) AS AssignedToFullName
                FROM SupportTicket t
                JOIN [User] u ON t.UserId = u.UserId
                LEFT JOIN [User] a ON t.AssignedToUserId = a.UserId
                ORDER BY CASE
                    WHEN t.TicketStatus = 'Open' THEN 1
                    WHEN t.TicketStatus = 'AwaitingResponse' THEN 2
                    WHEN t.TicketStatus = 'InProgress' THEN 3
                    WHEN t.TicketStatus = 'Resolved' THEN 4
                    WHEN t.TicketStatus = 'Closed' THEN 5
                    ELSE 99 
                END, t.CreatedAt DESC";

            return await conn.QueryAsync<SupportTicket>(sql);
        }

        public async Task<SupportTicket?> GetTicketByIdAsync(int ticketId)
        {
            await using var conn = GetConnection();
            string sql = @"
                SELECT t.TicketId, t.UserId, t.OrderId, t.TicketSource, 
                       t.TicketCategory, t.Subject, t.Description,
                       t.AttachmentUrl, t.AttachmentBucket, t.AttachmentPath,
                       t.TicketStatus, t.AssignedToUserId, t.ResolvedAt, t.CreatedAt, t.UpdatedAt,
                       (u.FirstName + ' ' + u.LastName) AS UserFullName,
                       u.Email AS UserEmail,
                       (a.FirstName + ' ' + a.LastName) AS AssignedToFullName
                FROM SupportTicket t
                JOIN [User] u ON t.UserId = u.UserId
                LEFT JOIN [User] a ON t.AssignedToUserId = a.UserId
                WHERE t.TicketId = @TicketId";

            return await conn.QueryFirstOrDefaultAsync<SupportTicket>(sql, new { TicketId = ticketId });
        }

        public async Task<IEnumerable<SupportTicketReply>> GetRepliesAsync(int ticketId)
        {
            await using var conn = GetConnection();
            string sql = @"
                SELECT r.ReplyId, r.TicketId, r.UserId, r.IsAdminReply, 
                       r.Message, r.AttachmentUrl, r.AttachmentBucket, r.AttachmentPath, r.CreatedAt,
                       (u.FirstName + ' ' + u.LastName) AS UserFullName
                FROM SupportTicketReply r
                JOIN [User] u ON r.UserId = u.UserId
                WHERE r.TicketId = @TicketId
                ORDER BY r.CreatedAt ASC";

            return await conn.QueryAsync<SupportTicketReply>(sql, new { TicketId = ticketId });
        }

        public async Task UpdateTicketStatusAsync(int ticketId, string status, int? assignedToUserId)
        {
            await using var conn = GetConnection();
            string sql = @"
                UPDATE SupportTicket
                SET TicketStatus = @Status,
                    AssignedToUserId = COALESCE(@AssignedTo, AssignedToUserId),
                    UpdatedAt = GETUTCDATE()
                WHERE TicketId = @TicketId";

            await conn.ExecuteAsync(sql, new { TicketId = ticketId, Status = status, AssignedTo = assignedToUserId });
        }

        public async Task ResolveTicketAsync(int ticketId, int resolvedByUserId)
        {
            await using var conn = GetConnection();
            string sql = @"
                UPDATE SupportTicket
                SET TicketStatus = 'Resolved',
                    ResolvedAt = GETUTCDATE(),
                    UpdatedAt = GETUTCDATE(),
                    AssignedToUserId = COALESCE(AssignedToUserId, @ResolverId)
                WHERE TicketId = @TicketId";
                
            await conn.ExecuteAsync(sql, new { TicketId = ticketId, ResolverId = resolvedByUserId });
        }

        public async Task<long> AddReplyAsync(SupportTicketReply reply)
        {
            await using var conn = GetConnection();
            string sql = @"
                INSERT INTO SupportTicketReply
                    (TicketId, UserId, IsAdminReply, Message,
                     AttachmentUrl, AttachmentBucket, AttachmentPath, CreatedAt)
                VALUES
                    (@TicketId, @UserId, @IsAdminReply, @Message,
                     @AttachmentUrl, @AttachmentBucket, @AttachmentPath, GETUTCDATE());
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            return await conn.ExecuteScalarAsync<long>(sql, reply);
        }
    }
}
