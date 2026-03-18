// WebApplication/wwwroot/js/customer.js
// Customer area JS — loaded on authenticated customer pages.
// Depends on: utils.js, site.js

'use strict';

document.addEventListener('DOMContentLoaded', () => {

    // =========================================================================
    // Profile dropdown keyboard accessibility
    // =========================================================================
    const userDropdownBtn = document.querySelector('.tbs-nav__user-btn');
    if (userDropdownBtn) {
        userDropdownBtn.addEventListener('keydown', e => {
            if (e.key === 'Enter' || e.key === ' ') {
                e.preventDefault();
                userDropdownBtn.click();
            }
        });
    }

    // =========================================================================
    // Notification bell — placeholder for future unread count badge
    // =========================================================================
    const notifBell = document.getElementById('notif-bell');
    if (notifBell) {
        notifBell.addEventListener('click', () => {
            // Will be wired to notification panel in a future step
        });
    }

    // =========================================================================
    // Session expiry warning
    // Shows a dismissible info toast 5 minutes before the 30-minute session expires.
    // The session timer resets on every page load, so this only fires if the user
    // has been idle on one page for 25 minutes without any navigation.
    // =========================================================================
    const SESSION_TIMEOUT_MS = 30 * 60 * 1000; // 30 minutes (must match appsettings)
    const WARNING_BEFORE_MS  =  5 * 60 * 1000; // warn 5 minutes before

    let sessionWarningTimer = null;

    function scheduleSessionWarning() {
        clearTimeout(sessionWarningTimer);
        sessionWarningTimer = setTimeout(() => {
            showAlert(
                'info',
                'Your session will expire in 5 minutes due to inactivity. ' +
                'Save your work or click anywhere to stay signed in.',
                10000
            );
        }, SESSION_TIMEOUT_MS - WARNING_BEFORE_MS);
    }

    // Reset the timer on any user interaction
    ['click', 'keydown', 'scroll', 'mousemove'].forEach(event => {
        document.addEventListener(event, debounce(scheduleSessionWarning, 1000), { passive: true });
    });

    scheduleSessionWarning();

    // =========================================================================
    // Sidebar active link highlighting
    // Marks the current page's sidebar link as active based on the URL path.
    // =========================================================================
    const currentPath = window.location.pathname.toLowerCase();
    document.querySelectorAll('.tbs-sidebar__nav a').forEach(link => {
        const href = link.getAttribute('href')?.toLowerCase();
        if (href && currentPath.startsWith(href) && href !== '/') {
            link.classList.add('active');
        }
    });

});