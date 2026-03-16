// WebApplication/Models/Entities/UserVoucher.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Assigns a voucher to a specific user, controlling per-user voucher access.
/// Not all vouchers require a UserVoucher row — public vouchers (e.g. promotional
/// codes shared on social media) can be used by any authenticated user without
/// a UserVoucher assignment. Assigned vouchers are visible in the user's
/// voucher wallet.
/// <para>
/// The combination of <see cref="UserId"/> and <see cref="VoucherId"/> is unique —
/// enforced by UX_UserVoucher_Pair in the database. A user cannot be assigned
/// the same voucher twice.
/// </para>
/// <para>
/// <see cref="ExpiresAt"/> is an optional per-user override of the voucher's global
/// <c>Voucher.EndDate</c>. When set, the earlier of the two dates applies.
/// </para>
/// </summary>
public sealed class UserVoucher
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int UserVoucherId { get; set; }

    /// <summary>FK to the user this voucher is assigned to.</summary>
    public int UserId { get; set; }

    /// <summary>FK to the voucher being assigned.</summary>
    public int VoucherId { get; set; }

    /// <summary>UTC timestamp when this voucher was assigned to the user.</summary>
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Optional per-user expiry override. When set, this user's access to
    /// the voucher expires at this time regardless of the voucher's global
    /// <c>Voucher.EndDate</c>.
    /// NULL means the voucher's global end date applies.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The user this voucher assignment belongs to.</summary>
    public User User { get; set; } = null!;

    /// <summary>The voucher being assigned to the user.</summary>
    public Voucher Voucher { get; set; } = null!;
}