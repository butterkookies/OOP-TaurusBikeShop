/* ============================================================
   PRODUCT.JS — Catalog filters, detail page interactions
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

    // ── Gallery thumbnails ─────────────────────────────────
    var mainImg  = document.getElementById('galleryMain');
    var thumbs   = document.querySelectorAll('.gallery-thumb');

    thumbs.forEach(function (thumb) {
        thumb.addEventListener('click', function () {
            if (mainImg) mainImg.src = thumb.src;
            thumbs.forEach(function (t) { t.classList.remove('active'); });
            thumb.classList.add('active');
        });
    });

    // ── Variant selection ──────────────────────────────────
    var variantBtns  = document.querySelectorAll('.variant-btn');
    var variantInput = document.getElementById('selectedVariantId');
    var priceEl      = document.getElementById('productPrice');
    var stockEl      = document.getElementById('stockIndicator');

    variantBtns.forEach(function (btn) {
        btn.addEventListener('click', function () {
            if (btn.disabled) return;
            variantBtns.forEach(function (b) { b.classList.remove('active'); });
            btn.classList.add('active');

            if (variantInput) variantInput.value = btn.dataset.variantId;

            var price  = btn.dataset.price;
            var stock  = parseInt(btn.dataset.stock, 10);

            if (priceEl && price) priceEl.textContent = formatCurrency(price);

            if (stockEl) {
                var dot   = stockEl.querySelector('.stock-dot');
                var label = stockEl.querySelector('.stock-label');
                dot.className = 'stock-dot';
                if (stock > 5) {
                    dot.classList.add('in-stock');
                    label.textContent = 'In Stock';
                } else if (stock > 0) {
                    dot.classList.add('low-stock');
                    label.textContent = 'Low Stock';
                } else {
                    dot.classList.add('out-of-stock');
                    label.textContent = 'Out of Stock';
                }
            }

            var addBtn = document.getElementById('addToCartBtn');
            if (addBtn) addBtn.disabled = stock === 0;
        });
    });

    // ── Quantity stepper ───────────────────────────────────
    var qtyField = document.getElementById('qtyInput');
    var qtyInc   = document.getElementById('qtyInc');
    var qtyDec   = document.getElementById('qtyDec');

    if (qtyInc && qtyField) {
        qtyInc.addEventListener('click', function () {
            var max = parseInt(qtyField.max, 10) || 99;
            var val = parseInt(qtyField.value, 10) || 1;
            if (val < max) qtyField.value = val + 1;
        });
    }
    if (qtyDec && qtyField) {
        qtyDec.addEventListener('click', function () {
            var val = parseInt(qtyField.value, 10) || 1;
            if (val > 1) qtyField.value = val - 1;
        });
    }

    // ── Add to cart ────────────────────────────────────────
    var addToCartBtn = document.getElementById('addToCartBtn');
    if (addToCartBtn) {
        addToCartBtn.addEventListener('click', function () {
            var variantId = variantInput ? variantInput.value : null;
            var qty       = qtyField ? parseInt(qtyField.value, 10) : 1;

            if (!variantId) {
                showAlert('#productMessages', 'Please select a variant.', 'warning');
                return;
            }

            showSpinner(addToCartBtn);

            fetchWithCSRF('/Cart/AddItem', {
                productVariantId: parseInt(variantId, 10),
                quantity: qty
            }).then(function (res) {
                if (res.success) {
                    showAlert('#productMessages', 'Added to cart!', 'success');
                    if (res.data && res.data.cartCount !== undefined)
                        updateCartBadge(res.data.cartCount);
                } else {
                    showAlert('#productMessages', res.message || 'Failed to add item.', 'danger');
                }
            }).catch(function () {
                showAlert('#productMessages', 'Something went wrong. Please try again.', 'danger');
            }).finally(function () {
                hideSpinner(addToCartBtn);
            });
        });
    }

    // ── Wishlist toggle ────────────────────────────────────
    document.querySelectorAll('.wishlist-btn').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            var productId = btn.dataset.productId;

            fetchWithCSRF('/Wishlist/Toggle', { productId: parseInt(productId, 10) })
                .then(function (res) {
                    if (res.success) {
                        btn.classList.toggle('active', res.data && res.data.isWishlisted);
                    }
                }).catch(function () {});
        });
    });

    // ── Catalog search ─────────────────────────────────────
    var searchForm  = document.getElementById('catalogSearchForm');
    var searchInput = document.getElementById('catalogSearchInput');

    if (searchForm && searchInput) {
        var searchTimeout;
        searchInput.addEventListener('input', function () {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(function () {
                searchForm.submit();
            }, 600);
        });
    }

    // ── Star rating input (review form) ───────────────────
    var stars = document.querySelectorAll('.star-rating-input .star');
    var ratingInput = document.getElementById('ratingInput');

    stars.forEach(function (star, i) {
        star.addEventListener('click', function () {
            if (ratingInput) ratingInput.value = i + 1;
            stars.forEach(function (s, j) {
                s.classList.toggle('filled', j <= i);
            });
        });
        star.addEventListener('mouseenter', function () {
            stars.forEach(function (s, j) {
                s.style.color = j <= i ? '#F59E0B' : '';
            });
        });
    });

    var ratingWrap = document.querySelector('.star-rating-input');
    if (ratingWrap) {
        ratingWrap.addEventListener('mouseleave', function () {
            var val = ratingInput ? parseInt(ratingInput.value, 10) : 0;
            stars.forEach(function (s, j) {
                s.style.color = j < val ? '#F59E0B' : '';
            });
        });
    }
});
