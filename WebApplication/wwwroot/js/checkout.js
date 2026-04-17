/* ============================================================
   CHECKOUT.JS — Delivery selection, address highlight, spinner
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

    // ── Delivery method options ────────────────────────────
    document.querySelectorAll('.tbs-delivery-option').forEach(function (label) {
        label.addEventListener('click', function () {
            document.querySelectorAll('.tbs-delivery-option')
                .forEach(function (l) { l.classList.remove('tbs-delivery-option--selected'); });
            label.classList.add('tbs-delivery-option--selected');
        });
    });

    // ── Pickup details panel toggle ────────────────────────
    var pickupDetails = document.getElementById('pickup-details');
    if (pickupDetails) {
        document.querySelectorAll('input[name="DeliveryMethod"]').forEach(function (radio) {
            radio.addEventListener('change', function () {
                pickupDetails.style.display = radio.value === 'Pickup' && radio.checked ? 'block' : 'none';
            });
        });
    }

    // ── Payment method options ─────────────────────────────
    document.querySelectorAll('.tbs-payment-option').forEach(function (label) {
        label.addEventListener('click', function () {
            document.querySelectorAll('.tbs-payment-option')
                .forEach(function (l) { l.classList.remove('tbs-payment-option--selected'); });
            label.classList.add('tbs-payment-option--selected');
        });
    });

    // ── Address option highlight ───────────────────────────
    document.querySelectorAll('.tbs-address-option').forEach(function (label) {
        label.addEventListener('click', function () {
            document.querySelectorAll('.tbs-address-option')
                .forEach(function (l) { l.classList.remove('tbs-address-option--selected'); });
            label.classList.add('tbs-address-option--selected');
        });
    });

    // ── Form submit — validate then allow submit ───────────
    // IMPORTANT: listen on the FORM submit event, not the button click event.
    // Disabling a submit button inside a click handler cancels form submission
    // in Chromium-based browsers before the request is sent.
    var form = document.getElementById('checkout-form');
    var placeOrderBtn = document.getElementById('place-order-btn');

    if (form) {
        form.addEventListener('submit', function (e) {
            var deliverySelected = form.querySelector('input[name="DeliveryMethod"]:checked');
            if (!deliverySelected) {
                e.preventDefault();
                showToast('error', 'Please select a delivery method.');
                return;
            }

            var paymentSelected = form.querySelector('input[name="PaymentMethod"]:checked');
            if (!paymentSelected) {
                e.preventDefault();
                showToast('error', 'Please select a payment method.');
                return;
            }

            // Form is valid and will submit — show spinner to prevent double-click.
            // Spinner is shown AFTER submission is confirmed (inside submit event,
            // not click event) so the browser has already committed to sending the request.
            if (placeOrderBtn) showSpinner(placeOrderBtn);
        });
    }
});
