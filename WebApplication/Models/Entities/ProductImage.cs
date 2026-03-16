// WebApplication/Models/Entities/ProductImage.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Stores Google Cloud Storage metadata for a product image.
/// Binary image files live in GCS — only the URL and storage path metadata
/// are persisted here.
/// <para>
/// <b>Primary image rule:</b> At most one image per product may have
/// <see cref="IsPrimary"/> = true. This is enforced by the UX_ProductImage_Primary
/// filtered unique index in the database. Application code in
/// <c>FileUploadHelper</c> must clear any existing primary flag before
/// setting a new one.
/// </para>
/// <para>
/// Images are ordered for display using <see cref="DisplayOrder"/> — lower values
/// appear first in the gallery. The primary image should typically have
/// <see cref="DisplayOrder"/> = 0.
/// </para>
/// </summary>
public sealed class ProductImage
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int ProductImageId { get; set; }

    /// <summary>
    /// FK to the product this image belongs to.
    /// Configured with CASCADE DELETE — removing a product removes all its images.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// GCS bucket name where the binary image file is stored.
    /// (e.g. taurus-bikeshop-assets)
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string StorageBucket { get; set; } = string.Empty;

    /// <summary>
    /// GCS object path within the bucket.
    /// (e.g. products/product-123/main.webp)
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string StoragePath { get; set; } = string.Empty;

    /// <summary>
    /// Publicly accessible CDN URL for this image, served via GCS.
    /// This is the URL used in &lt;img&gt; tags in views and ViewModels.
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// The size/resolution variant of this image.
    /// Constrained by CK_ProductImage_Type: Full, Medium, or Thumbnail.
    /// Use <see cref="ImageTypes"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ImageType { get; set; } = string.Empty;

    /// <summary>
    /// <c>true</c> if this is the primary display image for the product.
    /// Only one image per product may be primary — enforced by filtered unique index.
    /// Used as the thumbnail in product listing cards and the main image
    /// on the product detail page.
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Sort order within the product's image gallery. Lower values appear first.
    /// Must be &gt;= 0.
    /// </summary>
    public int DisplayOrder { get; set; } = 0;

    /// <summary>Accessibility alt text for screen readers. Optional but recommended.</summary>
    [MaxLength(200)]
    public string? AltText { get; set; }

    /// <summary>File size in bytes. Optional — used for storage reporting.</summary>
    public int? FileSizeBytes { get; set; }

    /// <summary>
    /// MIME type of the image file (e.g. image/webp, image/jpeg).
    /// Optional — stored for content negotiation and validation purposes.
    /// </summary>
    [MaxLength(50)]
    public string? MimeType { get; set; }

    /// <summary>Image width in pixels. Optional.</summary>
    public int? Width { get; set; }

    /// <summary>Image height in pixels. Optional.</summary>
    public int? Height { get; set; }

    /// <summary>
    /// FK to the admin user who uploaded this image.
    /// NULL if uploaded via an automated process.
    /// </summary>
    public int? UploadedByUserId { get; set; }

    /// <summary>UTC timestamp when this image row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The product this image belongs to.</summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// The admin user who uploaded this image.
    /// NULL for system-uploaded images.
    /// </summary>
    public User? UploadedBy { get; set; }
}

/// <summary>
/// Compile-time constants for all valid product image type values.
/// Mirrors the CK_ProductImage_Type CHECK constraint in the database.
/// </summary>
public static class ImageTypes
{
    /// <summary>Full-resolution image for the product detail page gallery.</summary>
    public const string Full = "Full";

    /// <summary>Medium-resolution image for standard display contexts.</summary>
    public const string Medium = "Medium";

    /// <summary>Small thumbnail image for product listing cards.</summary>
    public const string Thumbnail = "Thumbnail";
}