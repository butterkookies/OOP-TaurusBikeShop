namespace AdminSystem.Models
{
    public class SupportTicketReply
    {
        public int    ReplyId       { get; set; }
        public int    TicketId      { get; set; }
        public int    UserId        { get; set; }
        public bool   IsAdminReply  { get; set; }
        public string Message       { get; set; }
        public string AttachmentUrl { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string AuthorName    { get; set; }
    }
}
