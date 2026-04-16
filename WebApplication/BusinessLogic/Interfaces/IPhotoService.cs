public interface IPhotoService
{
    /// <summary>
    /// Uploads a product image to Cloudinary (resized to 800×600, quality auto).
    /// Returns the secure CDN URL.
    /// </summary>
    Task<string> UploadAsync(IFormFile file);

    /// <summary>
    /// Uploads a payment proof image or PDF to Cloudinary under
    /// <c>taurus-bikeshop/payment-proofs/order-{orderId}</c>.
    /// Validates file type (JPEG, PNG, WebP, PDF) and size (≤ 5 MB).
    /// Returns the secure CDN URL to persist in the database.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the file type or size is invalid.
    /// </exception>
    Task<string> UploadPaymentProofAsync(IFormFile file, int orderId);

    Task DeleteAsync(string publicId);
}