/**
 * =========================================================================
 * INTERACTIVE BACKGROUND CANVAS
 * =========================================================================
 * A lightweight, adjustable particle network that moves organically and 
 * reacts to the user's mouse cursor. Automatically syncs with dark mode.
 */

// ==========================================
// 🛠️ TWEAKABLE SETTINGS
// ==========================================
const BG_CONFIG = {
    particleCount: 50,           // Number of geometric objects floating
    baseSpeed: 0.5,              // Base moving speed
    particleSizeMin: 2,          // Minimum dot radius
    particleSizeMax: 5,          // Maximum dot radius
    connexionDistance: 160,      // Max distance to draw connecting lines
    mouseInteractionRadius: 200, // Radius for mouse repulsion
    mouseRepelForce: 5,          // How fast they flee the cursor
    mouseRestoringForce: 0.02,   // How quickly they return to normal speed

    // Theme Colors
    colorLight: {
        dot: 'rgba(34, 42, 53, 0.3)',  // Charcoal, slight opacity
        line: 'rgba(34, 42, 53, 0.1)'
    },
    colorDark: {
        dot: 'rgba(255, 255, 255, 0.2)', // White, slight opacity
        line: 'rgba(255, 255, 255, 0.06)'
    }
};
// ==========================================

(function initInteractiveBackground() {
    // 1. Create and inject the Canvas element
    const canvas = document.createElement('canvas');
    canvas.id = 'bg-canvas';
    // Positioned strictly in the background (-z-10), spans full screen, ignores clicks
    canvas.className = 'fixed top-0 left-0 w-full h-full -z-10 pointer-events-none transition-opacity duration-1000 opacity-0';
    document.body.prepend(canvas);

    // Fade in effect
    setTimeout(() => canvas.classList.remove('opacity-0'), 100);

    const ctx = canvas.getContext('2d');
    let width, height;
    let particles = [];

    // Mouse tracking from the window
    let mouse = { x: null, y: null };
    window.addEventListener('mousemove', (e) => {
        mouse.x = e.clientX;
        mouse.y = e.clientY;
    });
    window.addEventListener('mouseleave', () => {
        mouse.x = null;
        mouse.y = null;
    });

    // Handle resizing
    function resize() {
        width = window.innerWidth;
        height = window.innerHeight;
        // Fix scaling for high DPI displays (Retina)
        const dpr = window.devicePixelRatio || 1;
        canvas.width = width * dpr;
        canvas.height = height * dpr;
        ctx.scale(dpr, dpr);
    }
    window.addEventListener('resize', resize);
    resize();

    // Utility: is dark mode active?
    function isDark() {
        return document.documentElement.classList.contains('dark');
    }

    // Particle Class
    class Particle {
        constructor() {
            this.x = Math.random() * width;
            this.y = Math.random() * height;
            // Native velocities
            this.vx = (Math.random() - 0.5) * BG_CONFIG.baseSpeed;
            this.vy = (Math.random() - 0.5) * BG_CONFIG.baseSpeed;

            // External force (applied by mouse)
            this.fx = 0;
            this.fy = 0;

            this.radius = Math.random() * (BG_CONFIG.particleSizeMax - BG_CONFIG.particleSizeMin) + BG_CONFIG.particleSizeMin;
        }

        update() {
            // Apply mouse repulsion if active
            if (mouse.x != null && mouse.y != null) {
                const dx = this.x - mouse.x;
                const dy = this.y - mouse.y;
                const distance = Math.sqrt(dx * dx + dy * dy);

                if (distance < BG_CONFIG.mouseInteractionRadius) {
                    // Force strength degrades as distance increases
                    const force = (BG_CONFIG.mouseInteractionRadius - distance) / BG_CONFIG.mouseInteractionRadius;
                    this.fx = (dx / distance) * force * BG_CONFIG.mouseRepelForce;
                    this.fy = (dy / distance) * force * BG_CONFIG.mouseRepelForce;
                }
            }

            // Bleed out external forces quickly (friction restoring them to 0)
            this.fx += (0 - this.fx) * BG_CONFIG.mouseRestoringForce;
            this.fy += (0 - this.fy) * BG_CONFIG.mouseRestoringForce;

            // Update position
            this.x += this.vx + this.fx;
            this.y += this.vy + this.fy;

            // Bounce off walls exactly
            if (this.x < 0 || this.x > width) this.vx *= -1;
            if (this.y < 0 || this.y > height) this.vy *= -1;

            // Constraint failsafes (if they violently shoot past borders)
            if (this.x < 0) this.x = 0;
            if (this.x > width) this.x = width;
            if (this.y < 0) this.y = 0;
            if (this.y > height) this.y = height;
        }

        draw() {
            ctx.beginPath();
            ctx.arc(this.x, this.y, this.radius, 0, Math.PI * 2);
            ctx.fillStyle = isDark() ? BG_CONFIG.colorDark.dot : BG_CONFIG.colorLight.dot;
            ctx.fill();
        }
    }

    // Initialize particles
    for (let i = 0; i < BG_CONFIG.particleCount; i++) {
        particles.push(new Particle());
    }

    // Animation Loop
    function animate() {
        ctx.clearRect(0, 0, width, height);

        const dark = isDark();

        for (let i = 0; i < particles.length; i++) {
            particles[i].update();
            particles[i].draw();

            // Draw connecting lines
            for (let j = i + 1; j < particles.length; j++) {
                const dx = particles[i].x - particles[j].x;
                const dy = particles[i].y - particles[j].y;
                const dist = Math.sqrt(dx * dx + dy * dy);

                if (dist < BG_CONFIG.connexionDistance) {
                    ctx.beginPath();
                    // Fade lines out automatically based on distance scaling
                    const opacityRatio = 1 - (dist / BG_CONFIG.connexionDistance);

                    // Parse rgba string and inject calculated alpha
                    const baseColor = dark ? BG_CONFIG.colorDark.line : BG_CONFIG.colorLight.line;
                    const parsedAlpha = parseFloat(baseColor.split(',')[3]) * opacityRatio;
                    const finalColor = dark
                        ? `rgba(255, 255, 255, ${parsedAlpha})`
                        : `rgba(34, 42, 53, ${parsedAlpha})`;

                    ctx.strokeStyle = finalColor;
                    ctx.lineWidth = 1;
                    ctx.moveTo(particles[i].x, particles[i].y);
                    ctx.lineTo(particles[j].x, particles[j].y);
                    ctx.stroke();
                }
            }
        }

        requestAnimationFrame(animate);
    }

    // Start engine
    animate();
})();
