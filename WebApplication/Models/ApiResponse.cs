// WebApplication/Models/ApiResponse.cs

namespace WebApplication.Models;

/// <summary>
/// Standard JSON envelope returned by all AJAX controller actions.
/// Every action that returns <c>JsonResult</c> must use this shape —
/// never return anonymous objects or raw data directly.
/// <para>
/// Use the static factory methods <see cref="Ok"/> and <see cref="Fail"/>
/// for clean, consistent call sites:
/// <code>
/// return Json(ApiResponse.Ok(new { cartCount = 3 }));
/// return Json(ApiResponse.Fail("Voucher code is expired."));
/// </code>
/// </para>
/// </summary>
public sealed class ApiResponse
{
    /// <summary>
    /// <c>true</c> when the operation completed successfully;
    /// <c>false</c> when it failed for any reason.
    /// Client-side JavaScript checks this flag before reading <see cref="Data"/>.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Human-readable message accompanying the response.
    /// Always set on failure — describes what went wrong.
    /// Optional on success — used for toast notifications (e.g. "Item added to cart").
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Payload returned on success. Serialised to JSON by the MVC framework.
    /// <c>null</c> for operations that have no meaningful return value
    /// (e.g. remove from wishlist).
    /// </summary>
    public object? Data { get; init; }

    /// <summary>
    /// Creates a successful <see cref="ApiResponse"/> with optional data and message.
    /// </summary>
    /// <param name="data">
    /// The response payload. Pass an anonymous object or a strongly-typed DTO.
    /// </param>
    /// <param name="message">Optional success message for toast display.</param>
    /// <returns>A successful <see cref="ApiResponse"/>.</returns>
    public static ApiResponse Ok(object? data = null, string? message = null) =>
        new() { Success = true, Data = data, Message = message };

    /// <summary>
    /// Creates a failed <see cref="ApiResponse"/> with an error message.
    /// </summary>
    /// <param name="message">
    /// A user-facing description of what went wrong.
    /// Must not expose internal exception details or stack traces.
    /// </param>
    /// <returns>A failed <see cref="ApiResponse"/>.</returns>
    public static ApiResponse Fail(string message) =>
        new() { Success = false, Message = message };
}