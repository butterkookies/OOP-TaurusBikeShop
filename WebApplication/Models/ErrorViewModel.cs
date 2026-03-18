// WebApplication/Models/ErrorViewModel.cs

namespace WebApplication.Models;

/// <summary>
/// Model for the unhandled exception fallback view (<c>Error.cshtml</c>).
/// Populated by <c>HomeController.Error</c> from the HTTP context.
/// </summary>
public sealed class ErrorViewModel
{
    /// <summary>
    /// The ASP.NET Core request trace identifier for this error.
    /// Used to correlate the user-facing error with server-side logs.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// <c>true</c> when <see cref="RequestId"/> is non-empty and should
    /// be displayed to the user for support reference.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}