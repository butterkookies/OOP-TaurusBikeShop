// WebApplication/Models/Entities/ActiveSession.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents an active login session tracked in the database.
/// Used for tracking active sessions, handling forceful logouts,
/// and maintaining parity with the physical database schema.
/// </summary>
public sealed class ActiveSession
{
    public int SessionId { get; set; }

    public int UserId { get; set; }

    [Required]
    [MaxLength(500)]
    public string RefreshToken { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? DeviceInfo { get; set; }

    [MaxLength(50)]
    public string? IpAddress { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAt { get; set; }

    // Navigation Property
    public User? User { get; set; }
}
