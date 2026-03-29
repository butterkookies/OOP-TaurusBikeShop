// WebApplication/wwwroot/js/wishlist.js
// Handles remove and move-to-cart on the Wishlist page.
// Depends on: utils.js

'use strict';

document.addEventListener('DOMContentLoaded', () => {

    // =========================================================================
    // REMOVE FROM WISHLIST
    // =========================================================================
    document.addEventListener('click', async e => {
        const removeBtn = e.target.closest('.tbs-wishlist-card__remove-btn');
        if (!removeBtn) return;

        const productId = removeBtn.dataset.productId;
        const cardCol   = document.getElementById(`wishlist-card-${productId}`);

        if (cardCol) {
            const card = cardCol.querySelector('.tbs-wishlist-card');
            card?.classList.add('tbs-wishlist-card--removing');
            await sleep(260);
        }

        try {
            const data = await fetchWithCSRF('/Wishlist/Remove',
                { productId: parseInt(productId, 10) });

            if (!data.success) {
                showAlert('error', data.message ?? 'Could not remove item.');
                cardCol?.querySelector('.tbs-wishlist-card')
                    ?.classList.remove('tbs-wishlist-card--removing');
                return;
            }

            cardCol?.remove();
            updateWishlistCount();

            // Show empty state if grid is now empty
            const grid = document.getElementById('wishlist-grid');
            if (grid && grid.children.length === 0) {
                showEmptyState();
            }

        } catch {
            showAlert('error', 'Could not remove item. Please try again.');
            cardCol?.querySelector('.tbs-wishlist-card')
                ?.classList.remove('tbs-wishlist-card--removing');
        }
    });

    // =========================================================================
    // MOVE TO CART
    // =========================================================================
    document.addEventListener('click', async e => {
        const moveBtn = e.target.closest('.tbs-wishlist-card__move-btn');
        if (!moveBtn) return;

        const productId = moveBtn.dataset.productId;
        const cardCol   = document.getElementById(`wishlist-card-${productId}`);

        moveBtn.disabled    = true;
        moveBtn.textContent = 'Moving…';

        try {
            const data = await fetchWithCSRF('/Wishlist/MoveToCart',
                { productId: parseInt(productId, 10) });

            if (!data.success) {
                showAlert('error', data.message ?? 'Could not move to cart.');
                moveBtn.disabled    = false;
                moveBtn.textContent = 'Move to Cart';
                return;
            }

            showAlert('success', 'Moved to cart!');
            refreshCartBadge();

            // Animate card out
            const card = cardCol?.querySelector('.tbs-wishlist-card');
            card?.classList.add('tbs-wishlist-card--removing');
            await sleep(260);
            cardCol?.remove();

            updateWishlistCount();

            if (!document.getElementById('wishlist-grid')?.children.length) {
                showEmptyState();
            }

        } catch {
            showAlert('error', 'Could not move item. Please try again.');
            moveBtn.disabled    = false;
            moveBtn.textContent = 'Move to Cart';
        }
    });

    // =========================================================================
    // Helpers
    // =========================================================================

    function updateWishlistCount() {
        const grid    = document.getElementById('wishlist-grid');
        const count   = grid ? grid.children.length : 0;
        const subtitle= document.querySelector('.tbs-page-header__subtitle');
        if (subtitle) {
            subtitle.textContent = count === 1 ? '1 saved item' : `${count} saved items`;
        }
    }

    function showEmptyState() {
        const container = document.querySelector('.tbs-wishlist-page .container');
        const grid      = document.getElementById('wishlist-grid');
        const header    = document.querySelector('.tbs-page-header');

        grid?.remove();
        header?.querySelector('.tbs-section-link')?.remove();

        const subtitle = header?.querySelector('.tbs-page-header__subtitle');
        if (subtitle) subtitle.remove();

        if (!container) return;

        const empty = document.createElement('div');
        empty.className = 'tbs-wishlist-page__empty';
        empty.innerHTML = `
            <div class="tbs-wishlist-page__empty-icon" aria-hidden="true">❤️</div>
            <h2 class="tbs-wishlist-page__empty-title">Your wishlist is empty</h2>
            <p class="tbs-wishlist-page__empty-text">
                Save products you love by clicking the heart icon on any product.
            </p>
            <a href="/Product/List" class="btn tbs-btn-accent tbs-btn-lg">Browse Products</a>
        `;
        container.appendChild(empty);
    }

    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

});