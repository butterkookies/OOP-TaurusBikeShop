using System.Collections.ObjectModel;
using System.Linq;
using AdminSystem.Models;
using AdminSystem.Services;

namespace AdminSystem.ViewModels
{
    public class POSViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private const decimal VAT_RATE = 0.12m;

        public POSViewModel(IProductService productService)
        {
            _productService = productService;
            CartItems    = new ObservableCollection<POSCartItem>();
            Products     = new ObservableCollection<Product>();

            CartItems.CollectionChanged += (s, e) => RefreshTotals();

            LoadProductsCommand   = new RelayCommand(LoadProducts);
            AddToCartCommand      = new RelayCommand(AddToCart);
            IncrementCommand      = new RelayCommand(IncrementItem);
            DecrementCommand      = new RelayCommand(DecrementItem);
            RemoveItemCommand     = new RelayCommand(RemoveItem);
            ClearCartCommand      = new RelayCommand(ClearCart,
                _ => CartItems.Count > 0);
            ApplyDiscountCommand  = new RelayCommand(ApplyDiscount);
        }

        // ── Product list ────────────────────────────────────────────────
        public ObservableCollection<Product> Products { get; }

        public event System.EventHandler ProductsRefreshed;

        private string _productSearch;
        public string ProductSearch
        {
            get { return _productSearch; }
            set
            {
                SetField(ref _productSearch, value, "ProductSearch");
                SearchProducts();
            }
        }

        // ── Cart ────────────────────────────────────────────────────────
        public ObservableCollection<POSCartItem> CartItems { get; }

        private decimal _discountAmount;
        public decimal DiscountAmount
        {
            get { return _discountAmount; }
            set
            {
                SetField(ref _discountAmount, value, "DiscountAmount");
                RefreshTotals();
            }
        }

        private string _discountInput;
        public string DiscountInput
        {
            get { return _discountInput; }
            set { SetField(ref _discountInput, value, "DiscountInput"); }
        }

        // ── Totals ──────────────────────────────────────────────────────
        private int _totalItems;
        public int TotalItems
        {
            get { return _totalItems; }
            set { SetField(ref _totalItems, value, "TotalItems"); }
        }

        private decimal _subtotal;
        public decimal Subtotal
        {
            get { return _subtotal; }
            set
            {
                SetField(ref _subtotal, value, "Subtotal");
                OnPropertyChanged("SubtotalDisplay");
            }
        }
        public string SubtotalDisplay
            => string.Format("\u20B1 {0:N2}", _subtotal);

        private decimal _vat;
        public decimal VAT
        {
            get { return _vat; }
            set
            {
                SetField(ref _vat, value, "VAT");
                OnPropertyChanged("VATDisplay");
            }
        }
        public string VATDisplay
            => string.Format("\u20B1 {0:N2}", _vat);

        private decimal _grandTotal;
        public decimal GrandTotal
        {
            get { return _grandTotal; }
            set
            {
                SetField(ref _grandTotal, value, "GrandTotal");
                OnPropertyChanged("GrandTotalDisplay");
            }
        }
        public string GrandTotalDisplay
            => string.Format("\u20B1 {0:N2}", _grandTotal);

        public string DiscountDisplay
            => _discountAmount > 0
                ? string.Format("- \u20B1 {0:N2}", _discountAmount)
                : "\u20B1 0.00";

        // ── Commands ────────────────────────────────────────────────────
        public RelayCommand LoadProductsCommand  { get; }
        public RelayCommand AddToCartCommand     { get; }
        public RelayCommand IncrementCommand     { get; }
        public RelayCommand DecrementCommand     { get; }
        public RelayCommand RemoveItemCommand    { get; }
        public RelayCommand ClearCartCommand     { get; }
        public RelayCommand ApplyDiscountCommand { get; }

        // ── Methods ─────────────────────────────────────────────────────
        public void LoadProducts()
        {
            IsLoading = true;
            try
            {
                Products.Clear();
                foreach (Product p in _productService.GetAllProducts())
                    if (p.IsActive) Products.Add(p);
                ProductsRefreshed?.Invoke(this, System.EventArgs.Empty);
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
            finally { IsLoading = false; }
        }

        private void SearchProducts()
        {
            try
            {
                Products.Clear();
                System.Collections.Generic.IEnumerable<Product> result =
                    string.IsNullOrWhiteSpace(ProductSearch)
                        ? _productService.GetAllProducts()
                        : _productService.SearchProducts(ProductSearch);
                foreach (Product p in result)
                    if (p.IsActive) Products.Add(p);
                ProductsRefreshed?.Invoke(this, System.EventArgs.Empty);
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        public void AddToCart(object param)
        {
            Product product = param as Product;
            if (product == null) return;

            ProductVariant variant = product.Variants
                .FirstOrDefault(v => v.IsActive && v.StockQuantity > 0);

            if (variant == null)
            {
                ShowError(product.Name + " is out of stock.");
                return;
            }

            decimal price = product.Price + variant.AdditionalPrice;

            POSCartItem existing = CartItems
                .FirstOrDefault(i => i.ProductId == product.ProductId);

            if (existing != null)
            {
                int idx = CartItems.IndexOf(existing);
                existing.Quantity++;
                CartItems[idx] = existing;
            }
            else
            {
                CartItems.Add(new POSCartItem
                {
                    ProductId = product.ProductId,
                    Name      = product.Name,
                    UnitPrice = price,
                    Quantity  = 1
                });
            }
        }

        private void IncrementItem(object param)
        {
            POSCartItem item = param as POSCartItem;
            if (item == null) return;
            int idx = CartItems.IndexOf(item);
            if (idx < 0) return;
            item.Quantity++;
            CartItems[idx] = item;
        }

        private void DecrementItem(object param)
        {
            POSCartItem item = param as POSCartItem;
            if (item == null) return;
            int idx = CartItems.IndexOf(item);
            if (idx < 0) return;
            if (item.Quantity > 1) { item.Quantity--; CartItems[idx] = item; }
            else CartItems.RemoveAt(idx);
        }

        private void RemoveItem(object param)
        {
            POSCartItem item = param as POSCartItem;
            if (item != null) CartItems.Remove(item);
        }

        private void ClearCart(object param)
        {
            CartItems.Clear();
            DiscountAmount = 0m;
            DiscountInput  = string.Empty;
        }

        private void ApplyDiscount(object param)
        {
            decimal disc;
            if (!decimal.TryParse(DiscountInput, out disc) || disc < 0)
            {
                ShowError("Enter a valid discount amount.");
                return;
            }
            decimal sub = CartItems.Sum(i => i.Subtotal);
            if (disc > sub)
            {
                ShowError("Discount cannot exceed the subtotal.");
                return;
            }
            DiscountAmount = disc;
            ClearMessages();
        }

        private void RefreshTotals()
        {
            TotalItems = CartItems.Sum(i => i.Quantity);
            decimal sub   = CartItems.Sum(i => i.Subtotal);
            decimal after = sub - _discountAmount;
            decimal vat   = after * VAT_RATE;
            Subtotal   = sub;
            VAT        = vat;
            GrandTotal = after + vat;
            OnPropertyChanged("DiscountDisplay");
        }
    }

    // ── POS Cart Item ──────────────────────────────────────────────────────
    public class POSCartItem : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnChanged(string name)
            => PropertyChanged?.Invoke(this,
                new System.ComponentModel.PropertyChangedEventArgs(name));

        public int     ProductId { get; set; }
        public string  Name      { get; set; }
        public decimal UnitPrice { get; set; }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnChanged("Quantity");
                OnChanged("Subtotal");
                OnChanged("SubtotalDisplay");
            }
        }

        public decimal Subtotal         => UnitPrice * Quantity;
        public string  UnitPriceDisplay => string.Format("\u20B1 {0:N2}", UnitPrice);
        public string  SubtotalDisplay  => string.Format("\u20B1 {0:N2}", Subtotal);
    }
}
