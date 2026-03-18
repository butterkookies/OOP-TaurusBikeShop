// WebApplication/BusinessLogic/Services/BrandService.cs

using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="IBrandService"/> — simple read-only pass-through
/// to <see cref="BrandRepository"/>.
/// </summary>
public sealed class BrandService : IBrandService
{
    private readonly BrandRepository _brandRepo;

    /// <inheritdoc/>
    public BrandService(BrandRepository brandRepo)
    {
        _brandRepo = brandRepo ?? throw new ArgumentNullException(nameof(brandRepo));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Brand>> GetAllActiveBrandsAsync(
        CancellationToken cancellationToken = default)
        => await _brandRepo.GetAllActiveBrandsAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<Brand?> GetByIdAsync(
        int brandId,
        CancellationToken cancellationToken = default)
        => await _brandRepo.GetByIdAsync(brandId, cancellationToken);
}