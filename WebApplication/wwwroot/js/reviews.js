/**
 * reviews.js
 * Handles star rating picker and review form submission via fetch().
 */

(function () {
    'use strict';

    // ── Helpers ──────────────────────────────────────────────────────────────

    function renderStars(rating) {
        const full  = Math.round(rating);
        const empty = 5 - full;
        return '&#9733;'.repeat(full) + '&#9734;'.repeat(empty);
    }

    function escapeHtml(str) {
        if (!str) return '';
        return str
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    // ── Star Picker ──────────────────────────────────────────────────────────

    const starPicker      = document.getElementById('star-picker');
    const selectedRating  = document.getElementById('selected-rating');

    if (starPicker && selectedRating) {
        const starBtns = starPicker.querySelectorAll('.star-btn');

        function highlightStars(upTo) {
            starBtns.forEach(btn => {
                btn.classList.toggle('selected', parseInt(btn.dataset.value) <= upTo);
            });
        }

        starBtns.forEach(btn => {
            btn.addEventListener('mouseenter', () => {
                highlightStars(parseInt(btn.dataset.value));
            });

            btn.addEventListener('mouseleave', () => {
                highlightStars(parseInt(selectedRating.value) || 0);
            });

            btn.addEventListener('click', () => {
                const val = parseInt(btn.dataset.value);
                selectedRating.value = val;
                highlightStars(val);
            });
        });
    }

    // ── Toggle Form ──────────────────────────────────────────────────────────

    const toggleBtn     = document.getElementById('toggle-review-form');
    const formWrapper   = document.getElementById('review-form-wrapper');
    const cancelBtn     = document.getElementById('cancel-review-btn');

    if (toggleBtn && formWrapper) {
        // Populate hidden product/order fields from the section's data attribute
        const section = document.getElementById('reviews-section');
        const productId = section ? parseInt(section.dataset.productId || '0') : 0;

        const productIdInput = document.getElementById('review-product-id');
        if (productIdInput) productIdInput.value = productId;

        toggleBtn.addEventListener('click', () => {
            formWrapper.style.display = 'block';
            toggleBtn.style.display   = 'none';
        });
    }

    if (cancelBtn && formWrapper && toggleBtn) {
        cancelBtn.addEventListener('click', () => {
            formWrapper.style.display = 'none';
            toggleBtn.style.display   = '';
            clearForm();
        });
    }

    function clearForm() {
        const comment        = document.getElementById('review-comment');
        const rating         = document.getElementById('selected-rating');
        const feedback       = document.getElementById('review-feedback');
        const starBtns       = starPicker ? starPicker.querySelectorAll('.star-btn') : [];

        if (comment)   comment.value  = '';
        if (rating)    rating.value   = '0';
        starBtns.forEach(b => b.classList.remove('selected'));
        if (feedback) { feedback.style.display = 'none'; feedback.textContent = ''; }
    }

    // ── Form Submission ──────────────────────────────────────────────────────

    const submitBtn = document.getElementById('submit-review-btn');

    if (submitBtn) {
        submitBtn.addEventListener('click', async () => {
            const productIdEl = document.getElementById('review-product-id');
            const orderIdEl   = document.getElementById('review-order-id');
            const ratingEl    = document.getElementById('selected-rating');
            const commentEl   = document.getElementById('review-comment');
            const feedback    = document.getElementById('review-feedback');

            const productId = productIdEl ? parseInt(productIdEl.value) : 0;
            const orderId   = orderIdEl   ? parseInt(orderIdEl.value)   : 0;
            const rating    = ratingEl    ? parseInt(ratingEl.value)    : 0;
            const comment   = commentEl   ? commentEl.value.trim()      : '';

            // Validate
            if (rating < 1 || rating > 5) {
                showFeedback(feedback, 'Please select a star rating.', false);
                return;
            }

            submitBtn.disabled = true;
            submitBtn.textContent = 'Submitting...';

            try {
                const params = new URLSearchParams({ productId, orderId, rating, comment });
                const response = await fetch('/Review/Submit', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    body: params.toString()
                });

                const data = await response.json();

                if (data.success) {
                    showFeedback(feedback, 'Review submitted! Thank you.', true);
                    clearForm();

                    // Hide form, hide toggle button (already reviewed)
                    if (formWrapper) formWrapper.style.display = 'none';
                    if (toggleBtn)   toggleBtn.style.display   = 'none';

                    // Reload the review list via AJAX
                    await reloadReviews(productId);
                } else {
                    showFeedback(feedback, data.message || 'Could not submit review.', false);
                }
            } catch (err) {
                showFeedback(feedback, 'An error occurred. Please try again.', false);
            } finally {
                submitBtn.disabled = false;
                submitBtn.textContent = 'Submit Review';
            }
        });
    }

    function showFeedback(el, message, success) {
        if (!el) return;
        el.textContent    = message;
        el.className      = 'review-feedback ' + (success ? 'success' : 'error');
        el.style.display  = 'inline-block';
    }

    // ── Reload Review List ───────────────────────────────────────────────────

    async function reloadReviews(productId) {
        const reviewList     = document.getElementById('review-list');
        const countLabel     = document.getElementById('review-count-label');
        const avgStarsEl     = document.getElementById('avg-stars-display');
        const noReviews      = document.getElementById('no-reviews-placeholder');

        if (!reviewList) return;

        try {
            const res     = await fetch(`/Review/ProductReviews/${productId}`);
            const reviews = await res.json();

            // Clear list
            reviewList.innerHTML = '';

            if (reviews.length === 0) {
                reviewList.innerHTML = '<div class="no-reviews" id="no-reviews-placeholder">Be the first to review this product!</div>';
                if (countLabel) countLabel.textContent = 'No reviews yet';
                return;
            }

            // Update count
            if (countLabel) {
                countLabel.textContent = reviews.length + ' review' + (reviews.length !== 1 ? 's' : '');
            }

            // Update average stars
            if (avgStarsEl && reviews.length > 0) {
                const avg = reviews.reduce((sum, r) => sum + r.rating, 0) / reviews.length;
                avgStarsEl.innerHTML = renderStars(avg);
            }

            // Render cards
            reviews.forEach(review => {
                const card = document.createElement('div');
                card.className = 'review-card';

                const verifiedHtml = review.isVerifiedPurchase
                    ? '<span class="verified-badge">Verified Purchase</span>'
                    : '';

                card.innerHTML = `
                    <div class="review-card-header">
                        <span class="reviewer-name">${escapeHtml(review.userName)}</span>
                        <div class="review-meta">
                            ${verifiedHtml}
                            <span class="review-date">${escapeHtml(review.createdAt)}</span>
                        </div>
                    </div>
                    <div class="review-stars">${renderStars(review.rating)}</div>
                    ${review.comment ? `<p class="review-comment">${escapeHtml(review.comment)}</p>` : ''}
                `;
                reviewList.appendChild(card);
            });

        } catch (err) {
            console.error('Failed to reload reviews:', err);
        }
    }

})();
