using System.Windows;
using System.Windows.Controls;
using AdminSystem.Models;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    public partial class ProductsView : UserControl
    {
        private readonly ProductViewModel _vm;

        public ProductsView(ProductViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            BtnNewProduct.Click        += (s, e) => NewProduct();
            BtnSaveProduct.Click       += (s, e) => Save();
            BtnDeactivateProduct.Click += (s, e) => Deactivate();

            TbProductSearch.TextChanged += (s, e) =>
            {
                _vm.SearchText = TbProductSearch.Text;
                // ObservableCollection auto-notifies; reassign to force immediate refresh
                DgProducts.ItemsSource = _vm.Products;
            };

            WireSelectionChanged();
        }

        // ── Wire up DataGrid selection → form fields ──────────────────────
        private void WireSelectionChanged()
        {
            DgProducts.SelectionChanged += (s, e) =>
            {
                Product p = DgProducts.SelectedItem as Product;
                _vm.SelectedProduct = p;
                if (p == null) return;

                TbProductName.Text        = p.Name;
                TbProductPrice.Text       = p.Price.ToString("N2");
                TbProductDesc.Text        = p.Description;
                ChkActive.IsChecked       = p.IsActive;
                ChkFeatured.IsChecked     = p.IsFeatured;
                CbCategory.SelectedValue  = p.CategoryId;
                CbBrand.SelectedValue     = p.BrandId;
                TbProductFormError.Visibility = Visibility.Collapsed;
            };
        }

        // ── Refresh (called by MainWindow.Navigate) ───────────────────────
        public void Refresh()
        {
            _vm.Load();
            DgProducts.ItemsSource = _vm.Products;
            CbCategory.ItemsSource = _vm.Categories;
            CbBrand.ItemsSource    = _vm.Brands;
        }

        // ── New product: clear form ───────────────────────────────────────
        private void NewProduct()
        {
            _vm.NewProductCommand.Execute(null);
            DgProducts.SelectedItem           = null;
            TbProductName.Text                = string.Empty;
            TbProductPrice.Text               = string.Empty;
            TbProductDesc.Text                = string.Empty;
            ChkActive.IsChecked               = true;
            ChkFeatured.IsChecked             = false;
            CbCategory.SelectedIndex          = -1;
            CbBrand.SelectedIndex             = -1;
            TbProductFormError.Visibility     = Visibility.Collapsed;
        }

        // ── Save (insert or update) ───────────────────────────────────────
        private void Save()
        {
            if (_vm.EditingProduct == null)
                _vm.EditingProduct = new Product();

            decimal price;
            if (!decimal.TryParse(TbProductPrice.Text.Trim(), out price))
            {
                TbProductFormError.Text       = "Enter a valid price.";
                TbProductFormError.Visibility = Visibility.Visible;
                return;
            }

            _vm.EditingProduct.Name        = TbProductName.Text.Trim();
            _vm.EditingProduct.Price       = price;
            _vm.EditingProduct.Description = TbProductDesc.Text.Trim();
            _vm.EditingProduct.IsActive    = ChkActive.IsChecked == true;
            _vm.EditingProduct.IsFeatured  = ChkFeatured.IsChecked == true;
            _vm.EditingProduct.CategoryId  = CbCategory.SelectedValue != null
                ? (int?)System.Convert.ToInt32(CbCategory.SelectedValue) : null;
            _vm.EditingProduct.BrandId     = CbBrand.SelectedValue != null
                ? (int?)System.Convert.ToInt32(CbBrand.SelectedValue) : null;

            _vm.SaveProductCommand.Execute(null);

            if (_vm.HasError)
            {
                TbProductFormError.Text       = _vm.ErrorMessage;
                TbProductFormError.Visibility = Visibility.Visible;
            }
            else
            {
                TbProductFormError.Visibility = Visibility.Collapsed;
                DgProducts.ItemsSource        = _vm.Products;
            }
        }

        // ── Deactivate ────────────────────────────────────────────────────
        private void Deactivate()
        {
            if (_vm.SelectedProduct == null) return;
            if (MessageBox.Show(
                    "Deactivate \"" + _vm.SelectedProduct.Name + "\"?",
                    "Confirm Deactivation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _vm.DeleteProductCommand.Execute(null);
                DgProducts.ItemsSource = _vm.Products;
            }
        }
    }
}
