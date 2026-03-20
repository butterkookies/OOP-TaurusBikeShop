using AdminSystem.Helpers;
using Dapper;

namespace AdminSystem.Services
{
    public class NotificationService : INotificationService
    {
        private const string StatusPending = "Pending";

        public void Queue(string channel, string notifType, string recipient,
            string subject, string body, int userId,
            int? orderId = null, int? ticketId = null)
        {
            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    @"INSERT INTO Notifications
                        (UserId, Channel, NotifType, Recipient,
                         Subject, Body, Status, RetryCount,
                         OrderId, TicketId, CreatedAt)
                      VALUES
                        (@UserId, @Channel, @NotifType, @Recipient,
                         @Subject, @Body, @Status, 0,
                         @OrderId, @TicketId, GETUTCDATE())",
                    new { UserId    = userId,
                          Channel   = channel,
                          NotifType = notifType,
                          Recipient = recipient,
                          Subject   = subject,
                          Body      = body,
                          Status    = StatusPending,
                          OrderId   = orderId,
                          TicketId  = ticketId });
            }
        }
    }
}
