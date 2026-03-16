// WebApplication/Models/Entities/OTPVerification.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Stores one-time password records used during the two-step customer registration flow.
/// Intentionally has no FK to <c>User</c> — this row is created before the user account
/// exists. Once the OTP is verified, <see cref="IsUsed"/> is flipped to <c>true</c> and
/// the user account is created.
/// <para>
/// Expired or used rows can be purged by a maintenance job without affecting any user data.
/// </para>
/// </summary>
public sealed class OTPVerification
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int OTPId { get; set; }

    /// <summary>
    /// Email address the OTP was sent to.
    /// No FK to User — the user account does not exist yet at this point.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The 6-digit one-time code sent to the customer via email or SMS.
    /// Never log or expose this value after dispatch.
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string OTPCode { get; set; } = string.Empty;

    /// <summary>
    /// <c>false</c> while the code is still valid and unused.
    /// Flipped to <c>true</c> immediately after successful verification to prevent reuse.
    /// </summary>
    public bool IsUsed { get; set; } = false;

    /// <summary>
    /// UTC timestamp after which this OTP is no longer valid.
    /// Typically set to <c>CreatedAt + 10 minutes</c> by <c>UserService</c>.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>UTC timestamp when this OTP row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}