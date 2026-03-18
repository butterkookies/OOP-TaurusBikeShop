// WebApplication/wwwroot/js/payment.js
// Handles file-drop zone interactions and image preview
// on the payment proof upload page.
// Depends on: utils.js

'use strict';

document.addEventListener('DOMContentLoaded', () => {

    // Wire up both possible drop zones (GCash and BankTransfer pages)
    initDropZone('gcash-drop-zone',  'gcash-file-input',  'gcash-preview');
    initDropZone('bank-drop-zone',   'bank-file-input',   'bank-preview');

    // =========================================================================
    // File drop zone initialisation
    // =========================================================================
    function initDropZone(zoneId, inputId, previewId) {
        const zone    = document.getElementById(zoneId);
        const input   = document.getElementById(inputId);
        const preview = document.getElementById(previewId);

        if (!zone || !input) return;

        // Drag-and-drop visual feedback
        ['dragenter', 'dragover'].forEach(evt => {
            zone.addEventListener(evt, e => {
                e.preventDefault();
                zone.classList.add('tbs-file-drop--active');
            });
        });
        ['dragleave', 'drop'].forEach(evt => {
            zone.addEventListener(evt, () => {
                zone.classList.remove('tbs-file-drop--active');
            });
        });

        // Drop
        zone.addEventListener('drop', e => {
            e.preventDefault();
            const file = e.dataTransfer?.files?.[0];
            if (file) {
                // Transfer dropped file to the actual input
                const dt = new DataTransfer();
                dt.items.add(file);
                input.files = dt.files;
                handleFileSelected(file, preview, zone);
            }
        });

        // Normal file picker
        input.addEventListener('change', () => {
            const file = input.files?.[0];
            if (file) handleFileSelected(file, preview, zone);
        });
    }

    // =========================================================================
    // Handle selected / dropped file
    // =========================================================================
    function handleFileSelected(file, previewEl, zoneEl) {
        if (!previewEl) return;

        // Size guard (5 MB)
        const maxBytes = 5 * 1024 * 1024;
        if (file.size > maxBytes) {
            showAlert('error', 'File is too large. Maximum size is 5 MB.');
            return;
        }

        // Show preview or PDF label
        if (file.type === 'application/pdf') {
            previewEl.style.display = '';
            previewEl.innerHTML =
                `<p class="tbs-file-drop__pdf-label">📄 ${escapeHtml(file.name)}</p>`;
        } else if (file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = e => {
                previewEl.style.display = '';
                previewEl.innerHTML =
                    `<img src="${e.target.result}" alt="Preview" />`;
            };
            reader.readAsDataURL(file);
        }

        // Update the drop zone label to show the selected filename
        const textEl = zoneEl?.querySelector('.tbs-file-drop__text');
        if (textEl) {
            textEl.innerHTML =
                `<strong>${escapeHtml(file.name)}</strong><br>` +
                `<small>Click to change</small>`;
        }
    }

    // =========================================================================
    // Submit guard — disable button after first click
    // =========================================================================
    document.querySelectorAll(
        'form[action*="SubmitGCash"], form[action*="SubmitBankTransfer"]'
    ).forEach(form => {
        form.addEventListener('submit', e => {
            const fileInput = form.querySelector('input[type="file"]');
            if (!fileInput?.files?.length) {
                e.preventDefault();
                showAlert('error', 'Please select a file to upload.');
                return;
            }

            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn) {
                submitBtn.disabled    = true;
                submitBtn.textContent = 'Uploading…';
            }
        });
    });

});