/* ============================================================
   CART.JS — Cart page quantity updates, remove, voucher
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

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

                    var lineTotalEl = document.getElementById('line-total-' + itemId);
                    if (lineTotalEl && res.data?.formattedLineTotal)
                        lineTotalEl.textContent = res.data.formattedLineTotal;

                    var subtotalEl = document.getElementById('cart-subtotal');
                    if (subtotalEl && res.data?.formattedSubtotal)
                        subtotalEl.textContent = res.data.formattedSubtotal;

                    var totalEl = document.getElementById('cart-total');
                    if (totalEl && res.data?.formattedSubtotal)
                        totalEl.textContent = res.data.formattedSubtotal;

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

                            var subtotalEl = document.getElementById('cart-subtotal');
                            if (subtotalEl && res.data?.formattedSubtotal)
                                subtotalEl.textContent = res.data.formattedSubtotal;

                            var totalEl = document.getElementById('cart-total');
                            if (totalEl && res.data?.formattedSubtotal)
                                totalEl.textContent = res.data.formattedSubtotal;

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
