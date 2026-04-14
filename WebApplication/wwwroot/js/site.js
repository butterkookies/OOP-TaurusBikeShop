// WebApplication/wwwroot/js/site.js
// Global site initialisation — runs on every page.
// Depends on: utils.js (loaded before this in _Layout.cshtml)

'use strict';

document.addEventListener('DOMContentLoaded', () => {

    // =========================================================================
    // Bootstrap tooltip and popover initialisation
    // =========================================================================
    const tooltipElements = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipElements.forEach(el => new bootstrap.Tooltip(el));

    const popoverElements = document.querySelectorAll('[data-bs-toggle="popover"]');
    popoverElements.forEach(el => new bootstrap.Popover(el));

    // =========================================================================
    // Navbar scroll shadow
    // Adds .scrolled class to navbar when page is scrolled down > 10px
    // =========================================================================
    const navbar = document.querySelector('.tbs-navbar');
    if (navbar) {
        const handleScroll = throttle(() => {
            navbar.classList.toggle('scrolled', window.scrollY > 10);
        }, 50);
        window.addEventListener('scroll', handleScroll, { passive: true });
    }

    // =========================================================================
    // Toast dismiss — handles TempData-rendered toasts from _NotificationAlert
    // (AJAX-created toasts are handled in showAlert() in utils.js)
    // =========================================================================
    document.querySelectorAll('.tbs-toast__close').forEach(btn => {
        btn.addEventListener('click', () => {
            const toast = btn.closest('.tbs-toast');
            if (toast) dismissToast(toast);
        });
    });

    // Auto-dismiss server-rendered toasts after 5 seconds
    document.querySelectorAll('.tbs-toast').forEach(toast => {
        setTimeout(() => dismissToast(toast), 5000);
    });

    // =========================================================================
    // Cart badge — load count on every page
    // =========================================================================
    refreshCartBadge();

    // =========================================================================
    // Notification badge — load count on every page + poll every 60s
    // =========================================================================
    refreshNotificationBadge();
    setInterval(refreshNotificationBadge, 60000);

    // =========================================================================
    // Anti-forgery token meta tag for fetchWithCSRF
    // Injects a standalone CSRF token on pages that have no visible form,
    // so AJAX calls still work correctly.
    // =========================================================================
    if (!document.querySelector('input[name="__RequestVerificationToken"]')) {
        fetch('/antiforgery/token', { method: 'GET' })
            .catch(() => { /* silent — pages with forms already have the token */ });
    }

});

// =============================================================================
// Cart badge refresh
// Calls CartController.GetCartCount and updates the #cart-badge element.
// Called on page load and after any cart mutation.
// =============================================================================
async function refreshCartBadge() {
    const badge = document.getElementById('cart-badge');
    if (!badge) return;

    try {
        const response = await fetch('/Cart/GetCartCount', {
            method: 'GET',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });
        if (!response.ok) return;
        const data = await response.json();
        const count = data?.data?.count ?? 0;
        badge.textContent = count > 0 ? count : '';
        badge.style.display = count > 0 ? '' : 'none';
    } catch {
        // Non-fatal — badge stays at its last known value
    }
}

// =============================================================================
// Notification badge refresh
// Calls CustomerController.NotificationCount and updates #notification-badge.
// Called on page load and then polled every 60 seconds.
// =============================================================================
async function refreshNotificationBadge() {
    const badge = document.getElementById('notification-badge');
    if (!badge) return;

    try {
        const response = await fetch('/Customer/NotificationCount', {
            method: 'GET',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });
        if (!response.ok) return;
        const data = await response.json();
        const count = data?.count ?? 0;
        badge.textContent = count > 99 ? '99+' : count;
        badge.classList.toggle('hidden', count <= 0);
    } catch {
        // Non-fatal — badge stays at its last known value
    }
}