// WebApplication/Models/Entities/Role.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a system role assigned to users via the <see cref="UserRole"/> junction table.
/// Valid role names are enforced by the CK_Role_Name CHECK constraint on the database.
/// </summary>
public sealed class Role
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Unique role name. Constrained to: Admin, Manager, Cashier, Staff, Customer.
    /// Use <see cref="RoleNames"/> constants instead of magic strings when comparing.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;

    /// <summary>Human-readable description of this role's responsibilities.</summary>
    [MaxLength(255)]
    public string? Description { get; set; }

    /// <summary>UTC timestamp when this role row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>All user-role assignment rows that reference this role.</summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

/// <summary>
/// Compile-time constants for all valid role name values.
/// Use these instead of magic strings throughout services and authorization checks.
/// Mirrors the CK_Role_Name CHECK constraint values in the database.
/// </summary>
public static class RoleNames
{
    /// <summary>Full system administrator access.</summary>
    public const string Admin = "Admin";

    /// <summary>Store manager — elevated admin access.</summary>
    public const string Manager = "Manager";

    /// <summary>POS cashier — walk-in transaction access only.</summary>
    public const string Cashier = "Cashier";

    /// <summary>General staff — limited admin access.</summary>
    public const string Staff = "Staff";

    /// <summary>Registered customer — web application access only.</summary>
    public const string Customer = "Customer";
}