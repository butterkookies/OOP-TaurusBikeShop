/* ============================================================
   PAYMENT.JS — GCash number input, proof upload, OTP
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

    // ── GCash number formatter ─────────────────────────────
    var gcashInput = document.getElementById('gcashNumber');
    if (gcashInput) {
        gcashInput.addEventListener('input', function () {
            var val = gcashInput.value.replace(/\D/g, '');
            if (val.startsWith('63')) val = '0' + val.slice(2);
            gcashInput.value = val.slice(0, 11);
        });
    }

    // ── Proof upload drag-and-drop (legacy selector compat) ──
    var uploadBox   = document.getElementById('proofUploadBox')  || document.getElementById('file-drop-zone');
    var fileInput   = document.getElementById('proofFileInput')  || document.getElementById('payment-file-input');
    var fileNameEl  = document.getElementById('proofFileName');

    if (uploadBox && fileInput) {
        uploadBox.addEventListener('click', function () { fileInput.click(); });

        fileInput.addEventListener('change', function () {
            var file = fileInput.files[0];
            if (file) {
                uploadBox.classList.add('has-file');
                if (fileNameEl) fileNameEl.textContent = file.name;
            }
        });

        ['dragover', 'dragenter'].forEach(function (ev) {
            uploadBox.addEventListener(ev, function (e) {
                e.preventDefault();
                uploadBox.classList.add('is-dragover');
            });
        });

        ['dragleave', 'drop'].forEach(function (ev) {
            uploadBox.addEventListener(ev, function (e) {
                e.preventDefault();
                uploadBox.classList.remove('is-dragover');
                if (ev === 'drop' && e.dataTransfer.files.length) {
                    fileInput.files = e.dataTransfer.files;
                    fileInput.dispatchEvent(new Event('change'));
                }
            });
        });
    }

    // ── OTP input auto-advance ─────────────────────────────
    var otpInputs = document.querySelectorAll('.otp-digit');
    otpInputs.forEach(function (input, i) {
        input.addEventListener('input', function () {
            input.value = input.value.replace(/\D/g, '').slice(0, 1);
            if (input.value && otpInputs[i + 1]) otpInputs[i + 1].focus();
        });
        input.addEventListener('keydown', function (e) {
            if (e.key === 'Backspace' && !input.value && otpInputs[i - 1])
                otpInputs[i - 1].focus();
        });
    });

    // ── Submit payment form ────────────────────────────────
    var submitBtn = document.getElementById('submitPaymentBtn');
    if (submitBtn) {
        submitBtn.addEventListener('click', function () {
            showSpinner(submitBtn);
        });
    }
});
