/**
 * checkout.js — delivery method selection and form validation on the Checkout page.
 */

(function () {
    'use strict';

    var SHIPPING_RATES = {
        standard: 150,
        express: 350,
        pickup: 0
    };

    function formatPHP(amount) {
        return '\u20b1' + Number(amount).toLocaleString('en-PH', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function updateShippingDisplay(method) {
        var fee = SHIPPING_RATES[method] !== undefined ? SHIPPING_RATES[method] : 150;

        var shipFeeEl = document.getElementById('shipping-fee-display');
        var shipHiddenEl = document.getElementById('shipping-fee-value');
        var subtotalEl = document.getElementById('checkout-subtotal');
        var totalEl = document.getElementById('checkout-total');

        if (shipFeeEl) shipFeeEl.textContent = formatPHP(fee);
        if (shipHiddenEl) shipHiddenEl.value = fee;

        if (subtotalEl && totalEl) {
            var subtotal = parseFloat(subtotalEl.dataset.subtotal) || 0;
            totalEl.textContent = formatPHP(subtotal + fee);
        }
    }

    function validateForm(form) {
        var required = form.querySelectorAll('[required]');
        var valid = true;

        required.forEach(function (field) {
            if (!field.value.trim()) {
                field.classList.add('is-invalid');
                valid = false;
            } else {
                field.classList.remove('is-invalid');
            }
        });

        return valid;
    }

    function setLoading(btn, loading) {
        if (!btn) return;
        if (loading) {
            btn.dataset.originalText = btn.textContent;
            btn.disabled = true;
            btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Placing Order...';
        } else {
            btn.disabled = false;
            btn.textContent = btn.dataset.originalText || 'Place Order';
        }
    }

    function init() {
        // Delivery method radio buttons
        var deliveryRadios = document.querySelectorAll('input[name="deliveryMethod"]');
        deliveryRadios.forEach(function (radio) {
            radio.addEventListener('change', function () {
                updateShippingDisplay(this.value);

                // Toggle address fields visibility for pickup
                var addressSection = document.getElementById('address-section');
                if (addressSection) {
                    addressSection.style.display = this.value === 'pickup' ? 'none' : '';
                }
            });
        });

        // Set initial shipping display
        var checkedRadio = document.querySelector('input[name="deliveryMethod"]:checked');
        if (checkedRadio) {
            updateShippingDisplay(checkedRadio.value);
        }

        // Form submission
        var checkoutForm = document.getElementById('checkout-form');
        var submitBtn = document.getElementById('place-order-btn');

        if (checkoutForm) {
            checkoutForm.addEventListener('submit', function (e) {
                if (!validateForm(checkoutForm)) {
                    e.preventDefault();
                    var firstInvalid = checkoutForm.querySelector('.is-invalid');
                    if (firstInvalid) firstInvalid.scrollIntoView({ behavior: 'smooth', block: 'center' });
                    return;
                }
                setLoading(submitBtn, true);
            });
        }

        // Live validation — clear error on input
        document.querySelectorAll('[required]').forEach(function (field) {
            field.addEventListener('input', function () {
                if (this.value.trim()) {
                    this.classList.remove('is-invalid');
                }
            });
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
