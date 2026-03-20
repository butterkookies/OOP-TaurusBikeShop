// AdminSystem/Models/SupportTicket.cs
using System;
using System.Collections.Generic;

namespace AdminSystem.Models
{
    public class SupportTicket
    {
        public int     TicketId           { get; set; }
        public int     UserId             { get; set; }
        public int?    OrderId            { get; set; }
        public int?    AssignedToUserId   { get; set; }
        public string  Subject            { get; set; } = string.Empty;
        public string  TicketCategory     { get; set; } = string.Empty;
        public string  TicketStatus       { get; set; } = string.Empty;
        public string  TicketSource       { get; set; } = string.Empty;
        public DateTime  CreatedAt        { get; set; }
        public DateTime? ResolvedAt       { get; set; }

        public string CustomerName    { get; set; }
        public string AssignedToName  { get; set; }
        public List<SupportTicketReply> Replies { get; set; } = new List<SupportTicketReply>();
        public List<SupportTask>        Tasks   { get; set; } = new List<SupportTask>();
    }

    public static class TicketStatuses
    {
        public const string Open             = "Open";
        public const string InProgress       = "InProgress";
        public const string AwaitingResponse = "AwaitingResponse";
        public const string Resolved         = "Resolved";
        public const string Closed           = "Closed";
    }

    public static class TicketCategories
    {
        public const string DamagedItem     = "DamagedItem";
        public const string WrongItem       = "WrongItem";
        public const string DeliveryIssue   = "DeliveryIssue";
        public const string PaymentIssue    = "PaymentIssue";
        public const string ProductInquiry  = "ProductInquiry";
        public const string ReturnRefund    = "ReturnRefund";
        public const string General         = "General";
    }

    public static class SupportTaskTypes
    {
        public const string ShipReplacement  = "ShipReplacement";
        public const string ArrangeReturn    = "ArrangeReturn";
        public const string ContactSupplier  = "ContactSupplier";
        public const string Other            = "Other";
        // IssueRefund intentionally omitted — Taurus does not offer refunds
    }

    public static class SupportTaskStatuses
    {
        public const string Pending    = "Pending";
        public const string InProgress = "InProgress";
        public const string Done       = "Done";
        public const string Cancelled  = "Cancelled";
    }
}
