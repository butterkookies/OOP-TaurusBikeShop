/**
 * cart.js — handles quantity update and remove actions on the Cart page.
 * The cart table is server-rendered via Razor; this script sends AJAX
 * requests to /Cart/Update and /Cart/Remove and refreshes totals.
 */

(function () {
    'use strict';

    function getAntiForgeryToken() {
        const el = document.querySelector('input[name="__RequestVerificationToken"]');
        return el ? el.value : '';
    }

    function formatPHP(amount) {
        return '\u20b1' + Number(amount).toLocaleString('en-PH', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function recalcTotals() {
        let subtotal = 0;
        document.querySelectorAll('[data-line-total]').forEach(el => {
            subtotal += parseFloat(el.dataset.lineTotal) || 0;
        });

        const shipping = subtotal > 0 ? 150 : 0;
        const total = subtotal + shipping;

        const subEl = document.getElementById('subtotal');
        const shipEl = document.getElementById('shipping');
        const totalEl = document.getElementById('total');

        if (subEl) subEl.textContent = formatPHP(subtotal);
        if (shipEl) shipEl.textContent = formatPHP(shipping);
        if (totalEl) totalEl.textContent = formatPHP(total);
    }

    async function updateQuantity(productId, quantity, rowEl, unitPrice) {
        const token = getAntiForgeryToken();
        try {
            const res = await fetch('/Cart/Update', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'RequestVerificationToken': token
                },
                body: 'productId=' + productId + '&quantity=' + quantity
            });

            if (res.ok) {
                const lineTotal = quantity * unitPrice;
                const lineTotalEl = rowEl.querySelector('[data-line-total]');
                if (lineTotalEl) {
                    lineTotalEl.dataset.lineTotal = lineTotal;
                    lineTotalEl.textContent = formatPHP(lineTotal);
                }
                recalcTotals();
            } else {
                console.warn('Cart update failed:', res.status);
            }
        } catch (err) {
            console.error('Cart update error:', err);
        }
    }

    async function removeItem(productId, rowEl) {
        const token = getAntiForgeryToken();
        try {
            const res = await fetch('/Cart/Remove', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'RequestVerificationToken': token
                },
                body: 'productId=' + productId
            });

            if (res.ok) {
                rowEl.remove();
                recalcTotals();

                const tbody = document.getElementById('cart-items');
                if (tbody && tbody.querySelectorAll('tr').length === 0) {
                    const emptyRow = document.createElement('tr');
                    emptyRow.innerHTML = '<td colspan="5" class="text-center py-4 text-muted">Your cart is empty.</td>';
                    tbody.appendChild(emptyRow);
                }
            }
        } catch (err) {
            console.error('Cart remove error:', err);
        }
    }

    function init() {
        const cartItems = document.getElementById('cart-items');
        if (!cartItems) return;

        let debounceTimer;

        // Quantity input change
        cartItems.addEventListener('change', function (e) {
            const input = e.target.closest('.qty-input');
            if (!input) return;

            const row = input.closest('tr');
            const productId = input.dataset.productId;
            const unitPrice = parseFloat(input.dataset.unitPrice) || 0;
            let qty = parseInt(input.value) || 1;

            if (qty < 1) {
                qty = 1;
                input.value = 1;
            }

            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(function () {
                updateQuantity(productId, qty, row, unitPrice);
            }, 400);
        });

        // +/- buttons
        cartItems.addEventListener('click', function (e) {
            const btn = e.target.closest('[data-action]');
            if (!btn) return;

            const row = btn.closest('tr');
            const input = row ? row.querySelector('.qty-input') : null;
            if (!input) return;

            const productId = input.dataset.productId;
            const unitPrice = parseFloat(input.dataset.unitPrice) || 0;
            let qty = parseInt(input.value) || 1;

            if (btn.dataset.action === 'increase') {
                qty += 1;
            } else if (btn.dataset.action === 'decrease') {
                qty = Math.max(1, qty - 1);
            }

            input.value = qty;
            updateQuantity(productId, qty, row, unitPrice);
        });

        // Remove button
        cartItems.addEventListener('click', function (e) {
            const btn = e.target.closest('.remove-btn');
            if (!btn) return;

            const row = btn.closest('tr');
            const productId = btn.dataset.productId;
            if (productId && row) {
                removeItem(productId, row);
            }
        });

        recalcTotals();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
