// WebApplication/Models/Entities/User.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a user account — covers registered customers, admin/staff, and the
/// anonymous walk-in POS placeholder. Email and PasswordHash are nullable to support
/// the walk-in placeholder row (IsWalkIn = true).
/// <para>
/// Circular FK: <see cref="DefaultAddressId"/> → <see cref="Address.AddressId"/> is
/// a deferred relationship added via ALTER TABLE and configured in AppDbContext
/// using <c>IsRequired(false)</c> to avoid EF Core circular-insert issues.
/// </para>
/// </summary>
public sealed class User
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int UserId { get; set; }

    /// <summary>
    /// Login email address. NULL for walk-in POS placeholder rows.
    /// Enforced unique via a filtered index on non-NULL values in AppDbContext.
    /// </summary>
    [MaxLength(255)]
    public string? Email { get; set; }

    /// <summary>
    /// BCrypt hash of the user's password. NULL for walk-in placeholder rows.
    /// Never log, serialize, or expose this value.
    /// </summary>
    [MaxLength(255)]
    public string? PasswordHash { get; set; }

    /// <summary>Customer or staff first name.</summary>
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Customer or staff last name.</summary>
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>Optional contact phone number.</summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// FK to the user's currently selected default address.
    /// Part of the circular FK pair: User → Address → User.
    /// Nullable — walk-in users and newly registered users before address creation
    /// have no default. Configured in AppDbContext via Fluent API with
    /// <c>IsRequired(false)</c>.
    /// </summary>
    public int? DefaultAddressId { get; set; }

    /// <summary>
    /// Indicates whether the account is active. Set to <c>false</c> to soft-delete
    /// or suspend a user without removing their order history.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// <c>true</c> for the anonymous walk-in POS placeholder user.
    /// Walk-in users have no email, no password, and no real address.
    /// </summary>
    public bool IsWalkIn { get; set; } = false;

    /// <summary>UTC timestamp when this account row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp of the most recent successful login.
    /// NULL until first login. Updated on every successful authentication.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Number of consecutive failed login attempts since the last success.
    /// Reset to 0 on successful login. Used to trigger account lockout.
    /// Added in schema v8.2.
    /// </summary>
    public int FailedLoginAttempts { get; set; } = 0;

    /// <summary>
    /// UTC time until which the account is locked out due to too many failed
    /// login attempts. NULL when the account is not locked.
    /// Added in schema v8.2.
    /// </summary>
    public DateTime? LockoutUntil { get; set; }

    /// <summary>
    /// Soft-delete flag. When true, the account is treated as deleted but
    /// the row is retained for audit purposes. Added in schema v8.2.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    // -------------------------------------------------------------------------
    // Navigation properties — all relationships configured via Fluent API
    // in AppDbContext.OnModelCreating, never via data annotations.
    // -------------------------------------------------------------------------

    /// <summary>
    /// The user's default address (circular FK target).
    /// Navigates to the single Address row pointed to by <see cref="DefaultAddressId"/>.
    /// Configured with <c>IsRequired(false)</c> and no cascade delete to break the
    /// circular dependency.
    /// </summary>
    public Address? DefaultAddress { get; set; }

    /// <summary>
    /// All shipping/billing addresses saved by this user, including immutable
    /// checkout snapshot rows (IsSnapshot = true).
    /// </summary>
    public ICollection<Address> Addresses { get; set; } = new List<Address>();

    /// <summary>
    /// Role assignments for this user. Junction rows to the Role table.
    /// Unique on (UserId, RoleId) — no duplicate role assignments.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>All orders placed by this user, including walk-in POS orders.</summary>
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    /// <summary>Active and historical shopping carts for this user.</summary>
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();

    /// <summary>
    /// Products saved to this user's wishlist.
    /// Unique on (UserId, ProductId) — one entry per product per user.
    /// </summary>
    public ICollection<Wishlist> Wishlist { get; set; } = new List<Wishlist>();

    /// <summary>Reviews submitted by this user against confirmed delivered orders.</summary>
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    /// <summary>
    /// Vouchers assigned to this user via the UserVoucher junction table.
    /// Controls per-user voucher access and optional per-user expiry overrides.
    /// </summary>
    public ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();

    /// <summary>
    /// POS terminal sessions opened by this user (cashier/staff only).
    /// Each session tracks shift start/end and running total sales.
    /// </summary>
    public ICollection<POS_Session> POSSessions { get; set; } = new List<POS_Session>();

    /// <summary>Support tickets raised by this user.</summary>
    public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();

    /// <summary>
    /// Outbound email/SMS notification rows addressed to this user.
    /// Inserted by the notification service; dispatched by the background job.
    /// </summary>
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    /// <summary>
    /// Audit log entries attributed to this user.
    /// NULL UserId entries in SystemLog represent automated or system-generated events.
    /// </summary>
    public ICollection<SystemLog> SystemLogs { get; set; } = new List<SystemLog>();

    /// <summary>Active login sessions for this user.</summary>
    public ICollection<ActiveSession> ActiveSessions { get; set; } = new List<ActiveSession>();
}