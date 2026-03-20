/* ============================================================
   ORDER.JS — Order history filters, cancel order
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

    // ── Status filter tabs ─────────────────────────────────
    document.querySelectorAll('[data-order-filter]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var filter = btn.dataset.orderFilter;
            var url    = new URL(window.location.href);
            if (filter) url.searchParams.set('status', filter);
            else url.searchParams.delete('status');
            window.location.href = url.toString();
        });
    });

    // ── Cancel order ───────────────────────────────────────
    document.querySelectorAll('[data-cancel-order]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var orderId = btn.dataset.cancelOrder;
            confirmAction(
                'Are you sure you want to cancel this order? This cannot be undone.',
                function () {
                    showSpinner(btn);
                    fetchWithCSRF('/Order/Cancel', { orderId: parseInt(orderId, 10) })
                        .then(function (res) {
                            if (res.success) window.location.reload();
                            else {
                                hideSpinner(btn);
                                showAlert('#orderMessages', res.message || 'Cancel failed.', 'danger');
                            }
                        }).catch(function () {
                            hideSpinner(btn);
                            showAlert('#orderMessages', 'Something went wrong.', 'danger');
                        });
                }
            );
        });
    });
});
