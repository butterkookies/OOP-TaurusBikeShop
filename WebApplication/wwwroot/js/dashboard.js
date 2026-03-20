/* ============================================================
   DASHBOARD.JS — Customer dashboard interactions
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

    // ── Order tracking poll ────────────────────────────────
    // Polls every 60s if there is an active tracking badge on page
    var trackingBadge = document.querySelector('[data-tracking-poll]');
    if (trackingBadge) {
        setInterval(function () {
            var orderId = trackingBadge.dataset.orderId;
            if (!orderId) return;
            fetch('/Order/StatusPoll?orderId=' + orderId)
                .then(function (r) { return r.json(); })
                .then(function (res) {
                    if (res.success && res.data && res.data.status) {
                        trackingBadge.textContent = res.data.status;
                    }
                }).catch(function () {});
        }, 60000);
    }

    // ── Notification dropdown ──────────────────────────────
    var bellBtn      = document.getElementById('notifBellBtn');
    var notifDropdown = document.getElementById('notifDropdown');

    if (bellBtn && notifDropdown) {
        bellBtn.addEventListener('click', function (e) {
            e.stopPropagation();
            notifDropdown.classList.toggle('show');
        });

        document.addEventListener('click', function () {
            notifDropdown.classList.remove('show');
        });
    }

    // ── Support ticket form char counter ──────────────────
    var msgTextarea = document.getElementById('ticketMessage');
    var charCounter = document.getElementById('msgCharCount');
    if (msgTextarea && charCounter) {
        msgTextarea.addEventListener('input', function () {
            charCounter.textContent = msgTextarea.value.length;
        });
    }
});
