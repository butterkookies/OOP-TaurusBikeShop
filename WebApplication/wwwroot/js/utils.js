// WebApplication/wwwroot/js/utils.js
// Shared utilities — imported by all other JS files.
// No jQuery dependency — pure vanilla JS.

'use strict';

/**
 * Formats a decimal number as Philippine Peso currency.
 * @param {number} amount - The amount to format.
 * @returns {string} Formatted string, e.g. "₱1,234.50"
 */
function formatCurrency(amount) {
    return '₱' + Number(amount).toLocaleString('en-PH', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

/**
 * Returns a debounced version of the given function.
 * The debounced function delays invoking fn until after
 * delayMs milliseconds have elapsed since the last invocation.
 * @param {Function} fn - The function to debounce.
 * @param {number} delayMs - Delay in milliseconds (minimum 300 recommended for search inputs).
 * @returns {Function} The debounced function.
 */
function debounce(fn, delayMs) {
    let timer = null;
    return function (...args) {
        clearTimeout(timer);
        timer = setTimeout(() => fn.apply(this, args), delayMs);
    };
}

/**
 * Returns a throttled version of the given function.
 * The throttled function invokes fn at most once per limitMs milliseconds.
 * @param {Function} fn - The function to throttle.
 * @param {number} limitMs - Minimum interval in milliseconds between calls.
 * @returns {Function} The throttled function.
 */
function throttle(fn, limitMs) {
    let lastCall = 0;
    return function (...args) {
        const now = Date.now();
        if (now - lastCall >= limitMs) {
            lastCall = now;
            fn.apply(this, args);
        }
    };
}

/**
 * Displays a temporary toast alert at the top-right of the screen.
 * Used for AJAX operation feedback — success, error, or info.
 * @param {'success'|'error'|'info'} type - Toast variant.
 * @param {string} message - The message text to display.
 * @param {number} [durationMs=4000] - How long to show the toast before auto-dismiss.
 */
function showAlert(type, message, durationMs = 4000) {
    const icons = {
        success: `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" viewBox="0 0 16 16" aria-hidden="true"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/></svg>`,
        error:   `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" viewBox="0 0 16 16" aria-hidden="true"><path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/></svg>`,
        info:    `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" viewBox="0 0 16 16" aria-hidden="true"><path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z"/></svg>`
    };

    let container = document.querySelector('.tbs-toast-container');
    if (!container) {
        container = document.createElement('div');
        container.className = 'tbs-toast-container';
        container.setAttribute('aria-live', 'polite');
        container.setAttribute('aria-atomic', 'true');
        document.body.appendChild(container);
    }

    const toast = document.createElement('div');
    toast.className = `tbs-toast tbs-toast--${type}`;
    toast.setAttribute('role', 'alert');
    toast.innerHTML = `
        <span class="tbs-toast__icon">${icons[type] ?? icons.info}</span>
        <span class="tbs-toast__message">${escapeHtml(message)}</span>
        <button type="button" class="tbs-toast__close" aria-label="Close">&#10005;</button>
    `;

    container.appendChild(toast);

    // Close button
    toast.querySelector('.tbs-toast__close').addEventListener('click', () => dismissToast(toast));

    // Auto-dismiss
    setTimeout(() => dismissToast(toast), durationMs);
}

/**
 * Animates and removes a toast element from the DOM.
 * @param {HTMLElement} toast - The toast element to dismiss.
 */
function dismissToast(toast) {
    if (!toast || toast._dismissing) return;
    toast._dismissing = true;
    toast.style.transition = 'opacity 0.25s ease, transform 0.25s ease';
    toast.style.opacity = '0';
    toast.style.transform = 'translateX(120%)';
    setTimeout(() => toast.remove(), 280);
}

/**
 * Escapes HTML special characters to prevent XSS in dynamically inserted strings.
 * @param {string} str - The string to escape.
 * @returns {string} HTML-escaped string.
 */
function escapeHtml(str) {
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;');
}

/**
 * Wraps the Fetch API with automatic CSRF token injection.
 * All AJAX POST requests in this application must use this function
 * instead of raw fetch() to satisfy [ValidateAntiForgeryToken].
 *
 * The CSRF token is read from the hidden __RequestVerificationToken input
 * that ASP.NET Core renders inside forms via @Html.AntiForgeryToken().
 * For pages without a form, add a standalone hidden input with that name.
 *
 * @param {string} url - The URL to fetch.
 * @param {RequestInit} [options={}] - Standard fetch options.
 * @returns {Promise<Response>} The fetch response.
 */
async function fetchWithCSRF(url, options = {}) {
    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

    const headers = {
        'Content-Type': 'application/json',
        ...(options.headers ?? {}),
        ...(token ? { 'RequestVerificationToken': token } : {})
    };

    return fetch(url, { ...options, headers });
}

/**
 * Reads a JSON response from a fetchWithCSRF call and parses it.
 * Throws a descriptive error if the response is not OK or not valid JSON.
 * @param {Response} response - The fetch Response object.
 * @returns {Promise<object>} Parsed JSON body.
 */
async function parseJsonResponse(response) {
    if (!response.ok) {
        throw new Error(`Server returned ${response.status}: ${response.statusText}`);
    }
    const data = await response.json();
    return data;
}