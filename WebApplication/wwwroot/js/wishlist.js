// WebApplication/wwwroot/js/wishlist.js
// Handles remove and add-to-cart on the Wishlist page.
// Depends on: utils.js, site.js

'use strict';

document.addEventListener('DOMContentLoaded', () => {

    // =========================================================================
    // REMOVE FROM WISHLIST
    // =========================================================================
    document.addEventListener('click', async e => {
        const removeBtn = e.target.closest('.wishlist-remove-btn');
        if (!removeBtn) return;

        e.stopPropagation();
        e.preventDefault();

        const productId = removeBtn.dataset.productId;
        const card = removeBtn.closest('.cal-card');

        try {
            const data = await fetchWithCSRF('/Wishlist/Remove',
                { productId: parseInt(productId, 10) });

            if (!data.success) {
                showToast('error', data.message ?? 'Could not remove item.');
                return;
            }

            // Animate card out and remove
            if (card) {
                card.style.transition = 'opacity 0.25s, transform 0.25s';
                card.style.opacity = '0';
                card.style.transform = 'scale(0.95)';
                await sleep(260);
                card.remove();
            }

            updateWishlistCount();

            // Show empty state if grid is now empty
            const grid = document.querySelector('[data-reveal="delay-1"]');
            if (grid && grid.children.length === 0) {
                window.location.reload();
            }

            showToast('success', 'Removed from wishlist.');

        } catch {
            showToast('error', 'Could not remove item. Please try again.');
        }
    });

    // =========================================================================
    // ADD TO CART FROM WISHLIST
    // =========================================================================
    document.addEventListener('click', async e => {
        const cartBtn = e.target.closest('.wishlist-add-to-cart');
        if (!cartBtn) return;

        e.stopPropagation();
        e.preventDefault();

        const productId = cartBtn.dataset.productId;

        cartBtn.disabled = true;
        cartBtn.textContent = 'Adding…';

        try {
            const data = await fetchWithCSRF('/Cart/AddToCart',
                { productId: parseInt(productId, 10), qty: 1 });

            if (data.success) {
                const count = data.data?.cartCount ?? '';
                showToast('success', count
                    ? `Added to cart! (${count} item${count !== 1 ? 's' : ''} in cart)`
                    : 'Added to cart!');
                refreshCartBadge();
            } else {
                showToast('error', data.message ?? 'Could not add to cart.');
            }
        } catch {
            showToast('error', 'Could not add to cart. Please try again.');
        } finally {
            cartBtn.disabled = false;
            cartBtn.textContent = 'Add to Cart';
        }
    });

    // =========================================================================
    // Helpers
    // =========================================================================

    function updateWishlistCount() {
        const grid = document.querySelector('[data-reveal="delay-1"]');
        const count = grid ? grid.children.length : 0;
        const subtitle = document.querySelector('.mb-10 .text-base');
        if (subtitle) {
            subtitle.textContent = count === 1 ? '1 item saved' : `${count} items saved`;
        }
    }

    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

});
