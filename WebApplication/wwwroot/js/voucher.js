// WebApplication/wwwroot/js/voucher.js
// Handles the voucher input widget on the checkout page.
// Calls VoucherController.Validate / Remove via AJAX and updates
// the order summary totals in real time.
// Depends on: utils.js

'use strict';

document.addEventListener('DOMContentLoaded', () => {

    const voucherInput = document.getElementById('voucher-code-input');
    const voucherCombo = document.getElementById('assigned-vouchers-select');
    const wrapper = document.getElementById('voucher-inputs-wrapper');
    const applyBtn = document.getElementById('voucher-apply-btn');
    const removeBtn = document.getElementById('voucher-remove-btn');
    const voucherError = document.getElementById('voucher-error');
    const appliedBanner = document.getElementById('voucher-applied-banner');
    const appliedLabel = document.getElementById('voucher-applied-label');

    if (!voucherInput || !applyBtn) return;

    // =========================================================================
    // Apply voucher
    // =========================================================================
    applyBtn.addEventListener('click', applyVoucher);
    voucherInput.addEventListener('keydown', e => {
        if (e.key === 'Enter') {
            e.preventDefault();
            applyVoucher();
        }
    });

    async function applyVoucher() {
        const code = voucherInput.value.trim().toUpperCase();
        if (!code) {
            showVoucherError('Please enter a voucher code.');
            return;
        }

        applyBtn.disabled = true;
        applyBtn.textContent = 'Checking…';
        clearVoucherError();

        try {
            const data = await fetchWithCSRF('/Voucher/Validate', { code });

            if (!data.success) {
                showVoucherError(data.message ?? 'Invalid voucher code.');
                return;
            }

            const result = data.data;
            applyVoucherToSummary(result);

        } catch {
            showVoucherError('Unable to validate voucher. Please try again.');
        } finally {
            applyBtn.disabled = false;
            applyBtn.textContent = 'Apply';
        }
    }

    // =========================================================================
    // Remove voucher
    // =========================================================================
    removeBtn?.addEventListener('click', async () => {
        try {
            await fetchWithCSRF('/Voucher/Remove', {});
        } catch { /* non-fatal */ }

        clearAppliedVoucher();
    });

    // =========================================================================
    // Apply discount to order summary DOM
    // =========================================================================
    function applyVoucherToSummary(result) {
        // Show the applied banner
        if (appliedBanner) appliedBanner.style.display = '';
        if (appliedLabel) appliedLabel.textContent = `${result.voucherCode} — ${result.description}`;

        // Update discount line in order summary
        const formattedDiscount = result.discountAmount.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        updateOrderSummaryRow('checkout-discount-row', `−₱${formattedDiscount}`);
        updateOrderSummaryRow('checkout-grand-total-row', result.formattedNewTotal);

        const discountWrapper = document.getElementById('checkout-discount-row-wrapper');
        if (discountWrapper) discountWrapper.classList.remove('hidden');

        // Hide inputs wrapper
        if (wrapper) wrapper.style.display = 'none';

        // Store on the hidden checkout form inputs if they exist
        setHiddenInput('VoucherCode', result.voucherCode);
        setHiddenInput('DiscountAmount', result.discountAmount.toString());
    }

    function clearAppliedVoucher() {
        if (appliedBanner) appliedBanner.style.display = 'none';
        if (appliedLabel) appliedLabel.textContent = '';
        voucherInput.value = '';
        if (voucherCombo) voucherCombo.value = '';
        if (wrapper) wrapper.style.display = 'block';

        // Restore original subtotal as grand total
        const subtotalEl = document.getElementById('checkout-subtotal-value');
        const grandEl = document.getElementById('checkout-grand-total-row');
        if (subtotalEl && grandEl) grandEl.textContent = subtotalEl.textContent;

        updateOrderSummaryRow('checkout-discount-row', '₱0.00');

        const discountWrapper = document.getElementById('checkout-discount-row-wrapper');
        if (discountWrapper) discountWrapper.classList.add('hidden');

        setHiddenInput('VoucherCode', '');
        setHiddenInput('DiscountAmount', '0');
    }

    // =========================================================================
    // Helpers
    // =========================================================================

    function showVoucherError(msg) {
        if (!voucherError) return;
        voucherError.textContent = msg;
        voucherError.style.display = '';
    }

    function clearVoucherError() {
        if (!voucherError) return;
        voucherError.textContent = '';
        voucherError.style.display = 'none';
    }

    function updateOrderSummaryRow(elementId, text) {
        const el = document.getElementById(elementId);
        if (el) el.textContent = text;
    }

    function setHiddenInput(name, value) {
        const input = document.querySelector(`input[name="${name}"]`);
        if (input) input.value = value;
    }

});