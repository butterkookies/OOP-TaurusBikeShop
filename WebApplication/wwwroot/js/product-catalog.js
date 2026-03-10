/* ═══════════════════════════════════════════════════════════════
   TAURUS BIKE SHOP — PRODUCT CATALOG JAVASCRIPT
   File:    wwwroot/js/product-catalog.js
   Used by: Views/Customer/ProductCatalog.cshtml

   Features:
   - Category tab filtering (All/Mountain/Road/Fixie/Kids/Others)
   - Part-type dropdown filter
   - Live search (name, brand, part type)
   - Sort (default / price asc / price desc / name A-Z)
   - Wishlist heart toggle
   - Add to Cart green feedback
═══════════════════════════════════════════════════════════════ */

(function () {

    var grid    = document.getElementById('catalogGrid');
    var cards   = Array.from(grid.querySelectorAll('.product-card'));
    var search  = document.getElementById('catalogSearch');
    var ptSel   = document.getElementById('partTypeSelect');
    var sortSel = document.getElementById('catalogSort');
    var countEl = document.getElementById('countNum');
    var emptyEl = document.getElementById('catalogEmpty');
    var tabs    = document.querySelectorAll('.filter-tab');
    var activeCat = 'all';

    /* ── Count badges on tabs ── */
    ['all','mountain','road','fixie','kids','others'].forEach(function(cat) {
        var el = document.getElementById('cnt-' + cat);
        if (!el) return;
        el.textContent = cat === 'all'
            ? cards.length
            : cards.filter(function(c) { return c.dataset.category === cat; }).length;
    });

    /* ── Filter + Sort engine ── */
    function applyFilters() {
        var q     = search.value.toLowerCase().trim();
        var pt    = ptSel.value;
        var sort  = sortSel.value;

        var visible = cards.filter(function(c) {
            var okCat  = activeCat === 'all' || c.dataset.category === activeCat;
            var okPart = pt === 'all' || c.dataset.parttype === pt;
            var okQ    = !q || c.dataset.name.includes(q) || c.dataset.brand.includes(q)
                            || c.dataset.parttype.toLowerCase().includes(q);
            return okCat && okPart && okQ;
        });

        visible.sort(function(a, b) {
            if (sort === 'price-asc')  return +a.dataset.price - +b.dataset.price;
            if (sort === 'price-desc') return +b.dataset.price - +a.dataset.price;
            if (sort === 'name-asc')   return a.dataset.name.localeCompare(b.dataset.name);
            return 0;
        });

        cards.forEach(function(c) { c.style.display = 'none'; });
        visible.forEach(function(c, i) {
            c.style.display = 'flex';
            c.style.order   = i;
            c.style.animation = 'none';
            void c.offsetWidth;
            c.style.animation      = 'fadeUp 0.4s cubic-bezier(0.16,1,0.3,1) both';
            c.style.animationDelay = (i * 0.02) + 's';
        });

        countEl.textContent = visible.length;
        emptyEl.classList.toggle('visible', visible.length === 0);
    }

    /* ── Tab click ── */
    tabs.forEach(function(tab) {
        tab.addEventListener('click', function() {
            tabs.forEach(function(t) { t.classList.remove('active'); });
            this.classList.add('active');
            activeCat = this.dataset.cat;
            ptSel.value = 'all';
            applyFilters();
        });
    });

    search.addEventListener('input', applyFilters);
    ptSel.addEventListener('change', applyFilters);
    sortSel.addEventListener('change', applyFilters);

    /* ── Wishlist toggle ── */
    grid.addEventListener('click', function(e) {
        var w = e.target.closest('.card-wish');
        if (w) { e.stopPropagation(); w.classList.toggle('active'); }
    });

    /* ── Add to Cart feedback ── */
    grid.addEventListener('click', function(e) {
        var btn = e.target.closest('.btn-cart');
        if (!btn || btn.dataset.loading) return;
        btn.dataset.loading = '1';
        var orig = btn.innerHTML;
        btn.innerHTML = '<span>Added &#10003;</span>';
        btn.style.background = '#22C55E';
        setTimeout(function() {
            btn.innerHTML = orig;
            btn.style.background = '';
            delete btn.dataset.loading;
        }, 1800);
    });

    /* ── Init ── */
    applyFilters();

}());

