// ═══════════════════════════════════════════════════════════════════════
//  WalkInPOS_CartLogic.cs
//  Taurus Bike Shop — Walk-In POS Cart Feature
//
//  ADD THIS REGION into AdminDashboardWindow.xaml.cs
//
//  Features:
//  • CartItem model with quantity, price, subtotal
//  • AddToCart() triggered by product tile click OR "Add to Cart" button
//  • Live running total, item count, VAT breakdown
//  • Quantity increment / decrement per cart row
//  • Remove individual item
//  • Clear cart
//  • Checkout with confirmation dialog
// ═══════════════════════════════════════════════════════════════════════

// ── 1. ADD THESE USINGS at the top of AdminDashboardWindow.xaml.cs ──────
using System.Collections.ObjectModel;
using System.Linq;

// ════════════════════════════════════════════════════════════════════════
//  2. CART ITEM MODEL  (place just before the AdminDashboardWindow class,
//     or in its own file CartItem.cs inside the TaurusBikeShop namespace)
// ════════════════════════════════════════════════════════════════════════
namespace TaurusBikeShop
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;

        /// Displayed in the DataGrid — e.g. "₱ 2,499.00"
        public string UnitPriceDisplay => $"₱ {UnitPrice:N2}";
        public string SubtotalDisplay => $"₱ {Subtotal:N2}";
    }
}

// ════════════════════════════════════════════════════════════════════════
//  3. PASTE THIS REGION inside the AdminDashboardWindow class body
//     (right after the existing Pending Orders region)
// ════════════════════════════════════════════════════════════════════════

/* ─── Walk-In POS ─────────────────────────────────────────────────────── */

// Cart data source bound to DgCart DataGrid in XAML
private ObservableCollection<CartItem> _cartItems = new ObservableCollection<CartItem>();

/// <summary>
/// Call once in the constructor, AFTER InitializeComponent():
///     InitWalkInPOS();
/// </summary>
private void InitWalkInPOS()
{
    // Bind cart grid
    DgCart.ItemsSource = _cartItems;
    _cartItems.CollectionChanged += (s, e) => RefreshCartTotals();

    // Product tile / Add-to-Cart button wiring
    // Each product Button must have:
    //   Tag="{Binding}"  or  Tag="ProductId|Name|Category|Price"  (pipe-delimited)
    // Wire them here — add as many as you have:
    POS_AddBike001.Click += POS_ProductButton_Click;
    POS_AddBike002.Click += POS_ProductButton_Click;
    POS_AddAccessory001.Click += POS_ProductButton_Click;
    POS_AddPart001.Click += POS_ProductButton_Click;
    // ... repeat for every product tile button

    // Cart action buttons
    POS_ClearCartButton.Click += POS_ClearCart_Click;
    POS_CheckoutButton.Click += POS_Checkout_Click;
    POS_ApplyDiscountButton.Click += POS_ApplyDiscount_Click;

    // Qty controls inside the DataGrid are wired via event bubbling — see
    // DgCart_CellButtonClick below (attach in XAML via ButtonBase.Click on DgCart)
}

// ════════════════════════════════════════════════════════════════════════
//  ADD TO CART  — called by both tile-click and explicit "Add" button
// ════════════════════════════════════════════════════════════════════════

/// <summary>
/// Primary entry point. Pass product details directly when a tile is clicked.
/// </summary>
private void AddToCart(string productId, string name, string category, decimal unitPrice)
{
    CartItem existing = _cartItems.FirstOrDefault(i => i.ProductId == productId);
    if (existing != null)
    {
        existing.Quantity++;
        // Force DataGrid row refresh (ObservableCollection doesn't track property changes)
        int idx = _cartItems.IndexOf(existing);
        _cartItems[idx] = existing;     // triggers CollectionChanged → RefreshCartTotals
    }
    else
    {
        _cartItems.Add(new CartItem
        {
            ProductId = productId,
            Name = name,
            Category = category,
            UnitPrice = unitPrice,
            Quantity = 1
        });
    }

    // Flash the cart panel to give visual feedback
    AnimateCartFlash();
}

// ════════════════════════════════════════════════════════════════════════
//  PRODUCT TILE / ADD-TO-CART BUTTON CLICK
//  Convention: Button.Tag = "ProductId|Display Name|Category|Price"
//  Example Tag value:  "BIKE-001|Trek FX 3 Disc|Bikes|24999.00"
// ════════════════════════════════════════════════════════════════════════
private void POS_ProductButton_Click(object sender, RoutedEventArgs e)
{
    Button btn = sender as Button;
    if (btn?.Tag == null) return;

    string[] parts = btn.Tag.ToString().Split('|');
    if (parts.Length < 4) return;

    if (!decimal.TryParse(parts[3], out decimal price)) return;

    AddToCart(
        productId: parts[0],
        name: parts[1],
        category: parts[2],
        unitPrice: price
    );
}

// ════════════════════════════════════════════════════════════════════════
//  DATAGRID QTY BUTTONS  (+ / − / Remove)
//  In XAML, set ButtonBase.Click="DgCart_CellButtonClick" on <DataGrid>
// ════════════════════════════════════════════════════════════════════════
private void DgCart_CellButtonClick(object sender, RoutedEventArgs e)
{
    if (!(e.OriginalSource is Button btn)) return;
    if (!(btn.DataContext is CartItem item)) return;

    int idx = _cartItems.IndexOf(item);
    if (idx < 0) return;

    switch (btn.Tag?.ToString())
    {
        case "Increment":
            item.Quantity++;
            _cartItems[idx] = item;
            break;

        case "Decrement":
            if (item.Quantity > 1)
            {
                item.Quantity--;
                _cartItems[idx] = item;
            }
            else
            {
                _cartItems.RemoveAt(idx);
            }
            break;

        case "Remove":
            _cartItems.RemoveAt(idx);
            break;
    }
}

// ════════════════════════════════════════════════════════════════════════
//  CART TOTALS  — called every time _cartItems changes
// ════════════════════════════════════════════════════════════════════════
private const decimal VAT_RATE = 0.12m;   // 12% Philippine VAT

private decimal _discountAmount = 0m;

private void RefreshCartTotals()
{
    int totalQty = _cartItems.Sum(i => i.Quantity);
    decimal subtotal = _cartItems.Sum(i => i.Subtotal);
    decimal afterDiscount = subtotal - _discountAmount;
    decimal vat = afterDiscount * VAT_RATE;
    decimal grandTotal = afterDiscount + vat;

    // Update UI labels (wire these x:Name values in XAML)
    POS_ItemCountLabel.Text = totalQty == 1
        ? "1 item"
        : totalQty + " items";

    POS_SubtotalLabel.Text = $"₱ {subtotal:N2}";
    POS_DiscountLabel.Text = _discountAmount > 0
        ? $"− ₱ {_discountAmount:N2}"
        : "₱ 0.00";
    POS_VATLabel.Text = $"₱ {vat:N2}";
    POS_GrandTotalLabel.Text = $"₱ {grandTotal:N2}";

    // Disable checkout if cart is empty
    POS_CheckoutButton.IsEnabled = _cartItems.Count > 0;
}

// ════════════════════════════════════════════════════════════════════════
//  DISCOUNT
// ════════════════════════════════════════════════════════════════════════
private void POS_ApplyDiscount_Click(object sender, RoutedEventArgs e)
{
    string input = POS_DiscountTextBox.Text.Trim();
    if (!decimal.TryParse(input, out decimal discount) || discount < 0)
    {
        MessageBox.Show("Please enter a valid discount amount.", "Invalid Discount",
            MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    decimal subtotal = _cartItems.Sum(i => i.Subtotal);
    if (discount > subtotal)
    {
        MessageBox.Show("Discount cannot exceed subtotal.", "Invalid Discount",
            MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    _discountAmount = discount;
    RefreshCartTotals();
}

// ════════════════════════════════════════════════════════════════════════
//  CLEAR CART
// ════════════════════════════════════════════════════════════════════════
private void POS_ClearCart_Click(object sender, RoutedEventArgs e)
{
    if (_cartItems.Count == 0) return;
    if (MessageBox.Show("Clear all items from the cart?", "Clear Cart",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
    {
        _cartItems.Clear();
        _discountAmount = 0m;
        POS_DiscountTextBox.Text = string.Empty;
    }
}

// ════════════════════════════════════════════════════════════════════════
//  CHECKOUT
// ════════════════════════════════════════════════════════════════════════
private void POS_Checkout_Click(object sender, RoutedEventArgs e)
{
    if (_cartItems.Count == 0) return;

    decimal subtotal = _cartItems.Sum(i => i.Subtotal);
    decimal after = subtotal - _discountAmount;
    decimal vat = after * VAT_RATE;
    decimal grandTotal = after + vat;

    string summary = string.Format(
        "Items : {0}\nSubtotal : ₱ {1:N2}\nDiscount : − ₱ {2:N2}\nVAT (12%) : ₱ {3:N2}\n\nTotal Due : ₱ {4:N2}\n\nProceed to payment?",
        _cartItems.Sum(i => i.Quantity),
        subtotal, _discountAmount, vat, grandTotal);

    if (MessageBox.Show(summary, "Confirm Checkout",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
    {
        // TODO: Save transaction to DB, print receipt
        MessageBox.Show("Transaction recorded! Receipt printing...", "Success",
            MessageBoxButton.OK, MessageBoxImage.Information);

        _cartItems.Clear();
        _discountAmount = 0m;
        POS_DiscountTextBox.Text = string.Empty;
    }
}

// ════════════════════════════════════════════════════════════════════════
//  VISUAL FEEDBACK — cart flash animation
// ════════════════════════════════════════════════════════════════════════
private void AnimateCartFlash()
{
    // Briefly highlights the cart panel border to confirm item was added.
    // Requires a Border named POS_CartPanelBorder in XAML.
    if (POS_CartPanelBorder == null) return;

    var highlight = new System.Windows.Media.Animation.ColorAnimation
    {
        To = System.Windows.Media.Color.FromRgb(0xFF, 0xD7, 0x00), // gold flash
        Duration = TimeSpan.FromMilliseconds(150),
        AutoReverse = true,
        FillBehavior = System.Windows.Media.Animation.FillBehavior.Stop
    };

    var brush = new System.Windows.Media.SolidColorBrush(
        ((System.Windows.Media.SolidColorBrush)POS_CartPanelBorder.BorderBrush).Color);
    POS_CartPanelBorder.BorderBrush = brush;
    brush.BeginAnimation(System.Windows.Media.SolidColorBrush.ColorProperty, highlight);
}

// ════════════════════════════════════════════════════════════════════════
//  4. XAML WIRING CHECKLIST  (PageWalkInPOS inside AdminDashboardWindow.xaml)
// ════════════════════════════════════════════════════════════════════════

  ── Product Tiles ────────────────────────────────────────────────────
  Each clickable product card/button:

      < Button x: Name = "POS_AddBike001"
              Tag = "BIKE-001|Trek FX 3 Disc|Bikes|24999.00"
              Style = "{StaticResource ProductTileStyle}" >
          < StackPanel >
              < TextBlock Text = "Trek FX 3 Disc" />
              < TextBlock Text = "₱ 24,999.00" />   ← this is the price that gets clicked
          </StackPanel>
      </Button>

  ── Cart DataGrid ────────────────────────────────────────────────────

      <DataGrid x:Name = "DgCart"
                AutoGenerateColumns = "False"
                ButtonBase.Click = "DgCart_CellButtonClick" >
          < DataGrid.Columns >
              < DataGridTextColumn  Header = "Product"   Binding = "{Binding Name}"            Width = "*" />
              < DataGridTextColumn  Header = "Unit Price" Binding = "{Binding UnitPriceDisplay}" Width = "100" />
              < DataGridTemplateColumn Header = "Qty" Width = "90" >
                  < DataGridTemplateColumn.CellTemplate >
                      < DataTemplate >
                          < StackPanel Orientation = "Horizontal" >
                              < Button Tag = "Decrement" Content = "−" Width = "24" />
                              < TextBlock Text = "{Binding Quantity}" VerticalAlignment = "Center" Margin = "4,0" />
                              < Button Tag = "Increment" Content = "+" Width = "24" />
                          </ StackPanel >
                      </ DataTemplate >
                  </ DataGridTemplateColumn.CellTemplate >
              </ DataGridTemplateColumn >
              < DataGridTextColumn  Header = "Subtotal"  Binding = "{Binding SubtotalDisplay}" Width = "110" />
              < DataGridTemplateColumn Header = "" Width = "30" >
                  < DataGridTemplateColumn.CellTemplate >
                      < DataTemplate >
                          < Button Tag = "Remove" Content = "✕" />
                      </ DataTemplate >
                  </ DataGridTemplateColumn.CellTemplate >
              </ DataGridTemplateColumn >
          </ DataGrid.Columns >
      </ DataGrid >

  ── Totals Panel ─────────────────────────────────────────────────────

      <Border x:Name = "POS_CartPanelBorder" BorderThickness = "2" CornerRadius = "8" >
          < StackPanel Margin = "16" >
              < TextBlock x: Name = "POS_ItemCountLabel" />
              < TextBlock Text = "Subtotal" />< TextBlock x: Name = "POS_SubtotalLabel" />
              < TextBlock Text = "Discount" />< TextBlock x: Name = "POS_DiscountLabel" />
              < TextBlock Text = "VAT 12%" />< TextBlock x: Name = "POS_VATLabel" />
              < TextBlock Text = "TOTAL" />< TextBlock x: Name = "POS_GrandTotalLabel" FontSize = "20" FontWeight = "Bold" />

              < TextBox x: Name = "POS_DiscountTextBox" PlaceholderText = "Discount (₱)" />
              < Button  x: Name = "POS_ApplyDiscountButton" Content = "Apply Discount" />
              < Button  x: Name = "POS_ClearCartButton"     Content = "Clear Cart" />
              < Button  x: Name = "POS_CheckoutButton"      Content = "Checkout →" />
          </ StackPanel >
      </ Border >
*/
