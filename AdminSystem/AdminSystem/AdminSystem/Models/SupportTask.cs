namespace AdminSystem.Models
{
    public class SupportTask
    {
        public int    TaskId           { get; set; }
        public int    TicketId         { get; set; }
        public int?   AssignedToUserId { get; set; }
        public string TaskType         { get; set; }
        public string TaskStatus       { get; set; }
        public System.DateTime? DueDate      { get; set; }
        public string Notes            { get; set; }
        public System.DateTime  CreatedAt    { get; set; }
        public System.DateTime? CompletedAt  { get; set; }
    }
}
