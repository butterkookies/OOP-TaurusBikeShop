// WebApplication/DataAccess/Context/AppDbContext.SupportComms.cs
// Entity configurations: Review, POS_Session, SupportTicket, SupportTicketReply, SupportTask, Notification, SystemLog

using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context;

public sealed partial class AppDbContext
{
    private static void ConfigureReview(ModelBuilder mb)
    {
        mb.Entity<Review>(e =>
        {
            e.ToTable("Review");
            e.HasKey(r => r.ReviewId);

            e.HasIndex(r => r.ProductId)
                .HasDatabaseName("IX_Review_ProductId");

            e.HasIndex(r => r.UserId)
                .HasDatabaseName("IX_Review_UserId");

            e.HasIndex(r => r.OrderId)
                .HasDatabaseName("IX_Review_OrderId");

            e.HasIndex(r => r.Rating)
                .HasDatabaseName("IX_Review_Rating");

            e.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(r => r.Product)
                .WithMany()
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(r => r.Order)
                .WithMany(o => o.Reviews)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigurePOSSession(ModelBuilder mb)
    {
        mb.Entity<POS_Session>(e =>
        {
            e.ToTable("POS_Session");
            e.HasKey(p => p.POSSessionId);

            e.Property(p => p.TotalSales).HasPrecision(18, 2);

            e.HasIndex(p => p.UserId)
                .HasDatabaseName("IX_POS_Session_UserId");

            e.HasIndex(p => p.ShiftStart)
                .HasDatabaseName("IX_POS_Session_ShiftStart");

            e.HasIndex(p => p.ShiftEnd)
                .HasDatabaseName("IX_POS_Session_ShiftEnd");

            e.HasOne(p => p.User)
                .WithMany(u => u.POSSessions)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureSupportTicket(ModelBuilder mb)
    {
        mb.Entity<SupportTicket>(e =>
        {
            e.ToTable("SupportTicket");
            e.HasKey(t => t.TicketId);

            e.HasIndex(t => t.UserId)
                .HasDatabaseName("IX_Ticket_UserId");

            e.HasIndex(t => t.TicketStatus)
                .HasDatabaseName("IX_Ticket_Status");

            e.HasIndex(t => t.TicketCategory)
                .HasDatabaseName("IX_Ticket_Category");

            e.HasIndex(t => t.CreatedAt)
                .HasDatabaseName("IX_Ticket_CreatedAt");

            e.HasIndex(t => t.OrderId)
                .HasFilter("[OrderId] IS NOT NULL")
                .HasDatabaseName("IX_Ticket_OrderId");

            e.HasIndex(t => t.AssignedToUserId)
                .HasFilter("[AssignedToUserId] IS NOT NULL")
                .HasDatabaseName("IX_Ticket_AssignedTo");

            // Two separate FK relationships to User — must be configured
            // explicitly to avoid EF Core ambiguity
            e.HasOne(t => t.User)
                .WithMany(u => u.SupportTickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(t => t.Order)
                .WithMany(o => o.SupportTickets)
                .HasForeignKey(t => t.OrderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureSupportTicketReply(ModelBuilder mb)
    {
        mb.Entity<SupportTicketReply>(e =>
        {
            e.ToTable("SupportTicketReply");
            e.HasKey(r => r.ReplyId);

            e.HasIndex(r => r.TicketId)
                .HasDatabaseName("IX_Reply_TicketId");

            e.HasIndex(r => r.UserId)
                .HasDatabaseName("IX_Reply_UserId");

            e.HasIndex(r => r.CreatedAt)
                .HasDatabaseName("IX_Reply_CreatedAt");

            e.HasOne(r => r.Ticket)
                .WithMany(t => t.Replies)
                .HasForeignKey(r => r.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureSupportTask(ModelBuilder mb)
    {
        mb.Entity<SupportTask>(e =>
        {
            e.ToTable("SupportTask");
            e.HasKey(t => t.TaskId);

            e.HasIndex(t => t.TicketId)
                .HasDatabaseName("IX_SupportTask_TicketId");

            e.HasIndex(t => t.TaskStatus)
                .HasDatabaseName("IX_SupportTask_TaskStatus");

            e.HasIndex(t => t.AssignedToUserId)
                .HasFilter("[AssignedToUserId] IS NOT NULL")
                .HasDatabaseName("IX_SupportTask_AssignedToUserId");

            e.HasIndex(t => t.DueDate)
                .HasFilter("[DueDate] IS NOT NULL")
                .HasDatabaseName("IX_SupportTask_DueDate");

            e.HasOne(t => t.Ticket)
                .WithMany(st => st.Tasks)
                .HasForeignKey(t => t.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureNotification(ModelBuilder mb)
    {
        mb.Entity<Notification>(e =>
        {
            e.ToTable("Notification");
            e.HasKey(n => n.NotificationId);

            e.HasIndex(n => n.UserId)
                .HasDatabaseName("IX_Notif_UserId");

            e.HasIndex(n => n.Status)
                .HasDatabaseName("IX_Notif_Status");

            e.HasIndex(n => new { n.Status, n.CreatedAt })
                .HasFilter("[Status] = 'Pending'")
                .HasDatabaseName("IX_Notif_Pending");

            e.HasIndex(n => n.Channel)
                .HasDatabaseName("IX_Notif_Channel");

            e.HasIndex(n => n.CreatedAt)
                .HasDatabaseName("IX_Notif_CreatedAt");

            e.HasIndex(n => n.OrderId)
                .HasFilter("[OrderId] IS NOT NULL")
                .HasDatabaseName("IX_Notif_OrderId");

            e.HasIndex(n => n.TicketId)
                .HasFilter("[TicketId] IS NOT NULL")
                .HasDatabaseName("IX_Notif_TicketId");

            e.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(n => n.Order)
                .WithMany(o => o.Notifications)
                .HasForeignKey(n => n.OrderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(n => n.Ticket)
                .WithMany(t => t.Notifications)
                .HasForeignKey(n => n.TicketId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureSystemLog(ModelBuilder mb)
    {
        mb.Entity<SystemLog>(e =>
        {
            e.ToTable("SystemLog");
            e.HasKey(s => s.SystemLogId);

            e.HasIndex(s => s.UserId)
                .HasDatabaseName("IX_SystemLog_UserId");

            e.HasIndex(s => s.EventType)
                .HasDatabaseName("IX_SystemLog_EventType");

            e.HasIndex(s => s.CreatedAt)
                .HasDatabaseName("IX_SystemLog_CreatedAt");

            e.HasOne(s => s.User)
                .WithMany(u => u.SystemLogs)
                .HasForeignKey(s => s.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
