using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdminSystem.Helpers;
using AdminSystem.Models;
using Dapper;

namespace AdminSystem.Services
{
    public class SupportService : ISupportService
    {
        public IEnumerable<SupportTicket> GetAllTickets()
        {
            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                return conn.Query<SupportTicket>(
                    @"SELECT t.*,
                             u.FirstName + ' ' + u.LastName AS CustomerName,
                             a.FirstName + ' ' + a.LastName AS AssignedToName
                      FROM SupportTicket t
                      INNER JOIN [User] u ON t.UserId = u.UserId
                      LEFT  JOIN [User] a ON t.AssignedToUserId = a.UserId
                      ORDER BY t.CreatedAt DESC");
            }
        }

        public IEnumerable<SupportTicket> GetTicketsByStatus(string status)
        {
            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                return conn.Query<SupportTicket>(
                    @"SELECT t.*,
                             u.FirstName + ' ' + u.LastName AS CustomerName,
                             a.FirstName + ' ' + a.LastName AS AssignedToName
                      FROM SupportTicket t
                      INNER JOIN [User] u ON t.UserId = u.UserId
                      LEFT  JOIN [User] a ON t.AssignedToUserId = a.UserId
                      WHERE t.TicketStatus = @Status
                      ORDER BY t.CreatedAt DESC",
                    new { Status = status });
            }
        }

        public SupportTicket GetTicketById(int ticketId)
        {
            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                SupportTicket ticket = conn.QueryFirstOrDefault<SupportTicket>(
                    @"SELECT t.*,
                             u.FirstName + ' ' + u.LastName AS CustomerName,
                             a.FirstName + ' ' + a.LastName AS AssignedToName
                      FROM SupportTicket t
                      INNER JOIN [User] u ON t.UserId = u.UserId
                      LEFT  JOIN [User] a ON t.AssignedToUserId = a.UserId
                      WHERE t.TicketId = @Id",
                    new { Id = ticketId });

                if (ticket == null) return null;

                ticket.Replies = conn.Query<SupportTicketReply>(
                        @"SELECT r.*,
                                 u.FirstName + ' ' + u.LastName AS AuthorName
                          FROM SupportTicketReply r
                          INNER JOIN [User] u ON r.UserId = u.UserId
                          WHERE r.TicketId = @Id
                          ORDER BY r.CreatedAt ASC",
                        new { Id = ticketId }).ToList();

                ticket.Tasks = conn.Query<SupportTask>(
                        "SELECT * FROM SupportTask WHERE TicketId = @Id ORDER BY CreatedAt ASC",
                        new { Id = ticketId }).ToList();

                return ticket;
            }
        }

        public void AssignTicket(int ticketId, int assignedToUserId)
        {
            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    @"UPDATE SupportTicket
                      SET AssignedToUserId = @UserId,
                          TicketStatus     = @Status
                      WHERE TicketId = @Id",
                    new { UserId = assignedToUserId,
                          Status = TicketStatuses.InProgress,
                          Id     = ticketId });
            }
        }

        public void UpdateTicketStatus(int ticketId, string newStatus)
        {
            switch (newStatus)
            {
                case TicketStatuses.Open:
                case TicketStatuses.InProgress:
                case TicketStatuses.AwaitingResponse:
                case TicketStatuses.Resolved:
                case TicketStatuses.Closed:
                    break;
                default:
                    throw new ArgumentException(
                        "Invalid ticket status: " + newStatus);
            }

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    "UPDATE SupportTicket SET TicketStatus = @Status WHERE TicketId = @Id",
                    new { Status = newStatus, Id = ticketId });
            }
        }

        public void AddReply(int ticketId, string message, bool isAdminReply)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Reply message cannot be empty.");

            int userId = App.CurrentUser != null ? App.CurrentUser.UserId : 0;

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (IDbTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    conn.Execute(
                        @"INSERT INTO SupportTicketReply
                            (TicketId, UserId, IsAdminReply, Message, CreatedAt)
                          VALUES
                            (@TicketId, @UserId, @IsAdmin, @Message, GETUTCDATE())",
                        new { TicketId = ticketId, UserId = userId,
                              IsAdmin  = isAdminReply, Message = message }, tx);

                    // If admin replies, move ticket to AwaitingResponse
                    if (isAdminReply)
                        conn.Execute(
                            @"UPDATE SupportTicket
                              SET TicketStatus = @Status
                              WHERE TicketId = @Id
                                AND TicketStatus NOT IN (@S1, @S2)",
                            new { Status = TicketStatuses.AwaitingResponse,
                                  Id     = ticketId,
                                  S1     = TicketStatuses.Resolved,
                                  S2     = TicketStatuses.Closed }, tx);

                    tx.Commit();
                }
                catch { tx.Rollback(); throw; }
            }
        }

        public void AddTask(SupportTask task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            // IssueRefund is NOT a valid task type — Taurus does not offer refunds
            switch (task.TaskType)
            {
                case SupportTaskTypes.ShipReplacement:
                case SupportTaskTypes.ArrangeReturn:
                case SupportTaskTypes.ContactSupplier:
                case SupportTaskTypes.Other:
                    break;
                default:
                    throw new ArgumentException(
                        "Invalid task type: " + task.TaskType);
            }

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    @"INSERT INTO SupportTask
                        (TicketId, AssignedToUserId, TaskType, TaskStatus,
                         DueDate, Notes, CreatedAt)
                      VALUES
                        (@TicketId, @AssignedTo, @TaskType, @TaskStatus,
                         @DueDate, @Notes, GETUTCDATE())",
                    new { task.TicketId, AssignedTo = task.AssignedToUserId,
                          task.TaskType,
                          TaskStatus = SupportTaskStatuses.Pending,
                          task.DueDate, task.Notes });
            }
        }

        public void UpdateTaskStatus(int taskId, string newStatus)
        {
            switch (newStatus)
            {
                case SupportTaskStatuses.Pending:
                case SupportTaskStatuses.InProgress:
                case SupportTaskStatuses.Done:
                case SupportTaskStatuses.Cancelled:
                    break;
                default:
                    throw new ArgumentException(
                        "Invalid task status: " + newStatus);
            }

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                if (newStatus == SupportTaskStatuses.Done)
                    conn.Execute(
                        @"UPDATE SupportTask
                          SET TaskStatus = @Status, CompletedAt = GETUTCDATE()
                          WHERE TaskId = @Id",
                        new { Status = newStatus, Id = taskId });
                else
                    conn.Execute(
                        "UPDATE SupportTask SET TaskStatus = @Status WHERE TaskId = @Id",
                        new { Status = newStatus, Id = taskId });
            }
        }

        public void ResolveTicket(int ticketId)
        {
            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    @"UPDATE SupportTicket
                      SET TicketStatus = @Status,
                          ResolvedAt   = GETUTCDATE()
                      WHERE TicketId = @Id",
                    new { Status = TicketStatuses.Resolved, Id = ticketId });
            }
        }
    }
}
