/* ============================================================
   UTILS.JS — Shared utilities for all pages
   Loaded via _Layout.cshtml <script> tag
   ============================================================ */

// ── CSRF-aware fetch ─────────────────────────────────────────
// Usage: fetchWithCSRF(url, dataObject) — serialises dataObject as JSON body.
// Returns a Promise that resolves to the parsed JSON response.
function fetchWithCSRF(url, data) {
    var token = document.querySelector('input[name="__RequestVerificationToken"]');
    var headers = { 'Content-Type': 'application/json' };
    if (token) headers['RequestVerificationToken'] = token.value;

    return fetch(url, {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(data)
    }).then(function (res) {
        if (!res.ok) throw new Error('HTTP ' + res.status);
        return res.json();
    });
}

// ── Parse JSON response (pass-through — fetchWithCSRF already parses) ────────
function parseJsonResponse(data) {
    return Promise.resolve(data);
}

// ── Toast notification via #notificationAlert ─────────────────────────────────
// type: 'success' | 'error'
function showToast(type, message) {
    var box = document.getElementById('notificationAlert');
    var msg = document.getElementById('alertMessage');
    if (!box || !msg) return;
    msg.textContent = message;
    box.style.background = type === 'success' ? '#2e7d32' : '#d32f2f';
    box.style.display = 'flex';
    setTimeout(function () { box.style.display = 'none'; }, 4000);
}

// ── Currency formatter ───────────────────────────────────────
function formatCurrency(amount) {
    return '₱\u00A0' + parseFloat(amount).toLocaleString('en-PH', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

// ── Alert banner ─────────────────────────────────────────────
// type: 'success' | 'danger' | 'warning' | 'info'
function showAlert(container, message, type, autoDismiss) {
    type = type || 'info';
    autoDismiss = autoDismiss !== false;

    var alert = document.createElement('div');
    alert.className = 'alert alert-' + type + ' alert-dismissible';
    alert.innerHTML =
        '<span>' + message + '</span>' +
        '<button class="alert-close" type="button" aria-label="Close">&times;</button>';

    alert.querySelector('.alert-close').addEventListener('click', function () {
        alert.remove();
    });

    var el = typeof container === 'string'
        ? document.querySelector(container)
        : container;

    if (!el) return;
    el.innerHTML = '';
    el.appendChild(alert);

    if (autoDismiss) {
        setTimeout(function () { if (alert.parentNode) alert.remove(); }, 5000);
    }
}

// ── Spinner helpers ──────────────────────────────────────────
function showSpinner(btn) {
    if (!btn) return;
    btn.dataset.originalText = btn.innerHTML;
    btn.disabled = true;
    btn.innerHTML = '<span class="spinner"></span>';
}

function hideSpinner(btn) {
    if (!btn) return;
    btn.disabled = false;
    btn.innerHTML = btn.dataset.originalText || btn.innerHTML;
}

// ── Confirmation modal helper ────────────────────────────────
function confirmAction(message, onConfirm) {
    if (window.confirm(message)) onConfirm();
}

// ── Notification badge updater ───────────────────────────────
function updateCartBadge(count) {
    var badge = document.querySelector('.cart-badge');
    if (!badge) return;
    badge.textContent = count;
    badge.style.display = count > 0 ? 'flex' : 'none';
}

// ── Dismissible alerts auto-bind ─────────────────────────────
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.alert-close').forEach(function (btn) {
        btn.addEventListener('click', function () {
            btn.closest('.alert').remove();
        });
    });

    // Auto-dismiss success alerts after 5s
    document.querySelectorAll('.alert-success').forEach(function (el) {
        setTimeout(function () { if (el.parentNode) el.remove(); }, 5000);
    });
});
