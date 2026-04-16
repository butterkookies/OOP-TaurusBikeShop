// Services/PhotoService.cs
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

public class PhotoService : IPhotoService
{
    private const long MaxProofFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    private static readonly HashSet<string> AllowedProofMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp",
        "application/pdf"
    };

    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }

    /// <inheritdoc/>
    public async Task<string> UploadAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "taurus-bikeshop/products",
            Transformation = new Transformation()
                                 .Width(800).Height(600)
                                 .Crop("fill").Quality("auto")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            throw new Exception(result.Error.Message);

        return result.SecureUrl.ToString();
    }

    /// <inheritdoc/>
    public async Task<string> UploadPaymentProofAsync(IFormFile file, int orderId)
    {
        // Validate file size
        if (file.Length == 0)
            throw new InvalidOperationException("Uploaded file is empty.");

        if (file.Length > MaxProofFileSizeBytes)
            throw new InvalidOperationException(
                $"File size exceeds the 5 MB maximum. Please upload a smaller file.");

        // Validate MIME type
        string contentType = file.ContentType?.ToLowerInvariant() ?? string.Empty;
        if (!AllowedProofMimeTypes.Contains(contentType))
            throw new InvalidOperationException(
                $"File type '{contentType}' is not allowed. " +
                "Please upload a JPEG, PNG, WebP image, or PDF.");

        await using var stream = file.OpenReadStream();

        // Use RawUploadParams for PDFs; ImageUploadParams for images.
        // No transformation — we need the proof exactly as the customer uploaded it.
        string folder = $"taurus-bikeshop/payment-proofs/order-{orderId}";

        if (contentType == "application/pdf")
        {
            var uploadParams = new RawUploadParams
            {
                File   = new FileDescription(file.FileName, stream),
                Folder = folder,
            };
            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.Error != null)
                throw new InvalidOperationException(result.Error.Message);
            return result.SecureUrl.ToString();
        }
        else
        {
            var uploadParams = new ImageUploadParams
            {
                File    = new FileDescription(file.FileName, stream),
                Folder  = folder,
                // No Transformation — preserve original resolution for admin review
            };
            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.Error != null)
                throw new InvalidOperationException(result.Error.Message);
            return result.SecureUrl.ToString();
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }
}