// WebApplication/Models/Entities/UserRole.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Junction table that assigns roles to users (N:M between User and Role).
/// A user may have multiple roles (e.g. Admin + Cashier).
/// The combination of <see cref="UserId"/> and <see cref="RoleId"/> is unique —
/// enforced by UX_UserRole_Pair in the database — preventing duplicate assignments.
/// <para>
/// CASCADE DELETE is configured on the User side — removing a user removes all
/// their role assignments. The Role side has no cascade to prevent accidental
/// role-wide deletions.
/// </para>
/// </summary>
public sealed class UserRole
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int UserRoleId { get; set; }

    /// <summary>
    /// FK to the user being assigned the role.
    /// Configured with CASCADE DELETE in AppDbContext.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>FK to the role being assigned to the user.</summary>
    public int RoleId { get; set; }

    /// <summary>UTC timestamp when this role was assigned to the user.</summary>
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The user this role assignment belongs to.</summary>
    public User User { get; set; } = null!;

    /// <summary>The role being assigned.</summary>
    public Role Role { get; set; } = null!;
}