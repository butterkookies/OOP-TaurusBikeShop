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
        private readonly IReceiptPrintService _printService;
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

        // ── Voucher ───────────────────────────────────────────────────────────

        private string _voucherCodeInput = string.Empty;
        public string VoucherCodeInput
        {
            get => _voucherCodeInput;
            set
            {
                if (SetProperty(ref _voucherCodeInput, value))
                    ApplyVoucherFilter();
            }
        }

        // Suggestions dropdown
        private List<POSVoucherSuggestion> _allVoucherSuggestions = new();

        private ObservableCollection<POSVoucherSuggestion> _voucherSuggestions = new();
        public ObservableCollection<POSVoucherSuggestion> VoucherSuggestions
        {
            get => _voucherSuggestions;
            private set => SetProperty(ref _voucherSuggestions, value);
        }

        private bool _isVoucherDropdownOpen;
        public bool IsVoucherDropdownOpen
        {
            get => _isVoucherDropdownOpen;
            set => SetProperty(ref _isVoucherDropdownOpen, value);
        }

        private POSVoucherResult? _appliedVoucher;
        public POSVoucherResult? AppliedVoucher
        {
            get => _appliedVoucher;
            private set
            {
                SetProperty(ref _appliedVoucher, value);
                OnPropertyChanged(nameof(HasVoucherApplied));
                OnPropertyChanged(nameof(VoucherDiscountDisplay));
                OnPropertyChanged(nameof(DiscountAmount));
                RefreshTotals();
            }
        }

        public bool   HasVoucherApplied    => _appliedVoucher is { IsValid: true };
        public string VoucherDiscountDisplay => _appliedVoucher?.FormattedDiscount ?? string.Empty;

        private string _voucherError = string.Empty;
        public string VoucherError
        {
            get => _voucherError;
            set
            {
                SetProperty(ref _voucherError, value);
                OnPropertyChanged(nameof(HasVoucherError));
            }
        }
        public bool HasVoucherError => !string.IsNullOrEmpty(_voucherError);

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

        public decimal DiscountAmount => _appliedVoucher is { IsValid: true }
            ? _appliedVoucher.DiscountAmount
            : 0m;

        public decimal Subtotal    => CartItems.Sum(i => i.LineTotal);
        public decimal GrandTotal  => Math.Max(0m, Subtotal - DiscountAmount);
        public decimal Change      => IsCashPayment ? Math.Max(0m, CashReceived - GrandTotal) : 0m;

        // ── Session ───────────────────────────────────────────────────────────

        private POSSession? _activeSession;
        public POSSession? ActiveSession
        {
            get => _activeSession;
            private set
            {
                SetProperty(ref _activeSession, value);
                OnPropertyChanged(nameof(HasActiveSession));
                OnPropertyChanged(nameof(SessionTerminalName));
                OnPropertyChanged(nameof(SessionShiftStart));
                OnPropertyChanged(nameof(SessionTotalSales));
            }
        }

        public bool      HasActiveSession    => _activeSession != null;
        public string    SessionTerminalName => _activeSession?.TerminalName ?? string.Empty;
        public DateTime? SessionShiftStart   => _activeSession?.ShiftStart.ToLocalTime();
        public decimal   SessionTotalSales   => _activeSession?.TotalSales   ?? 0m;

        private string _terminalNameInput = "POS-TERMINAL-01";
        public string TerminalNameInput
        {
            get => _terminalNameInput;
            set => SetProperty(ref _terminalNameInput, value);
        }

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
        public ICommand PrintReceiptCommand       { get; }
        public ICommand ApplyVoucherCommand            { get; }
        public ICommand RemoveVoucherCommand           { get; }
        public ICommand LoadVoucherSuggestionsCommand  { get; }
        public ICommand SelectVoucherCommand           { get; }
        public ICommand CloseVoucherDropdownCommand    { get; }
        public ICommand OpenSessionCommand             { get; }
        public ICommand CloseSessionCommand            { get; }

        // ── Constructor ───────────────────────────────────────────────────────

        public POSViewModel(IPOSService pos, IReceiptPrintService printService)
        {
            _pos          = pos;
            _printService = printService;

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
            PrintReceiptCommand       = new RelayCommand(() => { if (LastResult != null) _printService.PrintReceipt(LastResult); });
            ApplyVoucherCommand           = new RelayCommand(async () => await ApplyVoucherAsync());
            RemoveVoucherCommand          = new RelayCommand(RemoveVoucher);
            LoadVoucherSuggestionsCommand = new RelayCommand(async () => await LoadVoucherSuggestionsAsync());
            SelectVoucherCommand          = new RelayCommand<POSVoucherSuggestion>(SelectVoucher);
            CloseVoucherDropdownCommand   = new RelayCommand(() => IsVoucherDropdownOpen = false);
            OpenSessionCommand            = new RelayCommand(async () => await OpenShiftAsync());
            CloseSessionCommand           = new RelayCommand(async () => await CloseShiftAsync(), () => HasActiveSession && !IsLoading);
        }

        // ── Called by MainWindowViewModel on navigate ─────────────────────────

        public async Task LoadAsync()
        {
            try
            {
                _walkInUserId = await _pos.GetWalkInUserIdAsync();
                ActiveSession = await _pos.GetActiveSessionAsync(CashierId);
            }
            catch (Exception ex)
            {
                ShowError($"Could not load POS data: {ex.Message}");
            }
        }

        // ── Session ───────────────────────────────────────────────────────────

        private async Task OpenShiftAsync()
        {
            if (string.IsNullOrWhiteSpace(TerminalNameInput))
            {
                ShowError("Please enter a terminal name.");
                return;
            }

            IsLoading = true;
            ClearMessages();
            try
            {
                string terminal = TerminalNameInput.Trim();
                int sessionId   = await _pos.OpenSessionAsync(CashierId, terminal);
                ActiveSession   = new POSSession
                {
                    POSSessionId = sessionId,
                    UserId       = CashierId,
                    TerminalName = terminal,
                    ShiftStart   = DateTime.UtcNow,
                    TotalSales   = 0m
                };
            }
            catch (Exception ex)
            {
                ShowError($"Failed to open shift: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task CloseShiftAsync()
        {
            if (_activeSession == null) return;

            IsLoading = true;
            ClearMessages();
            try
            {
                await _pos.CloseSessionAsync(_activeSession.POSSessionId);
                ActiveSession = null;
                StartNewSale();
            }
            catch (Exception ex)
            {
                ShowError($"Failed to close shift: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
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

            // Re-validate applied voucher when cart changes (subtotal may affect min order / discount)
            if (HasVoucherApplied)
                _ = RevalidateVoucherAsync();
        }

        private void RemoveFromCart(POSCartItem? item)
        {
            if (item == null) return;
            item.PropertyChanged -= OnCartItemChanged;
            CartItems.Remove(item);
            RefreshTotals();

            if (HasVoucherApplied)
                _ = RevalidateVoucherAsync();
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

            if (HasVoucherApplied)
                _ = RevalidateVoucherAsync();
        }

        private void DecrementQty(POSCartItem? item)
        {
            if (item == null) return;
            if (item.Quantity <= 1) { RemoveFromCart(item); return; }
            item.Quantity--;
            RefreshTotals();

            if (HasVoucherApplied)
                _ = RevalidateVoucherAsync();
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
            OnPropertyChanged(nameof(DiscountAmount));
            OnPropertyChanged(nameof(GrandTotal));
            OnPropertyChanged(nameof(Change));
        }

        // ── Voucher ──────────────────────────────────────────────────────────

        private async Task LoadVoucherSuggestionsAsync()
        {
            if (HasVoucherApplied) return;

            int userId = SelectedCustomer?.UserId ?? _walkInUserId;
            try
            {
                var suggestions = await _pos.GetVoucherSuggestionsAsync(userId);
                _allVoucherSuggestions = suggestions.ToList();
                ApplyVoucherFilter();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[POS] Failed to load voucher suggestions: {ex.Message}");
            }
        }

        private void ApplyVoucherFilter()
        {
            string filter = _voucherCodeInput.Trim().ToUpperInvariant();
            IEnumerable<POSVoucherSuggestion> filtered = string.IsNullOrEmpty(filter)
                ? _allVoucherSuggestions
                : _allVoucherSuggestions.Where(s => s.Code.StartsWith(filter, StringComparison.OrdinalIgnoreCase));

            VoucherSuggestions     = new ObservableCollection<POSVoucherSuggestion>(filtered);
            IsVoucherDropdownOpen  = VoucherSuggestions.Count > 0 && !HasVoucherApplied;
        }

        private void SelectVoucher(POSVoucherSuggestion? suggestion)
        {
            if (suggestion == null) return;
            // Temporarily suppress filter re-run by bypassing the public setter
            _voucherCodeInput      = suggestion.Code;
            OnPropertyChanged(nameof(VoucherCodeInput));
            IsVoucherDropdownOpen  = false;
        }

        private async Task ApplyVoucherAsync()
        {
            VoucherError          = string.Empty;
            IsVoucherDropdownOpen = false;

            if (string.IsNullOrWhiteSpace(VoucherCodeInput))
            {
                VoucherError = "Please enter a voucher code.";
                return;
            }

            if (CartItems.Count == 0)
            {
                VoucherError = "Add items to the cart before applying a voucher.";
                return;
            }

            int userId = SelectedCustomer?.UserId ?? _walkInUserId;

            try
            {
                var result = await _pos.ValidateVoucherAsync(
                    VoucherCodeInput.Trim(), userId, Subtotal);

                if (result.IsValid)
                {
                    AppliedVoucher        = result;
                    VoucherError          = string.Empty;
                    IsVoucherDropdownOpen = false;
                }
                else
                {
                    AppliedVoucher = null;
                    VoucherError   = result.Error;
                }
            }
            catch (Exception ex)
            {
                AppliedVoucher = null;
                VoucherError   = $"Voucher validation failed: {ex.Message}";
            }
        }

        private void RemoveVoucher()
        {
            AppliedVoucher        = null;
            VoucherCodeInput      = string.Empty;
            VoucherError          = string.Empty;
            _allVoucherSuggestions.Clear();
            VoucherSuggestions     = new ObservableCollection<POSVoucherSuggestion>();
            IsVoucherDropdownOpen  = false;
        }

        private async Task RevalidateVoucherAsync()
        {
            if (_appliedVoucher == null) return;

            int userId = SelectedCustomer?.UserId ?? _walkInUserId;

            try
            {
                var result = await _pos.ValidateVoucherAsync(
                    _appliedVoucher.VoucherCode, userId, Subtotal);

                if (result.IsValid)
                {
                    AppliedVoucher = result;
                    VoucherError   = string.Empty;
                }
                else
                {
                    AppliedVoucher = null;
                    VoucherError   = result.Error;
                }
            }
            catch
            {
                AppliedVoucher = null;
                VoucherError   = "Voucher could not be re-validated.";
            }
        }

        // ── Customer ──────────────────────────────────────────────────────────

        private void SelectCustomer(POSCustomer? customer)
        {
            SelectedCustomer         = customer;
            IsCustomerDropdownOpen   = false;
            CustomerSearch           = string.Empty;
            CustomerResults          = new ObservableCollection<POSCustomer>();

            // Re-validate voucher for the new customer (per-user cap may differ)
            if (HasVoucherApplied)
                _ = RevalidateVoucherAsync();

            // Refresh available voucher suggestions for this customer
            _allVoucherSuggestions.Clear();
            VoucherSuggestions     = new ObservableCollection<POSVoucherSuggestion>();
            IsVoucherDropdownOpen  = false;
        }

        private void SetWalkIn()
        {
            SelectedCustomer         = null;
            IsCustomerDropdownOpen   = false;
            CustomerSearch           = string.Empty;
            CustomerResults          = new ObservableCollection<POSCustomer>();

            if (HasVoucherApplied)
                _ = RevalidateVoucherAsync();

            // Refresh available voucher suggestions for walk-in
            _allVoucherSuggestions.Clear();
            VoucherSuggestions     = new ObservableCollection<POSVoucherSuggestion>();
            IsVoucherDropdownOpen  = false;
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

        private bool CanCompleteSale() =>
            CartItems.Count > 0 && !IsLoading && !IsCardPayment && HasActiveSession;

        private async Task CompleteSaleAsync()
        {
            if (CartItems.Count == 0)   { ShowError("Cart is empty.");                               return; }
            if (!HasActiveSession)      { ShowError("No active session. Open a shift first.");        return; }
            if (IsCardPayment)          { ShowError("Card payment is not yet available.");            return; }
            if (IsCashPayment && CashReceived < GrandTotal)
            {
                ShowError($"Cash received (\u20b1{CashReceived:N2}) is less than total (\u20b1{GrandTotal:N2}).");
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
                    _activeSession?.POSSessionId,
                    CustomerDisplayName,
                    CartItems.ToList(),
                    SelectedPaymentMethod,
                    CashReceived,
                    DiscountAmount,
                    _appliedVoucher?.VoucherCode);

                result.CashierName = CashierName;
                LastResult         = result;
                IsReceiptVisible   = true;

                // Keep local TotalSales in sync without a round-trip
                if (_activeSession != null)
                {
                    _activeSession.TotalSales += result.GrandTotal;
                    OnPropertyChanged(nameof(SessionTotalSales));
                }
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
            VoucherCodeInput           = string.Empty;
            AppliedVoucher             = null;
            VoucherError               = string.Empty;
            _allVoucherSuggestions.Clear();
            VoucherSuggestions         = new ObservableCollection<POSVoucherSuggestion>();
            IsVoucherDropdownOpen      = false;
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
