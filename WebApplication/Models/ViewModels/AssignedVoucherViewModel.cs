// WebApplication/Models/ViewModels/AssignedVoucherViewModel.cs

namespace WebApplication.Models.ViewModels;

/// <summary>
/// DTO representing an active voucher assigned to a user,
/// suitable for displaying in the checkout combobox.
/// </summary>
public sealed class AssignedVoucherViewModel
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TimeLeft { get; set; } = string.Empty;
}
