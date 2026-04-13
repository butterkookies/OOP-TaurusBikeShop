using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AdminSystem_v2.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;

        // ── List / search ─────────────────────────────────────────────────
        private ObservableCollection<Product> _products = new();
        public  ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private string _searchText = string.Empty;
        public  string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    _ = SearchAsync();
            }
        }

        // ── Selected product (left-side list click) ───────────────────────
        private Product? _selectedProduct;
        public  Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (SetProperty(ref _selectedProduct, value))
                    OnProductSelected(value);
            }
        }

        // ── Edit form (clone of selected, or blank for Add) ───────────────
        private Product? _editProduct;
        public  Product? EditProduct
        {
            get => _editProduct;
            set => SetProperty(ref _editProduct, value);
        }

        private bool _isEditing;
        public  bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        // ── Drop-down sources ─────────────────────────────────────────────
        private ObservableCollection<Category> _categories = new();
        public  ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        private ObservableCollection<Brand> _brands = new();
        public  ObservableCollection<Brand> Brands
        {
            get => _brands;
            set => SetProperty(ref _brands, value);
        }

        // ── Stock-adjustment overlay ──────────────────────────────────────
        private bool _isAdjustingStock;
        public  bool IsAdjustingStock
        {
            get => _isAdjustingStock;
            set => SetProperty(ref _isAdjustingStock, value);
        }

        private ProductVariant? _adjustTarget;
        public  ProductVariant? AdjustTarget
        {
            get => _adjustTarget;
            set => SetProperty(ref _adjustTarget, value);
        }

        private int _adjustQty;
        public  int AdjustQty
        {
            get => _adjustQty;
            set => SetProperty(ref _adjustQty, value);
        }

        private string _adjustType = InventoryChangeTypes.Adjustment;
        public  string AdjustType
        {
            get => _adjustType;
            set => SetProperty(ref _adjustType, value);
        }

        private string _adjustNotes = string.Empty;
        public  string AdjustNotes
        {
            get => _adjustNotes;
            set => SetProperty(ref _adjustNotes, value);
        }

        public List<string> AdjustTypes { get; } = new()
        {
            InventoryChangeTypes.Adjustment,
            InventoryChangeTypes.Purchase,
            InventoryChangeTypes.Return,
            InventoryChangeTypes.Damage,
            InventoryChangeTypes.Loss,
        };

        // ── Product images ────────────────────────────────────────────────

        private ObservableCollection<ProductImage> _editImages = new();
        public  ObservableCollection<ProductImage> EditImages
        {
            get => _editImages;
            private set => SetProperty(ref _editImages, value);
        }

        private string _newImageUrl = string.Empty;
        public  string NewImageUrl
        {
            get => _newImageUrl;
            set
            {
                if (SetProperty(ref _newImageUrl, value))
                    OnPropertyChanged(nameof(IsNewImageUrlEmpty));
            }
        }

        /// <summary>True when the URL input is blank — used to show/hide the placeholder hint.</summary>
        public bool IsNewImageUrlEmpty => string.IsNullOrEmpty(_newImageUrl);

        // ── Add-variant overlay ───────────────────────────────────────────
        private bool _isAddingVariant;
        public  bool IsAddingVariant
        {
            get => _isAddingVariant;
            set => SetProperty(ref _isAddingVariant, value);
        }

        private ProductVariant _newVariant = new();
        public  ProductVariant NewVariant
        {
            get => _newVariant;
            set => SetProperty(ref _newVariant, value);
        }

        // ── Commands ──────────────────────────────────────────────────────
        public ICommand RefreshCommand        { get; }
        public ICommand AddNewCommand         { get; }
        public ICommand SaveCommand           { get; }
        public ICommand CancelEditCommand     { get; }
        public ICommand DeactivateCommand     { get; }
        public ICommand OpenAdjustCommand     { get; }
        public ICommand ConfirmAdjustCommand  { get; }
        public ICommand CancelAdjustCommand   { get; }
        public ICommand OpenAddVariantCommand { get; }
        public ICommand SaveVariantCommand    { get; }
        public ICommand CancelVariantCommand  { get; }
        public ICommand AddImageCommand       { get; }
        public ICommand RemoveImageCommand    { get; }

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;

            RefreshCommand        = new RelayCommand(async () => await LoadAsync());
            AddNewCommand         = new RelayCommand(BeginAdd);
            SaveCommand           = new RelayCommand(async () => await SaveAsync(), () => IsEditing);
            CancelEditCommand     = new RelayCommand(CancelEdit,   () => IsEditing);
            DeactivateCommand     = new RelayCommand<Product>(async p => await DeactivateAsync(p));
            OpenAdjustCommand     = new RelayCommand<ProductVariant>(OpenAdjust);
            ConfirmAdjustCommand  = new RelayCommand(async () => await ConfirmAdjustAsync(), () => IsAdjustingStock);
            CancelAdjustCommand   = new RelayCommand(() => IsAdjustingStock = false);
            OpenAddVariantCommand = new RelayCommand(OpenAddVariant, () => EditProduct != null && EditProduct.ProductId > 0);
            SaveVariantCommand    = new RelayCommand(async () => await SaveVariantAsync(), () => IsAddingVariant);
            CancelVariantCommand  = new RelayCommand(() => IsAddingVariant = false);
            AddImageCommand       = new RelayCommand(async () => await AddImageAsync(),
                                        () => EditProduct?.ProductId > 0 && !string.IsNullOrWhiteSpace(NewImageUrl));
            RemoveImageCommand    = new RelayCommand<ProductImage>(async img => await RemoveImageAsync(img));
        }

        // ── Called by MainWindowViewModel when navigating here ────────────
        public async Task LoadAsync()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                var (products, categories, brands) = await LoadAllAsync();
                Products   = new ObservableCollection<Product>(products);
                Categories = new ObservableCollection<Category>(categories);
                Brands     = new ObservableCollection<Brand>(brands);
            }
            catch (Exception ex)
            {
                ShowError($"Failed to load products: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Private helpers ───────────────────────────────────────────────

        private async Task<(IEnumerable<Product>, IEnumerable<Category>, IEnumerable<Brand>)> LoadAllAsync()
        {
            var productsTask    = _productService.GetAllAsync();
            var categoriesTask  = _productService.GetAllCategoriesAsync();
            var brandsTask      = _productService.GetAllBrandsAsync();
            await Task.WhenAll(productsTask, categoriesTask, brandsTask);
            return (productsTask.Result, categoriesTask.Result, brandsTask.Result);
        }

        private async Task SearchAsync()
        {
            try
            {
                var results = string.IsNullOrWhiteSpace(SearchText)
                    ? await _productService.GetAllAsync()
                    : await _productService.SearchAsync(SearchText);

                Products = new ObservableCollection<Product>(results);
            }
            catch (Exception ex)
            {
                ShowError($"Search failed: {ex.Message}");
            }
        }

        private void OnProductSelected(Product? product)
        {
            if (product == null)
            {
                IsEditing   = false;
                EditProduct = null;
                EditImages  = new ObservableCollection<ProductImage>();
                return;
            }

            // Clone so edits don't mutate the list item until saved
            EditProduct = CloneProduct(product);
            IsEditing   = true;
            ClearMessages();
            IsAdjustingStock = false;
            IsAddingVariant  = false;
            NewImageUrl      = string.Empty;

            // Images load separately — they don't block selection
            _ = LoadImagesAsync(product.ProductId);
        }

        private void BeginAdd()
        {
            SelectedProduct  = null;
            EditProduct      = new Product { IsActive = true, Currency = "PHP" };
            IsEditing        = true;
            IsAdjustingStock = false;
            IsAddingVariant  = false;
            EditImages       = new ObservableCollection<ProductImage>();
            NewImageUrl      = string.Empty;
            ClearMessages();
        }

        private async Task SaveAsync()
        {
            if (EditProduct == null) return;

            if (string.IsNullOrWhiteSpace(EditProduct.Name))
            { ShowError("Product name is required."); return; }
            if (EditProduct.CategoryId == 0)
            { ShowError("Please select a category."); return; }
            if (EditProduct.Price < 0)
            { ShowError("Price cannot be negative."); return; }

            IsLoading = true;
            ClearMessages();
            try
            {
                if (EditProduct.ProductId == 0)
                {
                    int newId = await _productService.CreateAsync(EditProduct);
                    EditProduct.ProductId = newId;
                    ShowSuccess("Product created successfully.");
                }
                else
                {
                    await _productService.UpdateAsync(EditProduct);
                    ShowSuccess("Product updated successfully.");
                }
                await RefreshListAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Save failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CancelEdit()
        {
            EditProduct = null;
            IsEditing   = false;
            ClearMessages();
        }

        private async Task DeactivateAsync(Product? product)
        {
            if (product == null) return;
            IsLoading = true;
            ClearMessages();
            try
            {
                await _productService.DeactivateAsync(product.ProductId);
                ShowSuccess($"'{product.Name}' deactivated.");
                if (EditProduct?.ProductId == product.ProductId) CancelEdit();
                await RefreshListAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Deactivate failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OpenAdjust(ProductVariant? variant)
        {
            if (variant == null) return;
            AdjustTarget     = variant;
            AdjustQty        = 0;
            AdjustType       = InventoryChangeTypes.Adjustment;
            AdjustNotes      = string.Empty;
            IsAdjustingStock = true;
        }

        private async Task ConfirmAdjustAsync()
        {
            if (AdjustTarget == null) return;
            IsLoading = true;
            try
            {
                await _productService.AdjustStockAsync(
                    AdjustTarget.ProductVariantId, AdjustQty, AdjustType, AdjustNotes);

                IsAdjustingStock = false;
                ShowSuccess("Stock adjusted.");
                await RefreshListAsync();

                // Re-select the same product so variant table refreshes
                if (SelectedProduct != null)
                {
                    var refreshed = Products.FirstOrDefault(p => p.ProductId == SelectedProduct.ProductId);
                    if (refreshed != null) OnProductSelected(refreshed);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Adjustment failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OpenAddVariant()
        {
            NewVariant      = new ProductVariant
            {
                ProductId        = EditProduct!.ProductId,
                IsActive         = true,
                ReorderThreshold = 5,
            };
            IsAddingVariant = true;
        }

        private async Task SaveVariantAsync()
        {
            if (string.IsNullOrWhiteSpace(NewVariant.VariantName))
            { ShowError("Variant name is required."); return; }

            IsLoading = true;
            try
            {
                await _productService.AddVariantAsync(NewVariant);
                IsAddingVariant = false;
                ShowSuccess("Variant added.");
                await RefreshListAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Add variant failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Image actions ─────────────────────────────────────────────────

        private async Task LoadImagesAsync(int productId)
        {
            try
            {
                var images = await _productService.GetImagesAsync(productId);
                EditImages = new ObservableCollection<ProductImage>(images);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Products] Image load failed: {ex}");
            }
        }

        private async Task AddImageAsync()
        {
            if (EditProduct == null || EditProduct.ProductId == 0) return;
            if (string.IsNullOrWhiteSpace(NewImageUrl)) return;

            IsLoading = true;
            ClearMessages();
            try
            {
                await _productService.AddImageAsync(EditProduct.ProductId, NewImageUrl);
                NewImageUrl = string.Empty;
                await LoadImagesAsync(EditProduct.ProductId);
                ShowSuccess($"Image added for '{EditProduct.Name}'. URL reference saved (stored externally via Cloudinary).");
            }
            catch (Exception ex)
            {
                ShowError($"Failed to add image: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task RemoveImageAsync(ProductImage? image)
        {
            if (image == null) return;

            IsLoading = true;
            ClearMessages();
            try
            {
                await _productService.DeleteImageAsync(image.ProductImageId);
                EditImages.Remove(image);
            }
            catch (Exception ex)
            {
                ShowError($"Failed to remove image: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── List refresh helper ───────────────────────────────────────────

        private async Task RefreshListAsync()
        {
            var products = string.IsNullOrWhiteSpace(SearchText)
                ? await _productService.GetAllAsync()
                : await _productService.SearchAsync(SearchText);
            Products = new ObservableCollection<Product>(products);
        }

        private static Product CloneProduct(Product src) => new()
        {
            ProductId        = src.ProductId,
            CategoryId       = src.CategoryId,
            BrandId          = src.BrandId,
            Name             = src.Name,
            ShortDescription = src.ShortDescription,
            Description      = src.Description,
            Price            = src.Price,
            Currency         = src.Currency,
            IsActive         = src.IsActive,
            IsFeatured       = src.IsFeatured,
            CategoryName     = src.CategoryName,
            BrandName        = src.BrandName,
            Variants         = new List<ProductVariant>(src.Variants ?? new()),
        };
    }
}
