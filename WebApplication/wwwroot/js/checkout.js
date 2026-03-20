/* ============================================================
   CHECKOUT.JS — Delivery selection, address management
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

    // ── Delivery method cards ──────────────────────────────
    document.querySelectorAll('.delivery-card').forEach(function (card) {
        card.addEventListener('click', function () {
            document.querySelectorAll('.delivery-card')
                .forEach(function (c) { c.classList.remove('selected'); });
            card.classList.add('selected');
            var radio = card.querySelector('input[type="radio"]');
            if (radio) radio.checked = true;

            // Show/hide address fields based on method
            var method = radio ? radio.value : '';
            var addrSection = document.getElementById('deliveryAddressSection');
            var pickupSection = document.getElementById('pickupInfoSection');

            if (addrSection) addrSection.style.display =
                method === 'Pickup' ? 'none' : 'block';
            if (pickupSection) pickupSection.style.display =
                method === 'Pickup' ? 'block' : 'none';
        });
    });

    // ── Payment method cards ───────────────────────────────
    document.querySelectorAll('.payment-method-card').forEach(function (card) {
        card.addEventListener('click', function () {
            document.querySelectorAll('.payment-method-card')
                .forEach(function (c) { c.classList.remove('selected'); });
            card.classList.add('selected');
            var radio = card.querySelector('input[type="radio"]');
            if (radio) radio.checked = true;
        });
    });

    // ── Place order button ─────────────────────────────────
    var placeOrderBtn = document.getElementById('placeOrderBtn');
    if (placeOrderBtn) {
        placeOrderBtn.addEventListener('click', function (e) {
            var form = document.getElementById('checkoutForm');
            if (!form) return;

            var selected = form.querySelector('input[name="DeliveryMethod"]:checked');
            if (!selected) {
                e.preventDefault();
                showAlert('#checkoutMessages', 'Please select a delivery method.', 'warning');
                return;
            }

            showSpinner(placeOrderBtn);
            // Form submits normally — spinner shows while page loads
        });
    }
});
