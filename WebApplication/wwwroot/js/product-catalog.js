// WebApplication/wwwroot/js/product-catalog.js
// Handles: catalog filter AJAX, search debounce, wishlist toggle,
// variant selector on product detail page, thumbnail gallery.
// Depends on: utils.js

'use strict';

document.addEventListener('DOMContentLoaded', () => {

    // =========================================================================
    // CATALOG PAGE — filter and search
    // =========================================================================
    const applyBtn   = document.getElementById('apply-filters-btn');
    const searchInput= document.getElementById('search-input');

    if (applyBtn) {
        applyBtn.addEventListener('click', applyFilters);
    }

    if (searchInput) {
        searchInput.addEventListener('keydown', e => {
            if (e.key === 'Enter') applyFilters();
        });

        // Debounced auto-search (300ms)
        searchInput.addEventListener('input', debounce(applyFilters, 300));
    }

    function applyFilters() {
        const params = new URLSearchParams();

        // Category (dropdown or radio fallback)
        const catSelect = document.getElementById('category-select');
        const catRadio  = document.querySelector('input[name="categoryId"]:checked');
        const catVal    = catSelect?.value ?? catRadio?.value;
        if (catVal) params.set('categoryId', catVal);

        // Brand (dropdown or radio fallback)
        const brandSelect = document.getElementById('brand-select');
        const brandRadio  = document.querySelector('input[name="brandId"]:checked');
        const brandVal    = brandSelect?.value ?? brandRadio?.value;
        if (brandVal) params.set('brandId', brandVal);

        // Price
        const minPrice = document.getElementById('min-price')?.value;
        const maxPrice = document.getElementById('max-price')?.value;
        if (minPrice) params.set('minPrice', minPrice);
        if (maxPrice) params.set('maxPrice', maxPrice);

        // Search
        const search = searchInput?.value?.trim();
        if (search) params.set('search', search);

        window.location.href = `/Product/List?${params.toString()}`;
    }

    // =========================================================================
    // WISHLIST TOGGLE — both catalog cards and product detail page
    // =========================================================================
    document.addEventListener('click', async e => {
        const wishlistBtn = e.target.closest('.wishlist-toggle');
        if (!wishlistBtn) return;

        e.stopPropagation();
        e.preventDefault();

        const productId = wishlistBtn.dataset.productId;
        if (!productId) return;

        try {
            const data = await fetchWithCSRF('/Wishlist/Toggle',
                { productId: parseInt(productId, 10) });

            if (!data.success) {
                showToast('error', data.message ?? 'Unable to update wishlist.');
                return;
            }

            // Toggle active state on all wishlist buttons for this product
            document.querySelectorAll(
                `.wishlist-toggle[data-product-id="${productId}"]`
            ).forEach(btn => {
                const isNowInWishlist = data.data?.isInWishlist ?? false;
                btn.classList.toggle('is-active', isNowInWishlist);
                btn.classList.toggle('!text-error', isNowInWishlist);
                btn.setAttribute('aria-pressed', isNowInWishlist ? 'true' : 'false');
                btn.setAttribute('aria-label',
                    isNowInWishlist ? 'Remove from wishlist' : 'Add to wishlist');

                // Swap heart SVG icons
                const filled  = btn.querySelector('.heart-filled');
                const outline = btn.querySelector('.heart-outline');
                if (filled && outline) {
                    filled.classList.toggle('hidden', !isNowInWishlist);
                    outline.classList.toggle('hidden', isNowInWishlist);
                }
            });

            showToast('success', data.message ?? 'Wishlist updated.');

        } catch {
            showToast('error', 'Unable to update wishlist. Please try again.');
        }
    });

    // =========================================================================
    // QUICK ADD TO CART — from catalog product cards
    // =========================================================================
    document.addEventListener('click', async e => {
        const cartBtn = e.target.closest('.catalog-add-to-cart');
        if (!cartBtn) return;

        e.stopPropagation();
        e.preventDefault();

        const productId = cartBtn.dataset.productId;
        if (!productId) return;

        cartBtn.disabled = true;

        try {
            const data = await fetchWithCSRF('/Cart/AddToCart',
                { productId: parseInt(productId, 10), qty: 1 });

            if (data.success) {
                const count = data.data?.cartCount ?? '';
                showToast('success', count ? `Added to cart! (${count} item${count !== 1 ? 's' : ''} in cart)` : 'Added to cart!');
                refreshCartBadge();
            } else {
                showToast('error', data.message ?? 'Could not add to cart.');
            }
        } catch {
            showToast('error', 'Could not add to cart. Please try again.');
        } finally {
            cartBtn.disabled = false;
        }
    });

    // =========================================================================
    // PRODUCT DETAIL PAGE — variant selector
    // =========================================================================
    const variantBtns = document.querySelectorAll('.cal-variant-btn:not([disabled])');
    const addToCartBtn = document.getElementById('add-to-cart-btn');
    const displayedPrice = document.getElementById('displayed-price');

    variantBtns.forEach(btn => {
        btn.addEventListener('click', async () => {
            variantBtns.forEach(b => {
                b.classList.remove('cal-variant-btn--selected');
                b.setAttribute('aria-pressed', 'false');
            });
            btn.classList.add('cal-variant-btn--selected');
            btn.setAttribute('aria-pressed', 'true');

            const variantId = btn.dataset.variantId;
            if (addToCartBtn) addToCartBtn.dataset.variantId = variantId;

            // Update qty max to this variant's stock
            const variantStock = parseInt(btn.dataset.stock || '0', 10);
            const qtyIn = document.getElementById('qty-input');
            if (qtyIn) {
                qtyIn.setAttribute('max', variantStock);
                qtyIn.dataset.maxStock = variantStock;
                if (parseInt(qtyIn.value, 10) > variantStock)
                    qtyIn.value = Math.max(1, variantStock);
            }

            try {
                const response = await fetch(`/Product/GetVariantPrice?variantId=${variantId}`);
                const data = await response.json();
                if (data.success && displayedPrice) {
                    displayedPrice.textContent = data.data.formattedPrice;
                }
            } catch { /* non-fatal */ }
        });
    });

    // =========================================================================
    // PRODUCT DETAIL PAGE — add to cart button
    // =========================================================================
    if (addToCartBtn) {
        addToCartBtn.addEventListener('click', async () => {
            const productId = addToCartBtn.dataset.productId;
            const variantId = addToCartBtn.dataset.variantId;
            const qty = parseInt(document.getElementById('qty-input')?.value ?? '1', 10);

            addToCartBtn.disabled = true;
            addToCartBtn.textContent = 'Adding…';

            try {
                const data = await fetchWithCSRF('/Cart/AddToCart', {
                    productId: parseInt(productId, 10),
                    variantId: variantId ? parseInt(variantId, 10) : null,
                    qty
                });

                if (data.success) {
                    const count = data.data?.cartCount ?? '';
                    showToast('success', count ? `Added to cart! (${count} item${count !== 1 ? 's' : ''} in cart)` : 'Added to cart!');
                    refreshCartBadge();
                } else {
                    showToast('error', data.message ?? 'Could not add to cart.');
                }
            } catch {
                showToast('error', 'Could not add to cart. Please try again.');
            } finally {
                addToCartBtn.disabled = false;
                addToCartBtn.textContent = 'Add to Cart';
            }
        });
    }

    // =========================================================================
    // PRODUCT DETAIL PAGE — quantity stepper
    // =========================================================================
    const qtyInput = document.getElementById('qty-input');
    document.getElementById('qty-minus')?.addEventListener('click', () => {
        if (qtyInput && parseInt(qtyInput.value, 10) > 1)
            qtyInput.value = parseInt(qtyInput.value, 10) - 1;
    });
    document.getElementById('qty-plus')?.addEventListener('click', () => {
        if (!qtyInput) return;
        const maxStock = parseInt(qtyInput.getAttribute('max') || '99', 10);
        if (parseInt(qtyInput.value, 10) < maxStock)
            qtyInput.value = parseInt(qtyInput.value, 10) + 1;
    });
    // Clamp on manual input
    qtyInput?.addEventListener('change', () => {
        const maxStock = parseInt(qtyInput.getAttribute('max') || '99', 10);
        let val = parseInt(qtyInput.value, 10);
        if (isNaN(val) || val < 1) val = 1;
        if (val > maxStock) val = maxStock;
        qtyInput.value = val;
    });

    // =========================================================================
    // PRODUCT DETAIL PAGE — thumbnail gallery
    // =========================================================================
    document.querySelectorAll('.tbs-gallery__thumb').forEach(thumb => {
        thumb.addEventListener('click', () => {
            const mainImg = document.getElementById('main-product-img');
            if (mainImg) mainImg.src = thumb.dataset.src;
            document.querySelectorAll('.tbs-gallery__thumb')
                .forEach(t => t.classList.remove('tbs-gallery__thumb--active'));
            thumb.classList.add('tbs-gallery__thumb--active');
        });
    });

    // =========================================================================
    // PRODUCT DETAIL PAGE — Buy Now button + modal
    // =========================================================================
    const buyNowBtn   = document.getElementById('buy-now-btn');
    const buyNowModal = document.getElementById('buy-now-modal');

    if (buyNowBtn && buyNowModal) {
        const closeModal = () => buyNowModal.classList.add('hidden');

        buyNowBtn.addEventListener('click', async () => {
            const productId = buyNowBtn.dataset.productId;
            const variantId = buyNowBtn.dataset.variantId;
            const qty = parseInt(document.getElementById('qty-input')?.value ?? '1', 10);

            buyNowBtn.disabled = true;
            buyNowBtn.textContent = 'Adding…';

            try {
                const data = await fetchWithCSRF('/Cart/AddToCart', {
                    productId: parseInt(productId, 10),
                    variantId: variantId ? parseInt(variantId, 10) : null,
                    qty
                });

                if (data.success) {
                    refreshCartBadge();
                    buyNowModal.classList.remove('hidden');
                } else {
                    showToast('error', data.message ?? 'Could not add to cart.');
                }
            } catch {
                showToast('error', 'Could not add to cart. Please try again.');
            } finally {
                buyNowBtn.disabled = false;
                buyNowBtn.textContent = 'Buy Now';
            }
        });

        document.getElementById('buy-now-checkout')?.addEventListener('click', () => {
            window.location.href = '/Checkout';
        });

        document.getElementById('buy-now-close')?.addEventListener('click', closeModal);

        buyNowModal.addEventListener('click', e => {
            if (e.target === buyNowModal) closeModal();
        });
    }

    // =========================================================================
    // PRODUCT DETAIL PAGE — tabs
    // =========================================================================
    document.querySelectorAll('.tbs-tab').forEach(tab => {
        tab.addEventListener('click', () => {
            document.querySelectorAll('.tbs-tab').forEach(t => t.classList.remove('tbs-tab--active'));
            document.querySelectorAll('.tbs-tab-panel').forEach(p => p.classList.remove('tbs-tab-panel--active'));
            tab.classList.add('tbs-tab--active');
            const panel = document.getElementById(`tab-${tab.dataset.tab}`);
            if (panel) panel.classList.add('tbs-tab-panel--active');
        });
    });

    // =========================================================================
    // PRODUCT DETAIL PAGE — OTP modal countdown (shared util)
    // =========================================================================
    initOTPCountdown();

});

// =============================================================================
// OTP countdown timer — shared by Register and Payment pages
// =============================================================================
function initOTPCountdown() {
    const countdownEl = document.getElementById('otp-countdown');
    const resendBtn   = document.getElementById('otp-resend-btn');
    if (!countdownEl) return;

    let seconds = 10 * 60; // 10 minutes

    const interval = setInterval(() => {
        seconds--;
        const m = Math.floor(seconds / 60).toString().padStart(2, '0');
        const s = (seconds % 60).toString().padStart(2, '0');
        countdownEl.textContent = `${m}:${s}`;

        if (seconds <= 0) {
            clearInterval(interval);
            countdownEl.textContent = '00:00';
            countdownEl.style.color = 'var(--tbs-error)';
            if (resendBtn) resendBtn.disabled = false;
        }
    }, 1000);

    // Enable resend button after 60 seconds
    setTimeout(() => {
        if (resendBtn) resendBtn.disabled = false;
    }, 60000);

    // Resend handler
    resendBtn?.addEventListener('click', async () => {
        const email = document.querySelector('input[name="email"]')?.value;
        if (!email) return;

        resendBtn.disabled = true;
        resendBtn.textContent = 'Sending…';

        try {
            const response = await fetchWithCSRF('/Customer/ResendOTP', {
                method: 'POST',
                body: JSON.stringify({ email })
            });
            const data = await parseJsonResponse(response);

            if (data.success) {
                showAlert('success', data.message ?? 'New code sent.');
                // Reset countdown
                seconds = 10 * 60;
            } else {
                showAlert('error', data.message ?? 'Failed to resend code.');
                resendBtn.disabled = false;
            }
        } catch {
            showAlert('error', 'Failed to resend code.');
            resendBtn.disabled = false;
        } finally {
            resendBtn.textContent = 'Resend Code';
        }
    });
}