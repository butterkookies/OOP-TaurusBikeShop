# AdminSystem — Redesign Prompt
> Copy this entire document and paste it at the top of every message when asking an AI tool
> to implement or redesign a file. Then attach the specific file(s) you want regenerated.
> The AI will use this as its complete working context — no other briefing is needed.

---

## Who You Are and What You Are Doing

You are an expert WPF / C# developer implementing a full frontend redesign of a desktop
admin application called **Taurus Bike Shop — AdminSystem**.

The application is a WPF desktop app built on **.NET Framework 4.8** using the **MVVM pattern**.
It connects to a **SQL Server** database via **Dapper** and uses **BCrypt** for password hashing.

You have been given three things:
1. **This prompt** — your complete rules and reference.
2. **`redesign-structure.md`** — the source of truth for what every page must look like.
   Read it in full before writing any XAML.
3. **The specific file(s) to regenerate**, attached after this prompt.

Your job: regenerate the attached file(s) so they match `redesign-structure.md` exactly,
following every rule below.

---

## Project Architecture

### Tech stack
- WPF, .NET Framework 4.8, C#
- MVVM pattern (no binding framework — plain INotifyPropertyChanged + RelayCommand)
- SQL Server (local) via Dapper
- BCrypt.Net-Next for password hashing
- No third-party UI libraries (no MahApps, no MaterialDesign, no Syncfusion)

### Folder layout
```
AdminSystem/
├── App.xaml / App.xaml.cs               ← do not touch
├── AppStyles.xaml                        ← ONLY style source; never modify unless adding a token
├── AdminSystem.csproj
│
├── Views/
│   ├── MainWindow.xaml / .cs             ← Shell: sidebar + content frame
│   ├── Login.xaml / .cs
│   ├── Dashboard.xaml / .cs
│   ├── Orders.xaml / .cs
│   ├── POS.xaml / .cs
│   ├── Products.xaml / .cs
│   ├── CreateProductView.xaml / .cs      ← NEW (does not exist yet)
│   ├── Inventory.xaml / .cs
│   ├── Delivery.xaml / .cs
│   ├── Reports.xaml / .cs
│   ├── Vouchers.xaml / .cs
│   ├── Users.xaml / .cs
│   └── Support.xaml / .cs
│
├── ViewModels/
│   ├── BaseViewModel.cs
│   ├── DashboardViewModel.cs
│   ├── OrderViewModel.cs
│   ├── POSViewModel.cs
│   ├── ProductViewModel.cs               ← handles Products list + CreateProduct
│   ├── InventoryViewModel.cs
│   ├── DeliveryViewModel.cs
│   ├── ReportViewModel.cs
│   ├── VoucherViewModel.cs
│   ├── UsersViewModel.cs
│   └── SupportViewModel.cs
│
├── Models/
│   ├── Product.cs         → ProductId, CategoryId, BrandId, Name, Description, Price,
│   │                         IsActive, IsFeatured, CreatedAt, UpdatedAt,
│   │                         CategoryName, BrandName, Variants(List), Images(List)
│   ├── ProductImage.cs    → ProductImageId, ProductId, ImageUrl, StorageBucket,
│   │                         StoragePath, IsPrimary, SortOrder, CreatedAt
│   ├── Category.cs        → CategoryId, Name
│   ├── Brand.cs           → BrandId, BrandName
│   └── (other models)
│
├── Services/
│   └── IProductService.cs → CreateProduct(Product):int, UpdateProduct, DeactivateProduct,
│                            AddProductImage(ProductImage), DeleteProductImage(int),
│                            GetAllCategories, GetAllBrands, GetAllProducts,
│                            SearchProducts(string), GetProductById(int),
│                            AddVariant, UpdateVariant
│
├── Helpers/
│   ├── NavigationHelper.cs  ← handles page switching in MainWindow
│   └── PageNames.cs         ← string constants for page names
│
└── Converters/
    ├── BoolToVisibility.cs   ← registered as BoolToVis in resources
    ├── CurrencyConverter.cs
    └── StatusToColor.cs
```

### How navigation works
- `MainWindow.xaml` is the shell. It has a left sidebar and a `ContentControl` that swaps
  `UserControl` instances when a nav button is clicked.
- Every page is a `UserControl` — never a `Window` or `Page`.
- The new `CreateProductView` is navigated to by raising an event / calling a method that
  `MainWindow.xaml.cs` handles, swapping the content frame. The back button reverses this.

### MVVM pattern
- ViewModels use `INotifyPropertyChanged` via `BaseViewModel`.
- Commands use `RelayCommand` (already in codebase).
- Code-behind sets `DataContext` on `Loaded` or in the constructor.
- Do not introduce new NuGet packages or binding frameworks.

---

## AppStyles.xaml — Complete Token Reference

Every `Background`, `Foreground`, `BorderBrush`, font size, spacing, radius, and named
style **must** reference one of these keys. No raw hex. No inline numbers for font sizes.

### Spacing (Thickness resources)
| Key | Computed value |
|-----|----------------|
| `Space2` | 4 |
| `Space4` | 8 |
| `Space6` | 12 |
| `Space8` | 16 |
| `Space12` | 24 |
| `Space16` | 32 |
| `ButtonPadding` | 16,10 |
| `InputPadding` | 12,10 |
| `CardPadding` | 20 |
| `DataGridHeaderPadding` | 16,12 |
| `BadgePadding` | 10,4 |

### Corner radii (CornerRadius resources)
| Key | Value |
|-----|-------|
| `RadiusSM` | 6 |
| `RadiusMD` | 8 |
| `RadiusLG` | 14 |
| `RadiusPill` | 20 |

### Brushes (SolidColorBrush — use these directly)
| Key | Hex | Use for |
|-----|-----|---------|
| `BgBrush` | `#18181A` | Window / page background |
| `SurfaceBrush` | `#232325` | Cards, panels, sidebar |
| `HoverBrush` | `#2E2E30` | Row hover, nav hover |
| `BorderBrush` | `#3F3F42` | Input borders, dividers |
| `TextPrimaryBrush` | `#F9FAFB` | Body text, headings |
| `TextSecondaryBrush` | `#D1D5DB` | Subtitles, secondary labels |
| `TextMutedBrush` | `#9CA3AF` | Hints, timestamps, captions |
| `AccentBrush` | `#8AB4F8` | Focus rings, info highlights |
| `SuccessBrush` | `#81C995` | Delivered, active, verified |
| `WarningBrush` | `#FDE293` | Pending, low stock, on hold |
| `AlertBrush` | `#FFB74D` | Delayed, near-expiry |
| `InfoBrush` | `#C58AF9` | Info badges |
| `ErrorBrush` | `#F28B82` | Errors, cancelled states |

Do NOT reference `Color` resources (e.g., `{StaticResource Gray900}`) directly on
`Foreground` or `Background` — those properties require a `Brush`, not a `Color`.
Always use the `*Brush` key.

### Font family
| Key | Value |
|-----|-------|
| `FontBase` | `Segoe UI, Roboto, Helvetica Neue, sans-serif` |

### Font sizes (Double resources)
| Key | pt |
|-----|----|
| `FontXS` | 11 |
| `FontSM` | 13 |
| `FontMD` | 15 |
| `FontLG` | 18 |
| `FontXL` | 22 |
| `FontXXL` | 28 |

### Named element styles
| Key | TargetType | Description |
|-----|------------|-------------|
| `TextTitle` | TextBlock | FontXXL, SemiBold, TextPrimaryBrush |
| `TextSection` | TextBlock | FontLG, Medium, TextPrimaryBrush |
| `TextBody` | TextBlock | FontMD, TextPrimaryBrush |
| `TextSubtitle` | TextBlock | FontSM, TextSecondaryBrush |
| `TextCaption` | TextBlock | FontXS, TextMutedBrush |
| `Card` | Border | SurfaceBrush bg, RadiusLG, CardPadding |
| `BaseButton` | Button | font, padding, cursor — base for other buttons |
| `PrimaryButton` | Button | AccentBrush (blue) bg — use for non-branded CTAs |
| `SecondaryButton` | Button | Transparent bg, BorderBrush border |
| `TextBoxStyle` | TextBox | BgBrush bg, BorderBrush border, RadiusMD, focus → AccentBrush |
| `DataGridStyle` | DataGrid | Transparent bg, no border, 48px row height |

### TaurusRed — brand red (MainWindow.Resources local tokens)
These are defined in `MainWindow.xaml`'s `Window.Resources`, NOT in AppStyles.
Because all UserControls are hosted inside MainWindow, they inherit its resource scope,
so these keys ARE accessible from any view:

| Key | Value | Use for |
|-----|-------|---------|
| `TaurusRedBrush` | `#CC0000` | Primary CTA buttons in views (Save, Checkout, Approve) |
| `TaurusRedHoverBrush` | `#E50000` | Hover state |
| `TaurusRedDarkBrush` | `#990000` | Pressed state |
| `TaurusPrimaryButton` | Button style | Shell-level red button |
| `NavButtonStyle` | Button style | Inactive sidebar nav item |
| `NavButtonActiveStyle` | Button style | Active sidebar nav item (red left border) |

Use `TaurusRedBrush` as the `Background` for primary CTA buttons across all views.
Use `ErrorBrush` (from AppStyles) for error text and destructive action borders — not TaurusRed.

---

## Hard Rules — Violations Will Break the Build or Break UX

### Rule 1 — No hardcoded colors
Every `Foreground`, `Background`, `BorderBrush`, `Fill`, `Stroke` must use a StaticResource key.
`Transparent`, `White`, and `Black` are the only literal color values permitted.

### Rule 2 — No hardcoded font sizes
Use `{StaticResource FontXS/SM/MD/LG/XL/XXL}` always.

### Rule 3 — No hardcoded spacing
Use Space* or component padding tokens. For small icon gaps (≤6px) where no token maps
cleanly, a literal like `Margin="0,0,6,0"` is permitted as a narrow exception.

### Rule 4 — No Color on Brush properties
`Foreground` and `Background` require a Brush. Never write:
```xml
Foreground="{StaticResource Gray100}"  <!-- WRONG — Color, not Brush -->
```
Write:
```xml
Foreground="{StaticResource TextPrimaryBrush}"  <!-- CORRECT -->
```

### Rule 5 — Never duplicate DataGrid styles
Define `DataGridColumnHeaderStyle` and `DataGridCellStyle` once per view in
`<UserControl.Resources>`. Do not copy-paste style blocks to each DataGrid in the same file.

### Rule 6 — Preserve every x:Name
Code-behind references these by name. Renaming or removing any `x:Name` will cause
compile errors. If you restructure the layout, the named element must still exist somewhere.

### Rule 7 — Preserve every event handler attribute
`Click`, `SelectionChanged`, `TextChanged`, `Loaded`, `Closing`, etc. must remain on
exactly the same named elements. If you move an element, move its handler attribute with it.

### Rule 8 — Do not touch code-behind unless asked
If your XAML change logically requires a code-behind change, list it in a note after
your output — do not silently make the code-behind incompatible.

### Rule 9 — UserControl background
```xml
<UserControl ... Background="{StaticResource BgBrush}">
```
Never set background on a child Grid. Set it on the UserControl root element.

### Rule 10 — TextBox style
Every TextBox must use `Style="{StaticResource TextBoxStyle}"`.
Never manually set Background, BorderBrush, BorderThickness on a TextBox.

### Rule 11 — Card style
Use `Style="{StaticResource Card}"` on every surface container Border.
Do not manually combine Background + CornerRadius + Padding when the Card style covers them.

### Rule 12 — Empty states
Every DataGrid that can show zero rows needs a sibling TextBlock:
```xml
<TextBlock x:Name="TbEmptyOrders"
           Text="No orders found."
           Style="{StaticResource TextCaption}"
           HorizontalAlignment="Center"
           Margin="{StaticResource Space8}"
           Visibility="Collapsed"/>
```
Name it `TbEmpty[Context]`. Toggle from code-behind.

### Rule 13 — Standard error banner
Use this exact pattern for inline error messages:
```xml
<Border x:Name="BdrError"
        Background="#2A0E0E"
        BorderBrush="#5A1A1A"
        BorderThickness="1"
        CornerRadius="{StaticResource RadiusSM}"
        Padding="{StaticResource Space6}"
        Visibility="Collapsed">
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="&#xE814;"
                   FontFamily="Segoe MDL2 Assets"
                   Foreground="{StaticResource ErrorBrush}"
                   Margin="0,0,8,0"
                   VerticalAlignment="Center"/>
        <TextBlock x:Name="TbError"
                   Style="{StaticResource TextSubtitle}"
                   Foreground="{StaticResource ErrorBrush}"
                   TextWrapping="Wrap"/>
    </StackPanel>
</Border>
```
The two hardcoded hex values in the error banner background/border are intentional —
they are used only here and have no AppStyles token equivalent.

### Rule 14 — Standard success feedback
```xml
<TextBlock x:Name="TbSuccess"
           Style="{StaticResource TextCaption}"
           Foreground="{StaticResource SuccessBrush}"
           Visibility="Collapsed"/>
```

### Rule 15 — Button intent mapping
| Intent | Style / approach |
|--------|-----------------|
| Primary CTA (Save, Checkout, Approve, Create) | `Background="{StaticResource TaurusRedBrush}"` with TaurusPrimaryButton template or equivalent |
| Secondary / ghost (Refresh, Back, Clear) | `Style="{StaticResource SecondaryButton}"` |
| Destructive confirm (Cancel Order, Deactivate User) | `BorderBrush="{StaticResource ErrorBrush}"` + `Background="Transparent"` + `Foreground="{StaticResource ErrorBrush}"` |
| Nav buttons (MainWindow only) | `NavButtonStyle` / `NavButtonActiveStyle` |

### Rule 16 — ComboBox inline styling
No shared ComboBox style exists yet. Use:
```xml
Background="{StaticResource SurfaceBrush}"
Foreground="{StaticResource TextPrimaryBrush}"
BorderBrush="{StaticResource BorderBrush}"
BorderThickness="1"
```

### Rule 17 — Segoe MDL2 icons
Use `FontFamily="Segoe MDL2 Assets"` on a TextBlock.
Do not wrap in a key — no font-family token exists for icon fonts.
Common glyphs:
```
&#xE72C; → Refresh       &#xE71E; → Search       &#xE8F1; → Add / Plus
&#xE711; → Close / X     &#xE73E; → Checkmark     &#xE74D; → Delete / Trash
&#xE70F; → Edit / Pencil &#xE8B5; → Save          &#xE76B; → Back arrow
&#xE814; → Warning / !   &#xE7BF; → Cart          &#xEA37; → Box / Inventory
&#xE8D6; → Image         &#xE723; → Star          &#xE77B; → Person / User
```

---

## Standard Layout Patterns

### Page with scrolling content
```xml
<UserControl ... Background="{StaticResource BgBrush}">
    <ScrollViewer VerticalScrollBarVisibility="Auto">
        <StackPanel Margin="{StaticResource Space12}">
            <!-- PAGE HEADER -->
            <Grid Margin="0,0,0,16">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="Auto"/>
                </Grid.ColumnDefinitions>
                <StackPanel>
                    <TextBlock Text="Page Title" Style="{StaticResource TextTitle}"/>
                    <TextBlock x:Name="TbSubtitle" Style="{StaticResource TextSubtitle}"/>
                </StackPanel>
                <StackPanel Grid.Column="1" Orientation="Horizontal" VerticalAlignment="Center">
                    <!-- action buttons -->
                </StackPanel>
            </Grid>
            <!-- ERROR BANNER -->
            <Border x:Name="BdrError" .../>
            <!-- CONTENT -->
        </StackPanel>
    </ScrollViewer>
</UserControl>
```

### Full-height split-panel page (Orders, Support, Users, etc.)
```xml
<UserControl ... Background="{StaticResource BgBrush}">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>  <!-- header + filters -->
            <RowDefinition Height="Auto"/>  <!-- error bar -->
            <RowDefinition Height="*"/>     <!-- split content -->
        </Grid.RowDefinitions>

        <!-- header row -->
        <Grid Grid.Row="0" Margin="{StaticResource Space8}">...</Grid>

        <!-- error bar -->
        <Border x:Name="BdrError" Grid.Row="1" .../>

        <!-- split content -->
        <Grid Grid.Row="2" Margin="{StaticResource Space8}">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="280"/>
            </Grid.ColumnDefinitions>

            <!-- left: data grid -->
            <Border Grid.Column="0" Style="{StaticResource Card}" Margin="0,0,8,0">
                <DataGrid x:Name="DgData" .../>
            </Border>

            <!-- right: detail/action panel -->
            <Border Grid.Column="1" Style="{StaticResource Card}">
                <ScrollViewer VerticalScrollBarVisibility="Auto">
                    <StackPanel>...</StackPanel>
                </ScrollViewer>
            </Border>
        </Grid>
    </Grid>
</UserControl>
```

---

## New File Spec: CreateProductView.xaml

This file does not exist. Create it when asked.

### Root declaration
```xml
<UserControl x:Class="AdminSystem.Views.CreateProductView"
             xmlns="..."
             xmlns:x="..."
             Background="{StaticResource BgBrush}"
             Loaded="CreateProductView_Loaded">
```

### Two-column layout
- Left column (`3*`): form fields
- Right column (`2*`): image management
- Outer `ScrollViewer` so the full form is accessible on small screens

### Left column — form fields (top to bottom)
1. Back navigation row:
   ```xml
   <Button x:Name="BtnBack" Content="&#xE76B;  Back to Products"
           Style="{StaticResource SecondaryButton}"
           Click="BtnBack_Click"/>
   ```
2. Page title: `x:Name="TbPageTitle"`, Style=TextTitle, Text="Create New Product"
3. Subtitle: Style=TextSubtitle, Text="Changes reflect on the website immediately after saving."
4. Section divider
5. `NAME *` label (TextCaption) + TextBox `x:Name="TbProductName"` Style=TextBoxStyle
6. 2-col grid: `CATEGORY *` ComboBox `x:Name="CbCategory"` | `BRAND` ComboBox `x:Name="CbBrand"`
   - CbCategory: `DisplayMemberPath="Name"` `SelectedValuePath="CategoryId"`
   - CbBrand: `DisplayMemberPath="BrandName"` `SelectedValuePath="BrandId"`
7. 2-col grid: `PRICE (₱) *` TextBox `x:Name="TbProductPrice"` | CheckBox `x:Name="ChkFeatured"` Content="Mark as Featured"
8. `DESCRIPTION` label + TextBox `x:Name="TbProductDesc"` Height=80, AcceptsReturn=True, TextWrapping=Wrap
9. `STATUS` label + RadioButton group in a horizontal StackPanel:
   - `x:Name="RbActive"` Content="Active" IsChecked=True
   - `x:Name="RbInactive"` Content="Inactive"
10. Error banner: `x:Name="BdrFormError"` / `x:Name="TbFormError"`
11. Success banner: `x:Name="TbFormSuccess"` Visibility=Collapsed
12. Button row (right-aligned):
    - `[Cancel]` → `x:Name="BtnCancel"` Style=SecondaryButton Click="BtnCancel_Click"
    - `[Save Product]` → `x:Name="BtnSaveProduct"` Background=TaurusRedBrush Click="BtnSaveProduct_Click"

### Right column — image management (top to bottom)
1. Section label: "PRODUCT IMAGE" Style=TextCaption
2. Preview pane: `Border` Height=180, CornerRadius=RadiusMD, BorderBrush=BorderBrush, BorderThickness=1
   - `Image x:Name="ImgPreview"` Stretch=Uniform Visibility=Collapsed
   - `TextBlock x:Name="TbImgPlaceholder"` Text="Image preview will appear here"
     Style=TextCaption HorizontalAlignment=Center VerticalAlignment=Center
3. `IMAGE URL` label (TextCaption) + TextBox `x:Name="TbImageUrl"` Style=TextBoxStyle
4. `[+ Add Image]` Button `x:Name="BtnAddImage"` Style=SecondaryButton Click="BtnAddImage_Click"
5. Image error: `x:Name="TbImageError"` Style=TextCaption Foreground=ErrorBrush Visibility=Collapsed
6. `ADDED IMAGES` label (TextCaption)
7. ListBox `x:Name="LbImages"` MaxHeight=220 Background=Transparent BorderThickness=0
   - ItemTemplate: horizontal row per image
     - Image (40×40, Stretch=UniformToFill, CornerRadius via Border wrapper)
     - TextBlock URL (truncated, TextSubtitle, Width=*)
     - Button `[★ Primary]` Tag="{Binding}" Click="BtnSetPrimary_Click" Style=SecondaryButton
     - Button `[✕]` Tag="{Binding}" Click="BtnRemoveImage_Click" Style=SecondaryButton

### Code-behind stub expected (list these if you also generate the .cs)
```csharp
private void CreateProductView_Loaded(object sender, RoutedEventArgs e)
    // Load categories into CbCategory.ItemsSource
    // Load brands into CbBrand.ItemsSource

private void BtnBack_Click(object sender, RoutedEventArgs e)
    // Navigate back to Products page

private void BtnCancel_Click(object sender, RoutedEventArgs e)
    // Same as BtnBack_Click

private void BtnAddImage_Click(object sender, RoutedEventArgs e)
    // Read TbImageUrl.Text, load BitmapImage for preview,
    // add entry to LbImages.ItemsSource

private void BtnSetPrimary_Click(object sender, RoutedEventArgs e)
    // Mark clicked image as primary, unmark others

private void BtnRemoveImage_Click(object sender, RoutedEventArgs e)
    // Remove clicked image from LbImages.ItemsSource

private void BtnSaveProduct_Click(object sender, RoutedEventArgs e)
    // Validate: Name not empty, Category selected, Price is valid decimal
    // Call IProductService.CreateProduct(product) → get productId
    // Loop images: call IProductService.AddProductImage(image) per entry
    // Show success, navigate back after short delay
```

---

## Per-File Workflow

When you receive a file to redesign:

**Step 1 — Read the file completely.**
List every `x:Name` and every event handler you find. Do not skip this step.

**Step 2 — Find the page in redesign-structure.md.**
Read the ASCII wireframe and element list for that page.
Note the layout pattern, UX fixes, and any special sections.

**Step 3 — Write the XAML.**
Follow the appropriate layout pattern.
Apply all 17 rules.
Match every element described in redesign-structure.md.

**Step 4 — Self-verify before output.**
Run through this checklist:
- [ ] No hardcoded hex colors (except the two allowed in error banner bg/border)
- [ ] No hardcoded font size numbers
- [ ] No hardcoded spacing numbers (except small icon gaps ≤6px)
- [ ] No `Color` resource used directly on `Foreground`/`Background`
- [ ] All original `x:Name` values present
- [ ] All original event handler attributes present
- [ ] DataGrid column header + cell styles defined in `UserControl.Resources`, not per-grid
- [ ] Every TextBox uses `TextBoxStyle`
- [ ] Every surface container Border uses `Card` style (or has equivalent token properties)
- [ ] Every DataGrid has an empty-state TextBlock sibling
- [ ] Error banner follows the standard pattern
- [ ] `Background="{StaticResource BgBrush}"` on the UserControl root

**Step 5 — Output.**
Provide the complete file in one fenced XML code block.
After the block, list any required code-behind changes as a numbered note.
If you changed any `x:Name` or handler attribute, explain why immediately after the block.

---

## What You Must NOT Do

- Add NuGet packages or reference external DLLs
- Create a new ResourceDictionary file
- Convert a UserControl to a Window or Page
- Use `DataContext` bindings (`{Binding ...}`) on new elements unless instructed — existing
  views use direct `x:Name` manipulation from code-behind
- Change `App.xaml` or `App.xaml.cs`
- Rename existing `x:Name` attributes
- Remove existing event handler attributes
- Set background on a child Grid instead of the UserControl
- Regenerate `AppStyles.xaml` unless explicitly asked to add a specific new token
- Regenerate `.csproj` — new files must be added to it manually
- Use `async/await` or `Dispatcher.Invoke` unless the original file already does

---

## Quick Reference — File to redesign-structure.md Section

| File | Section in redesign-structure.md |
|------|----------------------------------|
| `MainWindow.xaml` | App Shell — MainWindow |
| `Login.xaml` | 1. Login |
| `Dashboard.xaml` | 2. Dashboard |
| `Orders.xaml` | 3. Orders |
| `POS.xaml` | 4. POS (Point of Sale) |
| `Products.xaml` | 5. Products (List + Edit) |
| `CreateProductView.xaml` (NEW) | 5a. Create / Edit Product |
| `Inventory.xaml` | 6. Inventory |
| `Delivery.xaml` | 7. Delivery |
| `Reports.xaml` | 8. Reports |
| `Vouchers.xaml` | 9. Vouchers |
| `Users.xaml` | 10. Users |
| `Support.xaml` | 11. Support |
| `AppStyles.xaml` | Design Token Reference — read only |

---

*End of prompt context. The file(s) to redesign are attached below this line.*
