// WebApplication/Models/ViewModels/NotificationViewModel.cs

namespace WebApplication.Models.ViewModels;

/// <summary>
/// View model for a single notification item on the Notifications page.
/// </summary>
public sealed class NotificationViewModel
{
    public int NotificationId { get; set; }
    public string NotifType { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    // Related entity IDs for linking
    public int? OrderId { get; set; }
    public int? TicketId { get; set; }

    // Computed helpers
    public string TimeAgo => FormatTimeAgo(CreatedAt);

    /// <summary>Returns a human-friendly icon name based on NotifType.</summary>
    public string IconClass => NotifType switch
    {
        "WelcomeEmail"          => "user-plus",
        "OrderConfirmation"     => "package",
        "PaymentReceived"       => "credit-card",
        "PaymentRejected"       => "x-circle",
        "PaymentHeld"           => "alert-triangle",
        "TrackingUpdate"        => "truck",
        "ReadyForPickup"        => "map-pin",
        "PickupExpiry"          => "clock",
        "DeliveryDelay"         => "alert-circle",
        "DeliveryConfirmation"  => "check-circle",
        "WishlistRestock"       => "heart",
        "SupportTicketCreated"  => "message-square",
        "SupportTicketReply"    => "message-circle",
        "SupportTicketResolved" => "check-square",
        "LowStockAlert"         => "alert-triangle",
        "PendingOrderAlert"     => "clock",
        "PendingOrderReminder"  => "clock",
        "OrderAutoCancelled"    => "x-circle",
        _                       => "bell"
    };

    /// <summary>Returns a human-friendly label for the notification type.</summary>
    public string TypeLabel => NotifType switch
    {
        "WelcomeEmail"          => "Welcome",
        "OrderConfirmation"     => "Order Confirmed",
        "PaymentReceived"       => "Payment Received",
        "PaymentRejected"       => "Payment Rejected",
        "PaymentHeld"           => "Payment On Hold",
        "TrackingUpdate"        => "Tracking Update",
        "ReadyForPickup"        => "Ready for Pickup",
        "PickupExpiry"          => "Pickup Expiring",
        "DeliveryDelay"         => "Delivery Delayed",
        "DeliveryConfirmation"  => "Delivered",
        "WishlistRestock"       => "Back in Stock",
        "SupportTicketCreated"  => "Ticket Created",
        "SupportTicketReply"    => "Ticket Reply",
        "SupportTicketResolved" => "Ticket Resolved",
        "LowStockAlert"         => "Low Stock",
        "PendingOrderAlert"     => "Order Pending",
        "PendingOrderReminder"  => "Payment Reminder",
        "OrderAutoCancelled"    => "Order Cancelled",
        _                       => "Notification"
    };

    private static string FormatTimeAgo(DateTime utcTime)
    {
        TimeSpan diff = DateTime.UtcNow - utcTime;
        if (diff.TotalMinutes < 1)  return "Just now";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes}m ago";
        if (diff.TotalHours < 24)   return $"{(int)diff.TotalHours}h ago";
        if (diff.TotalDays < 7)     return $"{(int)diff.TotalDays}d ago";
        return utcTime.ToString("MMM d, yyyy");
    }
}

/// <summary>
/// View model for the Notifications list page with pagination.
/// </summary>
public sealed class NotificationListViewModel
{
    public IReadOnlyList<NotificationViewModel> Notifications { get; set; } = [];
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int UnreadCount { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}
