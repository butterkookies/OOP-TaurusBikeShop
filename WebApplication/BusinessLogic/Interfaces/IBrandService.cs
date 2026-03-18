// WebApplication/BusinessLogic/Interfaces/IBrandService.cs

using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for brand retrieval operations.
/// Brands are reference data managed exclusively via AdminSystem.
/// The WebApplication reads brands for filter dropdowns and product details.
/// </summary>
public interface IBrandService
{
    /// <summary>
    /// Returns all active brands ordered alphabetically.
    /// Used to populate the brand filter sidebar in the product catalog.
    /// </summary>
    Task<IReadOnlyList<Brand>> GetAllActiveBrandsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a single brand by its ID, or <c>null</c> if not found.
    /// </summary>
    Task<Brand?> GetByIdAsync(
        int brandId,
        CancellationToken cancellationToken = default);
}