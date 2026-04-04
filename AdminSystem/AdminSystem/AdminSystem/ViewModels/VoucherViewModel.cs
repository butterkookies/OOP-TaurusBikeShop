using System.Collections.ObjectModel;
using System.Data.SqlClient;
using AdminSystem.Helpers;
using AdminSystem.Models;
using Dapper;

namespace AdminSystem.ViewModels
{
    public class VoucherViewModel : BaseViewModel
    {
        public VoucherViewModel()
        {
            Vouchers      = new ObservableCollection<Voucher>();
            LoadCommand   = new RelayCommand(Load);
            SaveCommand   = new RelayCommand(Save,
                _ => !string.IsNullOrWhiteSpace(EditingVoucher?.Code)
                     && EditingVoucher.DiscountValue > 0);
            NewCommand    = new RelayCommand(NewVoucher);
            ToggleCommand = new RelayCommand(ToggleActive,
                _ => SelectedVoucher != null);
        }

        public ObservableCollection<Voucher> Vouchers { get; }

        private Voucher _selectedVoucher;
        public Voucher SelectedVoucher
        {
            get { return _selectedVoucher; }
            set
            {
                SetField(ref _selectedVoucher, value, "SelectedVoucher");
                if (value != null) EditingVoucher = CloneVoucher(value);
            }
        }

        private Voucher _editingVoucher;
        public Voucher EditingVoucher
        {
            get { return _editingVoucher; }
            set { SetField(ref _editingVoucher, value, "EditingVoucher"); }
        }

        public RelayCommand LoadCommand   { get; }
        public RelayCommand SaveCommand   { get; }
        public RelayCommand NewCommand    { get; }
        public RelayCommand ToggleCommand { get; }

        public void Load()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                using (SqlConnection conn =
                    DatabaseHelper.GetConnection())
                {
                    Vouchers.Clear();
                    foreach (Voucher v in conn.Query<Voucher>(
                        "SELECT * FROM Vouchers ORDER BY CreatedAt DESC"))
                        Vouchers.Add(v);
                }
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
            finally { IsLoading = false; }
        }

        private void NewVoucher(object param)
        {
            EditingVoucher = new Voucher
            {
                IsActive     = true,
                DiscountType = DiscountTypes.Percentage
            };
            SelectedVoucher = null;
        }

        private void Save(object param)
        {
            if (EditingVoucher == null) return;
            ClearMessages();
            try
            {
                using (SqlConnection conn =
                    DatabaseHelper.GetConnection())
                {
                    if (EditingVoucher.VoucherId == 0)
                    {
                        conn.Execute(
                            @"INSERT INTO Vouchers
                                (Code, DiscountType, DiscountValue,
                                 MinimumOrderAmount, MaxUsageCount,
                                 MaxUsagePerUser, ExpiresAt,
                                 IsActive, CreatedAt)
                              VALUES
                                (@Code, @DiscountType, @DiscountValue,
                                 @MinimumOrderAmount, @MaxUsageCount,
                                 @MaxUsagePerUser, @ExpiresAt,
                                 @IsActive, GETUTCDATE())",
                            EditingVoucher);
                        ShowSuccess("Voucher created.");
                    }
                    else
                    {
                        conn.Execute(
                            @"UPDATE Vouchers SET
                                DiscountType       = @DiscountType,
                                DiscountValue      = @DiscountValue,
                                MinimumOrderAmount = @MinimumOrderAmount,
                                MaxUsageCount      = @MaxUsageCount,
                                MaxUsagePerUser    = @MaxUsagePerUser,
                                ExpiresAt          = @ExpiresAt,
                                IsActive           = @IsActive
                              WHERE VoucherId = @VoucherId",
                            EditingVoucher);
                        ShowSuccess("Voucher updated.");
                    }
                }
                Load();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private void ToggleActive(object param)
        {
            if (SelectedVoucher == null) return;
            ClearMessages();
            try
            {
                using (SqlConnection conn =
                    DatabaseHelper.GetConnection())
                {
                    conn.Execute(
                        "UPDATE Vouchers SET IsActive = @Val WHERE VoucherId = @Id",
                        new { Val = !SelectedVoucher.IsActive,
                              Id  = SelectedVoucher.VoucherId });
                }
                ShowSuccess("Voucher " +
                    (SelectedVoucher.IsActive ? "deactivated." : "activated."));
                Load();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private static Voucher CloneVoucher(Voucher src) => new Voucher
        {
            VoucherId          = src.VoucherId,
            Code               = src.Code,
            DiscountType       = src.DiscountType,
            DiscountValue      = src.DiscountValue,
            MinimumOrderAmount = src.MinimumOrderAmount,
            MaxUsageCount      = src.MaxUsageCount,
            MaxUsagePerUser    = src.MaxUsagePerUser,
            ExpiresAt          = src.ExpiresAt,
            IsActive           = src.IsActive
        };
    }
}
