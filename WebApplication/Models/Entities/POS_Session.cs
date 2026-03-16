// WebApplication/Models/Entities/POS_Session.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a single cashier shift on a POS terminal in the physical store.
/// A session begins when a cashier opens the terminal and ends when they close
/// their shift. Walk-in orders created during the session contribute to
/// <see cref="TotalSales"/>.
/// <para>
/// POS sessions are created and managed exclusively through the AdminSystem.
/// The WebApplication entity exists to support EF Core relationship mapping
/// and read-only reporting queries only.
/// </para>
/// </summary>
public sealed class POS_Session
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int POSSessionId { get; set; }

    /// <summary>
    /// FK to the cashier or staff member who opened this session.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Identifier of the physical terminal used for this session.
    /// (e.g. POS-TERMINAL-01)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string TerminalName { get; set; } = string.Empty;

    /// <summary>UTC timestamp when this shift started.</summary>
    public DateTime ShiftStart { get; set; }

    /// <summary>
    /// UTC timestamp when this shift ended.
    /// NULL while the session is still active — a NULL ShiftEnd means
    /// the terminal is currently open.
    /// </summary>
    public DateTime? ShiftEnd { get; set; }

    /// <summary>
    /// Running total of all cash and GCash sales processed during this shift.
    /// Updated by <c>OrderService.CreateWalkInOrderAsync</c> in AdminSystem
    /// each time a walk-in transaction is completed.
    /// </summary>
    public decimal TotalSales { get; set; } = 0m;

    /// <summary>UTC timestamp when this session row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The cashier or staff member who opened this POS session.</summary>
    public User User { get; set; } = null!;
}