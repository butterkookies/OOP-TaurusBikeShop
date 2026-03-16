// WebApplication/Models/Entities/CartItem.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a single product line item within a shopping cart.
/// <para>
/// <b>Price snapshot:</b> <see cref="PriceAtAdd"/> captures the effective selling
/// price of the selected variant at the exact moment the item was added to the cart.
/// This is <c>Product.Price + ProductVariant.AdditionalPrice</c> at add time.
/// If the product price changes after the item is in the cart, the cart item retains
/// the original price until the customer removes and re-adds the item.
/// </para>
/// <para>
/// <b>Variant rule:</b> <see cref="ProductVariantId"/> should always be set —
/// even for products using the Default variant. NULL is permitted by the schema
/// for legacy compatibility but new cart items should always reference a variant.
/// </para>
/// </summary>
public sealed class CartItem
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int CartItemId { get; set; }

    /// <summary>
    /// FK to the parent cart. Configured with CASCADE DELETE — removing a cart
    /// removes all its items.
    /// </summary>
    public int CartId { get; set; }

    /// <summary>FK to the product being added to the cart.</summary>
    public int ProductId { get; set; }

    /// <summary>
    /// FK to the specific variant of the product being added.
    /// Should always be set — use the Default variant FK for products
    /// with no real variants.
    /// </summary>
    public int? ProductVariantId { get; set; }

    /// <summary>
    /// Quantity of this product variant in the cart.
    /// Must be &gt; 0 — enforced by CK_CartItem_Quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Price snapshot at the time this item was added to the cart.
    /// Equals <c>Product.Price + ProductVariant.AdditionalPrice</c> at add time.
    /// Must be &gt;= 0 — enforced by CK_CartItem_PriceAtAdd.
    /// This value does not change if the product price is updated later.
    /// </summary>
    public decimal PriceAtAdd { get; set; }

    /// <summary>UTC timestamp when this item was added to the cart.</summary>
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The cart this item belongs to.</summary>
    public Cart Cart { get; set; } = null!;

    /// <summary>The product being added.</summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// The specific variant of the product.
    /// NULL only for legacy rows — new items always have a variant.
    /// </summary>
    public ProductVariant? Variant { get; set; }
}