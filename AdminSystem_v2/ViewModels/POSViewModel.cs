using System.Collections.ObjectModel;
using System.Windows.Input;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;

namespace AdminSystem_v2.ViewModels
{
    public class POSViewModel : BaseViewModel
    {
        private readonly IPOSService _pos;
        private int _walkInUserId;

        private int    CashierId   => App.CurrentUser?.UserId    ?? 0;
        private string CashierName => App.CurrentUser?.FullName  ?? "Staff";

        // ── Product search ────────────────────────────────────────────────────

        private string _productSearch = string.Empty;
        public string ProductSearch
        {
            get => _productSearch;
            set { if (SetProperty(ref _productSearch, value)) _ = LoadProductsAsync(); }
        }

        private ObservableCollection<POSProductItem> _searchResults = new();
        public ObservableCollection<POSProductItem> SearchResults
        {
            get => _searchResults;
            private set => SetProperty(ref _searchResults, value);
        }

        // ── Customer ──────────────────────────────────────────────────────────

        private string _customerSearch = string.Empty;
        public string CustomerSearch
        {
            get => _customerSearch;
            set { if (SetProperty(ref _customerSearch, value)) _ = LoadCustomersAsync(); }
        }

        private ObservableCollection<POSCustomer> _customerResults = new();
        public ObservableCollection<POSCustomer> CustomerResults
        {
            get => _customerResults;
            private set => SetProperty(ref _customerResults, value);
        }

        private bool _isCustomerDropdownOpen;
        public bool IsCustomerDropdownOpen
        {
            get => _isCustomerDropdownOpen;
            set => SetProperty(ref _isCustomerDropdownOpen, value);
        }

        private POSCustomer? _selectedCustomer;
        public POSCustomer? SelectedCustomer
        {
            get => _selectedCustomer;
            private set
            {
                SetProperty(ref _selectedCustomer, value);
                OnPropertyChanged(nameof(CustomerDisplayName));
            }
        }

        public string CustomerDisplayName => _selectedCustomer?.Name ?? "Walk-in Customer";

        // ── Cart ──────────────────────────────────────────────────────────────

        private ObservableCollection<POSCartItem> _cartItems = new();
        public ObservableCollection<POSCartItem> CartItems
        {
            get => _cartItems;
            private set => SetProperty(ref _cartItems, value);
        }

        public bool IsCartEmpty => CartItems.Count == 0;

        // ── Payment ───────────────────────────────────────────────────────────

        private string _selectedPaymentMethod = POSPaymentMethods.Cash;
        public string SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            private set
            {
                SetProperty(ref _selectedPaymentMethod, value);
                OnPropertyChanged(nameof(IsCashPayment));
                OnPropertyChanged(nameof(IsCardPayment));
                OnPropertyChanged(nameof(Change));
                ClearMessages();
            }
        }

        public bool IsCashPayment => SelectedPaymentMethod == POSPaymentMethods.Cash;
        public bool IsCardPayment  => SelectedPaymentMethod == POSPaymentMethods.Card;

        private string _cashReceivedText = "0";
        public string CashReceivedText
        {
            get => _cashReceivedText;
            set
            {
                SetProperty(ref _cashReceivedText, value);
                OnPropertyChanged(nameof(Change));
            }
        }

        private decimal CashReceived =>
            decimal.TryParse(_cashReceivedText, out var v) ? v : 0m;

        private string _discountText = "0";
        public string DiscountText
        {
            get => _discountText;
            set
            {
                SetProperty(ref _discountText, value);
                OnPropertyChanged(nameof(GrandTotal));
                OnPropertyChanged(nameof(Change));
            }
        }

        private decimal DiscountAmount =>
            decimal.TryParse(_discountText, out var v) ? Math.Max(0m, v) : 0m;

        public decimal Subtotal    => CartItems.Sum(i => i.LineTotal);
        public decimal GrandTotal  => Math.Max(0m, Subtotal - DiscountAmount);
        public decimal Change      => IsCashPayment ? Math.Max(0m, CashReceived - GrandTotal) : 0m;

        // ── Receipt ───────────────────────────────────────────────────────────

        private bool _isReceiptVisible;
        public bool IsReceiptVisible
        {
            get => _isReceiptVisible;
            set => SetProperty(ref _isReceiptVisible, value);
        }

        private POSOrderResult? _lastResult;
        public POSOrderResult? LastResult
        {
            get => _lastResult;
            set => SetProperty(ref _lastResult, value);
        }

        // ── Commands ──────────────────────────────────────────────────────────

        public ICommand AddToCartCommand          { get; }
        public ICommand RemoveFromCartCommand     { get; }
        public ICommand IncrementQtyCommand       { get; }
        public ICommand DecrementQtyCommand       { get; }
        public ICommand SelectPaymentMethodCommand { get; }
        public ICommand SelectCustomerCommand     { get; }
        public ICommand SetWalkInCommand          { get; }
        public ICommand ToggleCustomerDropdownCommand { get; }
        public ICommand CompleteSaleCommand       { get; }
        public ICommand NewSaleCommand            { get; }

        // ── Constructor ───────────────────────────────────────────────────────

        public POSViewModel(IPOSService pos)
        {
            _pos = pos;

            AddToCartCommand          = new RelayCommand<POSProductItem>(AddToCart);
            RemoveFromCartCommand     = new RelayCommand<POSCartItem>(RemoveFromCart);
            IncrementQtyCommand       = new RelayCommand<POSCartItem>(IncrementQty);
            DecrementQtyCommand       = new RelayCommand<POSCartItem>(DecrementQty);
            SelectPaymentMethodCommand = new RelayCommand<string>(m => SelectedPaymentMethod = m ?? POSPaymentMethods.Cash);
            SelectCustomerCommand     = new RelayCommand<POSCustomer>(SelectCustomer);
            SetWalkInCommand          = new RelayCommand(SetWalkIn);
            ToggleCustomerDropdownCommand = new RelayCommand(() => IsCustomerDropdownOpen = !IsCustomerDropdownOpen);
            CompleteSaleCommand       = new RelayCommand(async () => await CompleteSaleAsync(), CanCompleteSale);
            NewSaleCommand            = new RelayCommand(StartNewSale);
        }

        // ── Called by MainWindowViewModel on navigate ─────────────────────────

        public async Task LoadAsync()
        {
            try
            {
                _walkInUserId = await _pos.GetWalkInUserIdAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Could not resolve walk-in user: {ex.Message}");
            }
        }

        // ── Cart operations ───────────────────────────────────────────────────

        private void AddToCart(POSProductItem? item)
        {
            if (item == null) return;

            if (!item.InStock)
            {
                ShowError($"\"{item.DisplayName}\" is out of stock.");
                return;
            }

            var existing = CartItems.FirstOrDefault(c => c.ProductVariantId == item.ProductVariantId);
            if (existing != null)
            {
                if (existing.Quantity >= item.StockQuantity)
                {
                    ShowError($"Maximum available stock ({item.StockQuantity}) already in cart.");
                    return;
                }
                existing.Quantity++;
            }
            else
            {
                var cartItem = new POSCartItem
                {
                    ProductId        = item.ProductId,
                    ProductVariantId = item.ProductVariantId,
                    ProductName      = item.ProductName,
                    VariantName      = item.VariantName,
                    UnitPrice        = item.UnitPrice,
                    AvailableStock   = item.StockQuantity,
                    Quantity         = 1
                };
                cartItem.PropertyChanged += OnCartItemChanged;
                CartItems.Add(cartItem);
            }

            RefreshTotals();
            ClearMessages();
        }

        private void RemoveFromCart(POSCartItem? item)
        {
            if (item == null) return;
            item.PropertyChanged -= OnCartItemChanged;
            CartItems.Remove(item);
            RefreshTotals();
        }

        private void IncrementQty(POSCartItem? item)
        {
            if (item == null) return;
            if (item.Quantity >= item.AvailableStock)
            {
                ShowError($"Maximum available stock ({item.AvailableStock}) reached.");
                return;
            }
            item.Quantity++;
            RefreshTotals();
        }

        private void DecrementQty(POSCartItem? item)
        {
            if (item == null) return;
            if (item.Quantity <= 1) { RemoveFromCart(item); return; }
            item.Quantity--;
            RefreshTotals();
        }

        private void OnCartItemChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(POSCartItem.LineTotal) or nameof(POSCartItem.Quantity))
                RefreshTotals();
        }

        private void RefreshTotals()
        {
            OnPropertyChanged(nameof(IsCartEmpty));
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(GrandTotal));
            OnPropertyChanged(nameof(Change));
        }

        // ── Customer ──────────────────────────────────────────────────────────

        private void SelectCustomer(POSCustomer? customer)
        {
            SelectedCustomer         = customer;
            IsCustomerDropdownOpen   = false;
            CustomerSearch           = string.Empty;
            CustomerResults          = new ObservableCollection<POSCustomer>();
        }

        private void SetWalkIn()
        {
            SelectedCustomer         = null;
            IsCustomerDropdownOpen   = false;
            CustomerSearch           = string.Empty;
            CustomerResults          = new ObservableCollection<POSCustomer>();
        }

        // ── Search helpers ────────────────────────────────────────────────────

        private async Task LoadProductsAsync()
        {
            if (string.IsNullOrWhiteSpace(ProductSearch))
            {
                SearchResults = new ObservableCollection<POSProductItem>();
                return;
            }
            try
            {
                var results = await _pos.SearchProductsAsync(ProductSearch);
                SearchResults = new ObservableCollection<POSProductItem>(results);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[POS] Product search failed: {ex.Message}");
            }
        }

        private async Task LoadCustomersAsync()
        {
            if (string.IsNullOrWhiteSpace(CustomerSearch))
            {
                CustomerResults = new ObservableCollection<POSCustomer>();
                return;
            }
            try
            {
                var results = await _pos.SearchCustomersAsync(CustomerSearch);
                CustomerResults = new ObservableCollection<POSCustomer>(results);
                IsCustomerDropdownOpen = CustomerResults.Count > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[POS] Customer search failed: {ex.Message}");
            }
        }

        // ── Checkout ──────────────────────────────────────────────────────────

        private bool CanCompleteSale() => CartItems.Count > 0 && !IsLoading && !IsCardPayment;

        private async Task CompleteSaleAsync()
        {
            if (CartItems.Count == 0)   { ShowError("Cart is empty.");                               return; }
            if (IsCardPayment)          { ShowError("Card payment is not yet available.");            return; }
            if (IsCashPayment && CashReceived < GrandTotal)
            {
                ShowError($"Cash received (₱{CashReceived:N2}) is less than total (₱{GrandTotal:N2}).");
                return;
            }

            int userId = SelectedCustomer?.UserId ?? _walkInUserId;
            if (userId == 0) { ShowError("Walk-in user not configured. Contact administrator."); return; }

            IsLoading = true;
            ClearMessages();
            try
            {
                var result = await _pos.CreatePOSSaleAsync(
                    userId,
                    CashierId,
                    CustomerDisplayName,
                    CartItems.ToList(),
                    SelectedPaymentMethod,
                    CashReceived,
                    DiscountAmount);

                result.CashierName = CashierName;
                LastResult         = result;
                IsReceiptVisible   = true;
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError($"Sale failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── New sale ──────────────────────────────────────────────────────────

        private void StartNewSale()
        {
            foreach (var item in CartItems)
                item.PropertyChanged -= OnCartItemChanged;

            CartItems.Clear();
            SelectedCustomer       = null;
            SelectedPaymentMethod  = POSPaymentMethods.Cash;
            CashReceivedText       = "0";
            DiscountText           = "0";
            ProductSearch          = string.Empty;
            CustomerSearch         = string.Empty;
            SearchResults          = new ObservableCollection<POSProductItem>();
            CustomerResults        = new ObservableCollection<POSCustomer>();
            IsCustomerDropdownOpen = false;
            IsReceiptVisible       = false;
            LastResult             = null;
            RefreshTotals();
            ClearMessages();
        }
    }
}
