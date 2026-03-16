// WebApplication/Models/Entities/Supplier.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents an external supplier used for product restocking via purchase orders.
/// Suppliers are managed exclusively through the AdminSystem.
/// The WebApplication exposes supplier data as read-only via
/// <c>SupplierRepository</c> (no controller or views on the web side).
/// </summary>
public sealed class Supplier
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int SupplierId { get; set; }

    /// <summary>Supplier company name.</summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Name of the primary contact person at this supplier. Optional.</summary>
    [MaxLength(100)]
    public string? ContactPerson { get; set; }

    /// <summary>Supplier contact phone number. Optional.</summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Supplier email address. Optional.
    /// Enforced unique on non-NULL values via UX_Supplier_Email filtered index.
    /// </summary>
    [MaxLength(255)]
    public string? Email { get; set; }

    /// <summary>Physical address of the supplier. Stored as a free-text string.</summary>
    [MaxLength(500)]
    public string? Address { get; set; }

    /// <summary>
    /// Whether this supplier is currently active.
    /// Inactive suppliers are hidden from purchase order creation in AdminSystem.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>UTC timestamp when this supplier row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>All purchase orders raised against this supplier.</summary>
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}