namespace AdminSystem_v2.Models
{
    /// <summary>
    /// Thrown when an order status change violates the allowed forward-only
    /// transition rules. Carries structured context for UI display and audit logging.
    /// </summary>
    public sealed class InvalidStatusTransitionException : Exception
    {
        public int    OrderId       { get; }
        public string CurrentStatus { get; }
        public string AttemptedStatus { get; }

        public InvalidStatusTransitionException(int orderId, string currentStatus, string attemptedStatus)
            : base($"Invalid status transition for order {orderId}: " +
                   $"'{currentStatus}' → '{attemptedStatus}' is not allowed. " +
                   (OrderStatuses.TerminalStatuses.Contains(currentStatus)
                       ? $"Order is in terminal state '{currentStatus}' and cannot be changed."
                       : "Reverting to a previous status is not allowed."))
        {
            OrderId         = orderId;
            CurrentStatus   = currentStatus;
            AttemptedStatus = attemptedStatus;
        }
    }
}
