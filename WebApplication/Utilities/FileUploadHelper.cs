// WebApplication/Utilities/FileUploadHelper.cs

using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Utilities;

/// <summary>
/// Handles file uploads to Google Cloud Storage.
/// Used by <c>PaymentController</c> for bank transfer proof screenshots
/// and by AdminSystem (via the shared DB) for product image uploads.
/// <para>
/// Binary files are stored in GCS. Only the public URL and storage path
/// metadata are returned for persistence in the database
/// (<c>ProductImage</c>, <c>BankTransferPayment</c>, <c>SupportTicket</c>).
/// </para>
/// <para>
/// <b>Allowed MIME types:</b>
/// <list type="bullet">
///   <item>Product images: image/webp, image/jpeg, image/png</item>
///   <item>Payment proofs: image/webp, image/jpeg, image/png, application/pdf</item>
///   <item>Support attachments: image/webp, image/jpeg, image/png, application/pdf</item>
/// </list>
/// Maximum file size: 5 MB for all upload types.
/// </para>
/// </summary>
public sealed class FileUploadHelper
{
    /// <summary>Maximum allowed file size in bytes (5 MB).</summary>
    private const long MaxFileSizeBytes = 5 * 1024 * 1024;

    /// <summary>MIME types permitted for product images.</summary>
    private static readonly IReadOnlySet<string> ProductImageMimeTypes = new HashSet<string>(
        StringComparer.OrdinalIgnoreCase)
    {
        "image/webp",
        "image/jpeg",
        "image/png"
    };

    /// <summary>MIME types permitted for payment proof screenshots.</summary>
    private static readonly IReadOnlySet<string> PaymentProofMimeTypes = new HashSet<string>(
        StringComparer.OrdinalIgnoreCase)
    {
        "image/webp",
        "image/jpeg",
        "image/png",
        "application/pdf"
    };

    /// <summary>MIME types permitted for support ticket attachments.</summary>
    private static readonly IReadOnlySet<string> SupportAttachmentMimeTypes = new HashSet<string>(
        StringComparer.OrdinalIgnoreCase)
    {
        "image/webp",
        "image/jpeg",
        "image/png",
        "application/pdf"
    };

    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    /// <summary>
    /// Initialises the helper with a GCS storage client and target bucket.
    /// Credentials are resolved via Application Default Credentials (ADC)
    /// when running on Google Cloud infrastructure.
    /// For local development, set the <c>GOOGLE_APPLICATION_CREDENTIALS</c>
    /// environment variable to a service account key file path.
    /// </summary>
    /// <param name="storageClient">An authenticated GCS storage client.</param>
    /// <param name="bucketName">
    /// The GCS bucket name to upload files into.
    /// Sourced from <c>GoogleCloudStorage:BucketName</c> in appsettings.
    /// </param>
    public FileUploadHelper(StorageClient storageClient, string bucketName)
    {
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ArgumentException("Bucket name must not be null or whitespace.", nameof(bucketName));

        _storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        _bucketName = bucketName;
    }

    /// <summary>
    /// Uploads a product image to GCS and returns the storage metadata.
    /// </summary>
    /// <param name="file">The image file from an HTTP form upload.</param>
    /// <param name="folder">
    /// The GCS folder prefix (e.g. "products/product-123").
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// An <see cref="UploadResult"/> containing the public URL, bucket name,
    /// and GCS object path for database persistence.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the file fails MIME type or size validation.
    /// </exception>
    public Task<UploadResult> UploadProductImageAsync(
        IFormFile file,
        string folder,
        CancellationToken cancellationToken = default)
    {
        return UploadAsync(file, folder, ProductImageMimeTypes, cancellationToken);
    }

    /// <summary>
    /// Uploads a bank transfer payment proof screenshot or PDF to GCS.
    /// </summary>
    /// <param name="file">The proof file from an HTTP form upload.</param>
    /// <param name="folder">
    /// The GCS folder prefix (e.g. "payment-proofs/payment-456").
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// An <see cref="UploadResult"/> containing the public URL, bucket name,
    /// and GCS object path for persistence in <c>BankTransferPayment</c>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the file fails MIME type or size validation.
    /// </exception>
    public Task<UploadResult> UploadPaymentProofAsync(
        IFormFile file,
        string folder,
        CancellationToken cancellationToken = default)
    {
        return UploadAsync(file, folder, PaymentProofMimeTypes, cancellationToken);
    }

    /// <summary>
    /// Uploads a support ticket attachment to GCS.
    /// </summary>
    /// <param name="file">The attachment file from an HTTP form upload.</param>
    /// <param name="folder">
    /// The GCS folder prefix (e.g. "support-attachments/ticket-789").
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// An <see cref="UploadResult"/> containing the public URL, bucket name,
    /// and GCS object path for persistence in <c>SupportTicket</c>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the file fails MIME type or size validation.
    /// </exception>
    public Task<UploadResult> UploadSupportAttachmentAsync(
        IFormFile file,
        string folder,
        CancellationToken cancellationToken = default)
    {
        return UploadAsync(file, folder, SupportAttachmentMimeTypes, cancellationToken);
    }

    /// <summary>
    /// Core upload implementation shared by all public upload methods.
    /// Validates MIME type and file size, generates a unique GCS object path,
    /// uploads the file, and returns the storage metadata.
    /// </summary>
    private async Task<UploadResult> UploadAsync(
        IFormFile file,
        string folder,
        IReadOnlySet<string> allowedMimeTypes,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(file);

        if (string.IsNullOrWhiteSpace(folder))
            throw new ArgumentException("Folder must not be null or whitespace.", nameof(folder));

        // Validate file size
        if (file.Length == 0)
            throw new InvalidOperationException("Uploaded file is empty.");

        if (file.Length > MaxFileSizeBytes)
            throw new InvalidOperationException(
                $"File size {file.Length:N0} bytes exceeds the maximum allowed size of {MaxFileSizeBytes:N0} bytes (5 MB).");

        // Validate MIME type
        string contentType = file.ContentType?.ToLowerInvariant() ?? string.Empty;
        if (!allowedMimeTypes.Contains(contentType))
            throw new InvalidOperationException(
                $"File type '{contentType}' is not permitted. Allowed types: {string.Join(", ", allowedMimeTypes)}.");

        // Generate a unique, collision-resistant object path
        string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string objectName = $"{folder.TrimEnd('/')}/{Guid.NewGuid():N}{fileExtension}";

        // Upload to GCS
        await using Stream fileStream = file.OpenReadStream();

        Google.Apis.Storage.v1.Data.Object uploadedObject = await _storageClient.UploadObjectAsync(
            bucket: _bucketName,
            objectName: objectName,
            contentType: contentType,
            source: fileStream,
            cancellationToken: cancellationToken);

        // Build the public CDN URL
        // Format: https://storage.googleapis.com/{bucket}/{objectName}
        string publicUrl = $"https://storage.googleapis.com/{_bucketName}/{objectName}";

        return new UploadResult(
            ImageUrl: publicUrl,
            StorageBucket: _bucketName,
            StoragePath: objectName);
    }
}

/// <summary>
/// Immutable result returned by all <see cref="FileUploadHelper"/> upload methods.
/// All three values must be persisted to the database together — they are
/// needed to construct the public URL and to manage or delete the file later.
/// </summary>
/// <param name="ImageUrl">
/// The publicly accessible CDN URL for the uploaded file.
/// Use this in &lt;img&gt; tags and API responses.
/// </param>
/// <param name="StorageBucket">
/// The GCS bucket name where the file is stored.
/// </param>
/// <param name="StoragePath">
/// The GCS object path (key) within the bucket.
/// Used to reference or delete the file via the GCS API.
/// </param>
public sealed record UploadResult(
    string ImageUrl,
    string StorageBucket,
    string StoragePath);