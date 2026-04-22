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

// ── Toast notification ────────────────────────────────────────────────────────
// type: 'error' | 'success' | 'info' | 'warning'
// Creates a .cal-toast element matching the server-rendered toast style.
function showToast(type, message) {
    var validTypes = ['error', 'success', 'info', 'warning'];
    var safeType = validTypes.indexOf(type) !== -1 ? type : 'error';

    var icons = {
        success: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#22c55e" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/><polyline points="22 4 12 14.01 9 11.01"/></svg>',
        error:   '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#ef4444" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"/><line x1="15" y1="9" x2="9" y2="15"/><line x1="9" y1="9" x2="15" y2="15"/></svg>',
        warning: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#f59e0b" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"/><line x1="12" y1="9" x2="12" y2="13"/><line x1="12" y1="17" x2="12.01" y2="17"/></svg>',
        info:    '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#3b82f6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"/><line x1="12" y1="16" x2="12" y2="12"/><line x1="12" y1="8" x2="12.01" y2="8"/></svg>'
    };

    var toast = document.createElement('div');
    toast.className = 'cal-toast cal-toast--' + safeType;
    toast.setAttribute('role', 'alert');
    toast.innerHTML = icons[safeType] + '<span>' + message + '</span>';

    var container = document.getElementById('toast-container');
    if (!container) {
        container = document.createElement('div');
        container.id = 'toast-container';
        container.className = 'fixed top-[80px] right-4 z-[2000] flex flex-col gap-3 pointer-events-none md:right-6';
        document.body.appendChild(container);
    }
    
    container.appendChild(toast);

    setTimeout(function () {
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(20px)';
        toast.style.transition = 'opacity 0.3s, transform 0.3s';
        setTimeout(function () { toast.remove(); }, 300);
    }, 4500);
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

// ── Debounce ─────────────────────────────────────────────────
function debounce(fn, delay) {
    var timer;
    return function () {
        var args = arguments, ctx = this;
        clearTimeout(timer);
        timer = setTimeout(function () { fn.apply(ctx, args); }, delay);
    };
}

// ── Throttle ─────────────────────────────────────────────────
function throttle(fn, limit) {
    var lastCall = 0;
    return function () {
        var now = Date.now();
        if (now - lastCall >= limit) {
            lastCall = now;
            fn.apply(this, arguments);
        }
    };
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
