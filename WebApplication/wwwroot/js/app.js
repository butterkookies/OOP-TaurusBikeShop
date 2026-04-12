/**
 * app.js — Vanilla JS replacing Bootstrap components
 * Mobile nav toggle, user dropdown, dark mode, modals, scroll effects, tabs
 */
(function () {
  'use strict';

  /* ─────────────────────────────────────────────────────
     Mobile Navigation Toggle
     ───────────────────────────────────────────────────── */
  const mobileToggle = document.getElementById('mobile-nav-toggle');
  const mobileNav = document.getElementById('mobile-nav');

  if (mobileToggle && mobileNav) {
    mobileToggle.addEventListener('click', () => {
      const isHidden = mobileNav.classList.toggle('hidden');
      mobileToggle.setAttribute('aria-expanded', String(!isHidden));
    });
  }

  /* ─────────────────────────────────────────────────────
     User Dropdown
     ───────────────────────────────────────────────────── */
  const dropdownToggle = document.getElementById('user-dropdown-toggle');
  const dropdownMenu = document.getElementById('user-dropdown-menu');

  if (dropdownToggle && dropdownMenu) {
    dropdownToggle.addEventListener('click', (e) => {
      e.stopPropagation();
      const willShow = dropdownMenu.classList.toggle('hidden');
      dropdownToggle.setAttribute('aria-expanded', String(!dropdownMenu.classList.contains('hidden')));
    });

    document.addEventListener('click', (e) => {
      if (!dropdownMenu.contains(e.target) && !dropdownToggle.contains(e.target)) {
        dropdownMenu.classList.add('hidden');
        dropdownToggle.setAttribute('aria-expanded', 'false');
      }
    });

    document.addEventListener('keydown', (e) => {
      if (e.key === 'Escape' && !dropdownMenu.classList.contains('hidden')) {
        dropdownMenu.classList.add('hidden');
        dropdownToggle.setAttribute('aria-expanded', 'false');
        dropdownToggle.focus();
      }
    });
  }

  /* ─────────────────────────────────────────────────────
     Navbar Scroll Effect
     ───────────────────────────────────────────────────── */
  const header = document.querySelector('.cal-header');
  if (header) {
    window.addEventListener('scroll', () => {
      header.classList.toggle('cal-header--scrolled', window.scrollY > 20);
    }, { passive: true });
  }

  /* ─────────────────────────────────────────────────────
     Dark Mode Toggle
     ───────────────────────────────────────────────────── */
  const darkToggle = document.getElementById('dark-mode-toggle');
  if (darkToggle) {
    const html = document.documentElement;
    const stored = localStorage.getItem('theme');
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

    if (stored === 'dark' || (!stored && prefersDark)) {
      html.classList.add('dark');
    }

    darkToggle.addEventListener('click', () => {
      html.classList.toggle('dark');
      localStorage.setItem('theme', html.classList.contains('dark') ? 'dark' : 'light');
    });
  }

  /* ─────────────────────────────────────────────────────
     Generic Modal API
     ───────────────────────────────────────────────────── */
  window.openModal = function (id) {
    const modal = document.getElementById(id);
    if (!modal) return;
    modal.hidden = false;
    document.body.style.overflow = 'hidden';
    const first = modal.querySelector('input:not([type="hidden"]), button, [tabindex]');
    if (first) first.focus();
  };

  window.closeModal = function (id) {
    const modal = document.getElementById(id);
    if (!modal) return;
    modal.hidden = true;
    document.body.style.overflow = '';
  };

  // Close on backdrop click
  document.addEventListener('click', (e) => {
    if (e.target.classList.contains('cal-modal-backdrop')) {
      e.target.hidden = true;
      document.body.style.overflow = '';
    }
  });

  // Close on Escape
  document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
      const modal = document.querySelector('.cal-modal-backdrop:not([hidden])');
      if (modal) {
        modal.hidden = true;
        document.body.style.overflow = '';
      }
    }
  });

  /* ─────────────────────────────────────────────────────
     Scroll Reveal (fade-in on viewport entry)
     ───────────────────────────────────────────────────── */
  const reveals = document.querySelectorAll('[data-reveal]');
  if (reveals.length && 'IntersectionObserver' in window) {
    const io = new IntersectionObserver((entries) => {
      entries.forEach((entry) => {
        if (entry.isIntersecting) {
          entry.target.classList.add('revealed');
          io.unobserve(entry.target);
        }
      });
    }, { threshold: 0.1 });
    reveals.forEach((el) => io.observe(el));
  }

  /* ─────────────────────────────────────────────────────
     Tab Switching
     ───────────────────────────────────────────────────── */
  document.querySelectorAll('[data-tab]').forEach((tab) => {
    tab.addEventListener('click', () => {
      const tabs = tab.closest('.cal-tabs');
      const panelContainer = tabs ? tabs.parentElement : null;
      const tabId = tab.dataset.tab;
      if (!tabs || !panelContainer) return;

      // Toggle active tab
      tabs.querySelectorAll('[data-tab]').forEach((t) => t.classList.remove('cal-tab--active'));
      tab.classList.add('cal-tab--active');

      // Toggle panels
      panelContainer.querySelectorAll('.cal-tab-panel').forEach((p) => p.classList.remove('cal-tab-panel--active'));
      const panel = panelContainer.querySelector('#tab-' + tabId);
      if (panel) panel.classList.add('cal-tab-panel--active');
    });
  });

  /* ─────────────────────────────────────────────────────
     Notification Toast Auto-Dismiss
     ───────────────────────────────────────────────────── */
  document.querySelectorAll('.cal-toast[data-autodismiss]').forEach((toast) => {
    const delay = parseInt(toast.dataset.autodismiss, 10) || 5000;
    setTimeout(() => {
      toast.style.opacity = '0';
      toast.style.transform = 'translateX(20px)';
      setTimeout(() => toast.remove(), 300);
    }, delay);
  });

})();
