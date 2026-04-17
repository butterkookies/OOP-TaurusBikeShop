/* ============================================================
   CART.JS — Cart page quantity updates, remove, voucher
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

    // ── Selection state (client-only, sessionStorage-backed) ─
    var SELECTION_KEY = 'tbs_cart_unchecked';

    function loadUnchecked() {
        try {
            var raw = sessionStorage.getItem(SELECTION_KEY);
            if (!raw) return new Set();
            return new Set(JSON.parse(raw));
        } catch (e) { return new Set(); }
    }

    function saveUnchecked(set) {
        try {
            sessionStorage.setItem(SELECTION_KEY, JSON.stringify(Array.from(set)));
        } catch (e) { /* quota/denied — non-critical */ }
    }

    var uncheckedIds = loadUnchecked();

    function fmtCurrency(n) {
        return '₱' + n.toLocaleString('en-PH', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function recomputeSelectionTotals() {
        var rows = document.querySelectorAll('#cart-items-list [data-cart-item-id]');
        var subtotal = 0;
        var selectedCount = 0;
        var totalRows = rows.length;

        rows.forEach(function (row) {
            var cb = row.querySelector('.cart-item-select');
            if (cb && cb.checked) {
                var qtyInput = row.querySelector('.cal-qty-val');
                var qty = parseInt(qtyInput ? qtyInput.value : row.dataset.quantity, 10) || 0;
                var unit = parseFloat(row.dataset.unitPrice) || 0;
                subtotal += qty * unit;
                selectedCount++;
            }
        });

        var subtotalEl = document.getElementById('cart-subtotal');
        if (subtotalEl) subtotalEl.textContent = fmtCurrency(subtotal);
        var totalEl = document.getElementById('cart-total');
        if (totalEl) totalEl.textContent = fmtCurrency(subtotal);

        var countEl = document.getElementById('cart-selected-count');
        if (countEl) countEl.textContent = selectedCount;

        var selectAll = document.getElementById('cart-select-all');
        if (selectAll) {
            selectAll.checked = selectedCount > 0 && selectedCount === totalRows;
            selectAll.indeterminate = selectedCount > 0 && selectedCount < totalRows;
        }

        var proceedBtn = document.getElementById('proceed-to-checkout-btn');
        if (proceedBtn) proceedBtn.disabled = selectedCount === 0;
    }

    // Restore checkbox state from sessionStorage
    document.querySelectorAll('.cart-item-select').forEach(function (cb) {
        var id = cb.dataset.cartItemId;
        if (uncheckedIds.has(id)) cb.checked = false;
    });

    // Per-item checkbox handler
    document.querySelectorAll('.cart-item-select').forEach(function (cb) {
        cb.addEventListener('change', function () {
            var id = cb.dataset.cartItemId;
            if (cb.checked) uncheckedIds.delete(id);
            else uncheckedIds.add(id);
            saveUnchecked(uncheckedIds);
            recomputeSelectionTotals();
        });
    });

    // Select-all handler
    var selectAllEl = document.getElementById('cart-select-all');
    if (selectAllEl) {
        selectAllEl.addEventListener('change', function () {
            document.querySelectorAll('.cart-item-select').forEach(function (cb) {
                cb.checked = selectAllEl.checked;
                var id = cb.dataset.cartItemId;
                if (cb.checked) uncheckedIds.delete(id);
                else uncheckedIds.add(id);
            });
            saveUnchecked(uncheckedIds);
            recomputeSelectionTotals();
        });
    }

    // Proceed to Checkout handler
    var proceedBtn = document.getElementById('proceed-to-checkout-btn');
    if (proceedBtn) {
        proceedBtn.addEventListener('click', function () {
            var ids = [];
            document.querySelectorAll('.cart-item-select').forEach(function (cb) {
                if (cb.checked) ids.push(cb.dataset.cartItemId);
            });
            if (ids.length === 0) {
                showToast('error', 'Select at least one item to check out.');
                return;
            }
            window.location.href = '/Checkout?ids=' + encodeURIComponent(ids.join(','));
        });
    }

    recomputeSelectionTotals();

    // ── Quantity buttons ───────────────────────────────────
    document.querySelectorAll('.cal-qty-btn').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var itemId  = btn.dataset.cartItemId;
            var action  = btn.dataset.action; // 'increase' or 'decrease'
            var input   = document.querySelector('.cal-qty-val[data-cart-item-id="' + itemId + '"]');
            var current  = parseInt(input ? input.value : 1, 10);
            var maxStock = parseInt(input ? input.getAttribute('max') : 99, 10) || 99;
            var newQty   = action === 'increase' ? Math.min(current + 1, maxStock) : Math.max(1, current - 1);
            if (newQty === current) return;

            btn.disabled = true;

            fetchWithCSRF('/Cart/UpdateQuantity', {
                cartItemId: parseInt(itemId, 10),
                qty:        newQty
            }).then(function (res) {
                if (res.success) {
                    if (input) input.value = newQty;

                    var row = document.getElementById('cart-item-' + itemId);
                    if (row) row.dataset.quantity = newQty;

                    var lineTotalEl = document.getElementById('line-total-' + itemId);
                    if (lineTotalEl && res.data?.formattedLineTotal)
                        lineTotalEl.textContent = res.data.formattedLineTotal;

                    // Recompute subtotal/total from current selection, not from
                    // the server's full-cart total — unselected items are excluded.
                    recomputeSelectionTotals();
                    refreshCartBadge();
                } else {
                    showToast('error', res.message || 'Update failed.');
                }
            }).catch(function () {
                showToast('error', 'Something went wrong.');
            }).finally(function () {
                btn.disabled = false;
            });
        });
    });

    // ── Remove item ────────────────────────────────────────
    document.querySelectorAll('.cart-item-remove').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var itemId = btn.dataset.cartItemId;
            confirmAction('Remove this item from your cart?', function () {
                fetchWithCSRF('/Cart/RemoveFromCart', { cartItemId: parseInt(itemId, 10) })
                    .then(function (res) {
                        if (res.success) {
                            var row = document.getElementById('cart-item-' + itemId);
                            if (row) row.remove();

                            uncheckedIds.delete(itemId);
                            saveUnchecked(uncheckedIds);

                            recomputeSelectionTotals();
                            refreshCartBadge();

                            if (res.data?.isEmpty) window.location.reload();
                        } else {
                            showToast('error', res.message || 'Failed to remove item.');
                        }
                    }).catch(function () {
                        showToast('error', 'Something went wrong.');
                    });
            });
        });
    });

    // ── Apply voucher ──────────────────────────────────────
    var voucherBtn = document.getElementById('applyVoucherBtn');
    if (voucherBtn) {
        voucherBtn.addEventListener('click', function () {
            var code = document.getElementById('voucherInput').value.trim();
            if (!code) return;

            showSpinner(voucherBtn);
            fetchWithCSRF('/Voucher/Apply', { code: code })
                .then(function (res) {
                    if (res.success) {
                        showToast('success', 'Voucher applied!');
                        window.location.reload();
                    } else {
                        showToast('error', res.message || 'Invalid voucher.');
                    }
                }).catch(function () {
                    showToast('error', 'Something went wrong.');
                }).finally(function () {
                    hideSpinner(voucherBtn);
                });
        });
    }

    // ── Remove voucher ────────────────────────────────────
    var removeVoucherBtn = document.getElementById('removeVoucherBtn');
    if (removeVoucherBtn) {
        removeVoucherBtn.addEventListener('click', function () {
            fetchWithCSRF('/Voucher/Remove', {})
                .then(function (res) {
                    if (res.success) window.location.reload();
                }).catch(function () {});
        });
    }
});
