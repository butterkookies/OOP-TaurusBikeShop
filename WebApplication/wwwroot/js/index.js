/**
 * Taurus Bike Shop — Index.js
 * Place at: /wwwroot/js/Index.js
 * Scroll-triggered entrance animations + micro-interactions
 */

(function () {
    'use strict';

    /* ============================================================
       INTERSECTION OBSERVER — Category card entrance animation
       ============================================================ */
    const catCards = document.querySelectorAll('.cat-card');

    if (catCards.length && 'IntersectionObserver' in window) {
        const observer = new IntersectionObserver(
            (entries) => {
                entries.forEach((entry, idx) => {
                    if (entry.isIntersecting) {
                        const el = entry.target;
                        const delay = (parseInt(el.dataset.cardIdx) || idx) * 80;
                        setTimeout(() => el.classList.add('visible'), delay);
                        observer.unobserve(el);
                    }
                });
            },
            { threshold: 0.12, rootMargin: '0px 0px -48px 0px' }
        );

        catCards.forEach((card, i) => {
            card.dataset.cardIdx = i;
            observer.observe(card);
        });
    } else {
        // Fallback: show all immediately
        catCards.forEach(c => c.classList.add('visible'));
    }

    /* ============================================================
       INTERSECTION OBSERVER — Value cards and big-stats entrance
       ============================================================ */
    const fadeTargets = document.querySelectorAll('.value-card, .big-stat, .section-header');

    if (fadeTargets.length && 'IntersectionObserver' in window) {
        const fadeObserver = new IntersectionObserver(
            (entries) => {
                entries.forEach((entry, idx) => {
                    if (entry.isIntersecting) {
                        const el = entry.target;
                        setTimeout(() => {
                            el.style.opacity = '1';
                            el.style.transform = 'translateY(0)';
                        }, idx * 60);
                        fadeObserver.unobserve(el);
                    }
                });
            },
            { threshold: 0.1 }
        );

        fadeTargets.forEach(el => {
            el.style.opacity = '0';
            el.style.transform = 'translateY(24px)';
            el.style.transition = 'opacity 0.7s cubic-bezier(0.16, 1, 0.3, 1), transform 0.7s cubic-bezier(0.16, 1, 0.3, 1)';
            fadeObserver.observe(el);
        });
    }

    /* ============================================================
       STAT COUNTER ANIMATION — hero stats and big-stats
       ============================================================ */
    function animateCount(el, target, duration) {
        const start = performance.now();
        const isNum = /^\d+$/.test(target);
        if (!isNum) return; // skip non-numeric e.g. "PH"

        const end = parseInt(target, 10);
        const startVal = 0;

        function step(now) {
            const elapsed = now - start;
            const progress = Math.min(elapsed / duration, 1);
            // Ease-out cubic
            const eased = 1 - Math.pow(1 - progress, 3);
            const current = Math.round(startVal + (end - startVal) * eased);
            el.textContent = current;
            if (progress < 1) requestAnimationFrame(step);
        }
        requestAnimationFrame(step);
    }

    const statNumbers = document.querySelectorAll('.hero-stat__number, .big-stat__num');

    if (statNumbers.length && 'IntersectionObserver' in window) {
        const statObserver = new IntersectionObserver(
            (entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        const el = entry.target;
                        const target = el.textContent.trim();
                        animateCount(el, target, 1200);
                        statObserver.unobserve(el);
                    }
                });
            },
            { threshold: 0.5 }
        );
        statNumbers.forEach(el => statObserver.observe(el));
    }

    /* ============================================================
       PARALLAX — hero wheel subtle drift on mouse move
       ============================================================ */
    const wheelWrap = document.querySelector('.wheel-wrap');
    if (wheelWrap) {
        document.addEventListener('mousemove', (e) => {
            const cx = window.innerWidth / 2;
            const cy = window.innerHeight / 2;
            const dx = (e.clientX - cx) / cx;
            const dy = (e.clientY - cy) / cy;
            wheelWrap.style.transform = `translate(${dx * 12}px, ${dy * 8}px)`;
        }, { passive: true });
    }

    /* ============================================================
       HERO SCROLL FADE — fade hero content as user scrolls down
       ============================================================ */
    const heroContent = document.querySelector('.hero__content');
    if (heroContent) {
        let ticking = false;
        window.addEventListener('scroll', () => {
            if (!ticking) {
                requestAnimationFrame(() => {
                    const scrollY = window.scrollY;
                    const heroH = document.querySelector('.hero')?.offsetHeight || window.innerHeight;
                    if (scrollY < heroH * 0.6) {
                        const opacity = 1 - (scrollY / (heroH * 0.5));
                        heroContent.style.opacity = Math.max(opacity, 0);
                        heroContent.style.transform = `translateY(${scrollY * 0.12}px)`;
                    }
                    ticking = false;
                });
                ticking = true;
            }
        }, { passive: true });
    }

})();