// WebApplication/DataAccess/Repositories/BrandRepository.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="Brand"/> entities.
/// Brands are reference data managed exclusively via AdminSystem.
/// The WebApplication reads brands for product filter dropdowns and detail pages.
/// </summary>
public sealed class BrandRepository : Repository<Brand>
{
    /// <inheritdoc/>
    public BrandRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns all active brands ordered alphabetically by name.
    /// Used to populate the brand filter sidebar in the product catalog.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// All brands where <c>IsActive = true</c>, ordered by <c>BrandName</c> ascending.
    /// </returns>
    public async Task<IReadOnlyList<Brand>> GetAllActiveBrandsAsync(
        CancellationToken cancellationToken = default)
    {
        return await Context.Brands
            .AsNoTracking()
            .Where(b => b.IsActive)
            .OrderBy(b => b.BrandName)
            .ToListAsync(cancellationToken);
    }
}