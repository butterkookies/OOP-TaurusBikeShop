/**
 * order-history.js — client-side filtering for the Order History page.
 * Filters rows by status using data-status attributes and by order number
 * using a search input.
 */

(function () {
    'use strict';

    var currentStatus = 'all';
    var currentSearch = '';

    function filterRows() {
        var rows = document.querySelectorAll('#order-rows tr[data-status]');
        var noResultsEl = document.getElementById('no-orders-msg');
        var visibleCount = 0;

        rows.forEach(function (row) {
            var rowStatus = (row.dataset.status || '').toLowerCase();
            var rowNumber = (row.dataset.orderNumber || row.textContent || '').toLowerCase();

            var statusMatch = currentStatus === 'all' || rowStatus === currentStatus.toLowerCase();
            var searchMatch = !currentSearch || rowNumber.indexOf(currentSearch) !== -1;

            if (statusMatch && searchMatch) {
                row.style.display = '';
                visibleCount++;
            } else {
                row.style.display = 'none';
            }
        });

        if (noResultsEl) {
            noResultsEl.style.display = visibleCount === 0 ? '' : 'none';
        }
    }

    function setActiveFilter(btn) {
        document.querySelectorAll('.order-filter-btn').forEach(function (b) {
            b.classList.remove('active');
        });
        btn.classList.add('active');
    }

    function init() {
        // Status filter buttons
        document.querySelectorAll('.order-filter-btn').forEach(function (btn) {
            btn.addEventListener('click', function () {
                currentStatus = this.dataset.status || 'all';
                setActiveFilter(this);
                filterRows();
            });
        });

        // Order number search
        var searchInput = document.getElementById('order-search');
        if (searchInput) {
            searchInput.addEventListener('input', function () {
                currentSearch = this.value.trim().toLowerCase();
                filterRows();
            });

            searchInput.addEventListener('keydown', function (e) {
                if (e.key === 'Escape') {
                    this.value = '';
                    currentSearch = '';
                    filterRows();
                }
            });
        }

        filterRows();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
