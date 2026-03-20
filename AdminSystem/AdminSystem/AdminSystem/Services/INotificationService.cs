namespace AdminSystem.Services
{
    public interface INotificationService
    {
        /// <summary>
        /// Queues a notification row into the Notifications table.
        /// Dispatch is handled by the WebApplication background jobs.
        /// </summary>
        void Queue(string channel, string notifType, string recipient,
            string subject, string body, int userId,
            int? orderId = null, int? ticketId = null);
    }
}
