using System.Collections.ObjectModel;
using AdminSystem.Models;
using AdminSystem.Services;

namespace AdminSystem.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;
            Products    = new ObservableCollection<Product>();
            Categories  = new ObservableCollection<Category>();
            Brands      = new ObservableCollection<Brand>();

            LoadCommand          = new RelayCommand(Load);
            SaveProductCommand   = new RelayCommand(SaveProduct,
                _ => !string.IsNullOrWhiteSpace(EditingProduct?.Name));
            DeleteProductCommand = new RelayCommand(DeleteProduct,
                _ => SelectedProduct != null);
            NewProductCommand    = new RelayCommand(NewProduct);
        }

        // ── Collections ─────────────────────────────────────────────────
        public ObservableCollection<Product>  Products   { get; }
        public ObservableCollection<Category> Categories { get; }
        public ObservableCollection<Brand>    Brands     { get; }

        // ── Selected / editing ──────────────────────────────────────────
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                SetField(ref _selectedProduct, value, "SelectedProduct");
                if (value != null) EditingProduct = CloneProduct(value);
            }
        }

        private Product _editingProduct;
        public Product EditingProduct
        {
            get { return _editingProduct; }
            set { SetField(ref _editingProduct, value, "EditingProduct"); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetField(ref _searchText, value, "SearchText");
                Search();
            }
        }

        // ── Commands ────────────────────────────────────────────────────
        public RelayCommand LoadCommand          { get; }
        public RelayCommand SaveProductCommand   { get; }
        public RelayCommand DeleteProductCommand { get; }
        public RelayCommand NewProductCommand    { get; }

        // ── Methods ─────────────────────────────────────────────────────
        public void Load()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                Products.Clear();
                foreach (Product p in _productService.GetAllProducts())
                    Products.Add(p);

                Categories.Clear();
                foreach (Category c in _productService.GetAllCategories())
                    Categories.Add(c);

                Brands.Clear();
                foreach (Brand b in _productService.GetAllBrands())
                    Brands.Add(b);
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
            finally { IsLoading = false; }
        }

        private void Search()
        {
            try
            {
                Products.Clear();
                foreach (Product p in _productService.SearchProducts(SearchText))
                    Products.Add(p);
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private void NewProduct(object param)
        {
            EditingProduct  = new Product { IsActive = true };
            SelectedProduct = null;
        }

        private void SaveProduct(object param)
        {
            if (EditingProduct == null) return;
            ClearMessages();
            try
            {
                if (EditingProduct.ProductId == 0)
                    _productService.CreateProduct(EditingProduct);
                else
                    _productService.UpdateProduct(EditingProduct);

                ShowSuccess("Product saved.");
                Load();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private void DeleteProduct(object param)
        {
            if (SelectedProduct == null) return;
            ClearMessages();
            try
            {
                _productService.DeactivateProduct(SelectedProduct.ProductId);
                ShowSuccess("Product deactivated.");
                Load();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private static Product CloneProduct(Product src)
        {
            return new Product
            {
                ProductId   = src.ProductId,
                CategoryId  = src.CategoryId,
                BrandId     = src.BrandId,
                Name        = src.Name,
                Description = src.Description,
                Price       = src.Price,
                IsActive    = src.IsActive,
                IsFeatured  = src.IsFeatured
            };
        }
    }
}
