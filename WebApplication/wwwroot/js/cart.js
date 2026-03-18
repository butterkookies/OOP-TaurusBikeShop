// WebApplication/wwwroot/js/cart.js
// Handles all cart page interactions: quantity updates, item removal,
// subtotal refresh, and empty-cart state transitions.
// Depends on: utils.js

'use strict';

document.addEventListener('DOMContentLoaded', () => {

    // =========================================================================
    // QTY STEPPER — +/- buttons
    // =========================================================================
    document.addEventListener('click', async e => {

        const qtyBtn = e.target.closest('.tbs-cart-item__qty-btn');
        if (!qtyBtn) return;

        const cartItemId = parseInt(qtyBtn.dataset.cartItemId, 10);
        const action     = qtyBtn.dataset.action; // 'increase' | 'decrease'
        const input      = document.querySelector(
            `.tbs-cart-item__qty-input[data-cart-item-id="${cartItemId}"]`);

        if (!input) return;

        let newQty = parseInt(input.value, 10);
        if (action === 'increase') newQty = Math.min(newQty + 1, 99);
        if (action === 'decrease') newQty = Math.max(newQty - 1, 1);

        if (newQty === parseInt(input.value, 10)) return;

        input.value = newQty;
        await updateCartItem(cartItemId, newQty);
    });

    // =========================================================================
    // QTY INPUT — direct typing (debounced)
    // =========================================================================
    document.addEventListener('change', async e => {
        const input = e.target.closest('.tbs-cart-item__qty-input');
        if (!input) return;

        let newQty = parseInt(input.value, 10);
        if (isNaN(newQty) || newQty < 1) {
            newQty = 1;
            input.value = 1;
        }
        if (newQty > 99) {
            newQty = 99;
            input.value = 99;
        }

        const cartItemId = parseInt(input.dataset.cartItemId, 10);
        await updateCartItem(cartItemId, newQty);
    });

    // =========================================================================
    // REMOVE ITEM
    // =========================================================================
    document.addEventListener('click', async e => {
        const removeBtn = e.target.closest('.tbs-cart-item__remove');
        if (!removeBtn) return;

        const cartItemId = parseInt(removeBtn.dataset.cartItemId, 10);
        const itemRow    = document.getElementById(`cart-item-${cartItemId}`);

        // Animate out
        if (itemRow) {
            itemRow.classList.add('tbs-cart-item--removing');
            await sleep(260);
        }

        try {
            const response = await fetchWithCSRF('/Cart/RemoveFromCart', {
                method: 'POST',
                body: JSON.stringify({ cartItemId })
            });
            const data = await parseJsonResponse(response);

            if (!data.success) {
                showAlert('error', data.message ?? 'Could not remove item.');
                itemRow?.classList.remove('tbs-cart-item--removing');
                return;
            }

            // Remove from DOM
            itemRow?.remove();

            // Update totals
            updateSummaryDisplay(
                data.data.formattedSubtotal,
                data.data.formattedSubtotal,
                data.data.cartCount);

            refreshCartBadge();

            // Show empty state if needed
            if (data.data.isEmpty) {
                showEmptyCart();
            }

        } catch {
            showAlert('error', 'Could not remove item. Please try again.');
            itemRow?.classList.remove('tbs-cart-item--removing');
        }
    });

    // =========================================================================
    // Helper: call UpdateQuantity endpoint
    // =========================================================================
    async function updateCartItem(cartItemId, newQty) {
        const lineTotalEl = document.getElementById(`line-total-${cartItemId}`);

        // Optimistic loading state
        if (lineTotalEl) lineTotalEl.style.opacity = '0.4';

        try {
            const response = await fetchWithCSRF('/Cart/UpdateQuantity', {
                method: 'POST',
                body: JSON.stringify({ cartItemId, qty: newQty })
            });
            const data = await parseJsonResponse(response);

            if (!data.success) {
                showAlert('error', data.message ?? 'Could not update quantity.');
                return;
            }

            // Update line total in DOM
            if (lineTotalEl) {
                lineTotalEl.textContent = data.data.formattedLineTotal;
            }

            // Update summary totals
            updateSummaryDisplay(
                data.data.formattedSubtotal,
                data.data.formattedSubtotal,
                data.data.cartCount);

            refreshCartBadge();

        } catch {
            showAlert('error', 'Could not update quantity. Please try again.');
        } finally {
            if (lineTotalEl) lineTotalEl.style.opacity = '1';
        }
    }

    // =========================================================================
    // Helper: update order summary totals
    // =========================================================================
    function updateSummaryDisplay(subtotal, grandTotal, cartCount) {
        const subtotalEl = document.getElementById('cart-subtotal');
        const totalEl    = document.getElementById('cart-total');

        if (subtotalEl) subtotalEl.textContent = subtotal;
        if (totalEl)    totalEl.textContent    = grandTotal;

        // Update item count in page header
        const countEl = document.querySelector('.tbs-cart-page__count');
        if (countEl) {
            countEl.textContent = cartCount === 1
                ? '1 item'
                : `${cartCount} items`;
        }
    }

    // =========================================================================
    // Helper: swap to empty cart state without full page reload
    // =========================================================================
    function showEmptyCart() {
        const layout = document.querySelector('.tbs-cart-layout');
        const page   = document.querySelector('.tbs-cart-page .container');

        if (!layout || !page) {
            location.reload();
            return;
        }

        layout.remove();

        const emptyEl = document.createElement('div');
        emptyEl.className = 'tbs-cart-page__empty';
        emptyEl.innerHTML = `
            <div class="tbs-cart-page__empty-icon" aria-hidden="true">🛒</div>
            <h2 class="tbs-cart-page__empty-title">Your cart is empty</h2>
            <p class="tbs-cart-page__empty-text">Browse our catalog and add items to get started.</p>
            <a href="/Product/List" class="btn tbs-btn-accent tbs-btn-lg">Shop Now</a>
        `;

        // Remove the page header count
        document.querySelector('.tbs-cart-page__count')?.remove();

        page.appendChild(emptyEl);
    }

    // =========================================================================
    // Utility: await a short delay
    // =========================================================================
    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

});