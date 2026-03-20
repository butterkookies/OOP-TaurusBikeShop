/* ============================================================
   CART.JS — Cart page quantity updates, remove, voucher
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

    // ── Quantity buttons ───────────────────────────────────
    document.querySelectorAll('.cart-qty-btn').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var itemId   = btn.dataset.itemId;
            var action   = btn.dataset.action; // 'inc' or 'dec'
            var qtyEl    = document.getElementById('qty-' + itemId);
            var current  = parseInt(qtyEl ? qtyEl.textContent : 1, 10);
            var newQty   = action === 'inc' ? current + 1 : Math.max(1, current - 1);
            if (newQty === current) return;

            fetchWithCSRF('/Cart/UpdateQuantity', {
                cartItemId: parseInt(itemId, 10),
                quantity:   newQty
            }).then(function (res) {
                if (res.success) window.location.reload();
                else showAlert('#cartMessages', res.message || 'Update failed.', 'danger');
            }).catch(function () {
                showAlert('#cartMessages', 'Something went wrong.', 'danger');
            });
        });
    });

    // ── Remove item ────────────────────────────────────────
    document.querySelectorAll('.cart-remove').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var itemId = btn.dataset.itemId;
            confirmAction('Remove this item from your cart?', function () {
                fetchWithCSRF('/Cart/Remove', { cartItemId: parseInt(itemId, 10) })
                    .then(function (res) {
                        if (res.success) window.location.reload();
                        else showAlert('#cartMessages', res.message || 'Failed.', 'danger');
                    }).catch(function () {
                        showAlert('#cartMessages', 'Something went wrong.', 'danger');
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
                        showAlert('#cartMessages', 'Voucher applied!', 'success');
                        window.location.reload();
                    } else {
                        showAlert('#cartMessages', res.message || 'Invalid voucher.', 'danger');
                    }
                }).catch(function () {
                    showAlert('#cartMessages', 'Something went wrong.', 'danger');
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
