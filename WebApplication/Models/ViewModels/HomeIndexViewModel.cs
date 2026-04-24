// WebApplication/Models/ViewModels/HomeIndexViewModel.cs

namespace WebApplication.Models.ViewModels;

public sealed class HomeIndexViewModel
{
    public IReadOnlyList<ProductViewModel> FeaturedProducts { get; init; } = [];
    public int ProductCount  { get; init; }
    public int BrandCount    { get; init; }
    public int CategoryCount { get; init; }
}
