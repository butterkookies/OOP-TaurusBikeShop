// Services/PhotoService.cs
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class PhotoService : IPhotoService
{
    private const long MaxProofFileSizeBytes = 15 * 1024 * 1024; // 15 MB

    private static readonly HashSet<string> AllowedProofMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png"
    };

    private readonly Cloudinary? _cloudinary;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<PhotoService> _logger;

    public PhotoService(
        IOptions<CloudinarySettings> config,
        IWebHostEnvironment env,
        ILogger<PhotoService> logger)
    {
        _env    = env;
        _logger = logger;

        // Only initialise Cloudinary when real credentials are present.
        string? cloud  = config.Value.CloudName;
        string? apiKey = config.Value.ApiKey;
        string? secret = config.Value.ApiSecret;

        bool hasCredentials =
            !string.IsNullOrWhiteSpace(cloud)  &&
            !string.IsNullOrWhiteSpace(apiKey)  && apiKey  != "SET_VIA_USER_SECRETS_OR_ENVIRONMENT_VARIABLE" &&
            !string.IsNullOrWhiteSpace(secret) && secret != "SET_VIA_USER_SECRETS_OR_ENVIRONMENT_VARIABLE";

        if (hasCredentials)
        {
            try
            {
                _cloudinary = new Cloudinary(new Account(cloud, apiKey, secret));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to initialise Cloudinary — local storage will be used.");
            }
        }
        else
        {
            _logger.LogWarning("Cloudinary credentials not configured — payment proofs will be saved locally.");
        }
    }

    /// <inheritdoc/>
    public async Task<string> UploadAsync(IFormFile file)
    {
        // Try Cloudinary first
        if (_cloudinary != null)
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

        // Fallback: save locally
        return await SaveLocallyAsync(file, "products");
    }

    /// <inheritdoc/>
    public async Task<string> UploadPaymentProofAsync(IFormFile file, int orderId)
    {
        // Validate file size
        if (file.Length == 0)
            throw new InvalidOperationException("Uploaded file is empty.");

        if (file.Length > MaxProofFileSizeBytes)
            throw new InvalidOperationException(
                "File size exceeds the 15 MB maximum. Please upload a smaller file.");

        // Validate MIME type
        string contentType = file.ContentType?.ToLowerInvariant() ?? string.Empty;
        if (!AllowedProofMimeTypes.Contains(contentType))
            throw new InvalidOperationException(
                $"File type '{contentType}' is not allowed. " +
                "Please upload a JPG or PNG image.");

        string subfolder = $"payment-proofs/order-{orderId}";

        // Try Cloudinary first
        if (_cloudinary != null)
        {
            try
            {
                await using var stream = file.OpenReadStream();
                string folder = $"taurus-bikeshop/{subfolder}";

                var uploadParams = new ImageUploadParams
                {
                    File   = new FileDescription(file.FileName, stream),
                    Folder = folder,
                };
                var result = await _cloudinary.UploadAsync(uploadParams);
                if (result.Error != null)
                    throw new InvalidOperationException(result.Error.Message);
                return result.SecureUrl.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cloudinary upload failed for order {OrderId} — falling back to local.", orderId);
            }
        }

        // Fallback: save locally under wwwroot/uploads/
        return await SaveLocallyAsync(file, subfolder);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string publicId)
    {
        if (_cloudinary == null) return;
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }

    // =========================================================================
    // Local file fallback
    // =========================================================================

    private async Task<string> SaveLocallyAsync(IFormFile file, string subfolder)
    {
        // Save to wwwroot/uploads/{subfolder}/
        string uploadsDir = Path.Combine(_env.WebRootPath, "uploads", subfolder);
        Directory.CreateDirectory(uploadsDir);

        // Unique filename to avoid collisions
        string ext      = Path.GetExtension(file.FileName);
        string safeName = $"{Guid.NewGuid():N}{ext}";
        string filePath = Path.Combine(uploadsDir, safeName);

        await using var fs = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fs);

        _logger.LogInformation("Saved file locally: {FilePath}", filePath);

        // Return a URL-path relative to wwwroot so it can be served as a static file.
        return $"/uploads/{subfolder}/{safeName}";
    }
}