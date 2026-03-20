// WebApplication/wwwroot/js/support.js
// Handles: character counters on the create and reply forms,
// reply form submit guard, and auto-scroll to thread bottom on load.
// Depends on: utils.js

'use strict';

document.addEventListener('DOMContentLoaded', () => {

    // =========================================================================
    // CHARACTER COUNTER — create ticket message + reply textarea
    // =========================================================================
    function wireCharCounter(textareaSelector, counterId) {
        const textarea = document.querySelector(textareaSelector);
        const counter  = document.getElementById(counterId);
        if (!textarea || !counter) return;

        const update = () => {
            const len = textarea.value.length;
            counter.textContent = len;
            counter.style.color = len > 1900
                ? 'var(--tbs-error)'
                : len > 1600
                    ? 'var(--tbs-warning)'
                    : 'var(--tbs-gray-400)';
        };
        textarea.addEventListener('input', update);
        update();
    }

    wireCharCounter('.tbs-support-create__textarea', 'msg-char-count');
    wireCharCounter('.tbs-ticket-reply-form__textarea', 'reply-char-count');

    // =========================================================================
    // REPLY FORM SUBMIT GUARD — prevent double submission
    // =========================================================================
    const replyForm = document.getElementById('reply-form');
    replyForm?.addEventListener('submit', () => {
        const btn = document.getElementById('reply-submit-btn');
        if (btn) {
            btn.disabled    = true;
            btn.textContent = 'Sending…';
        }
    });

    // =========================================================================
    // AUTO-SCROLL to bottom of thread on detail page load
    // =========================================================================
    const thread = document.getElementById('ticket-thread');
    if (thread) {
        thread.scrollTop = thread.scrollHeight;
    }

    // =========================================================================
    // CREATE FORM SUBMIT GUARD
    // =========================================================================
    const createForm = document.querySelector('form[action*="Support/Create"]');
    createForm?.addEventListener('submit', () => {
        const btn = createForm.querySelector('button[type="submit"]');
        if (btn) {
            btn.disabled    = true;
            btn.textContent = 'Submitting…';
        }
    });

});