Use this exact prompt in your code AI (Claude, Copilot, etc.):

---

**PROMPT: FULL TAILWIND MIGRATION + BOOTSTRAP REMOVAL (ASP.NET MVC PROJECT)**

You are working on an ASP.NET MVC project.

Your task is to fully migrate the frontend from Bootstrap to Tailwind CSS and apply a complete redesign.

### 1. Install Tailwind CSS

* Install Tailwind CSS using the proper method for ASP.NET (Node + Tailwind CLI or PostCSS).
* Initialize:

  * `tailwind.config.js`
  * `postcss.config.js` (if needed)
* Configure `content` paths to include:

  ```
  ./Views/**/*.cshtml
  ./wwwroot/**/*.js
  ```
* Create a main Tailwind input CSS file (e.g. `/wwwroot/css/tailwind.css`)
* Add:

  ```
  @tailwind base;
  @tailwind components;
  @tailwind utilities;
  ```
* Build output CSS to:

  ```
  /wwwroot/css/output.css
  ```
* Ensure the layout file references ONLY the compiled Tailwind CSS.

---

### 2. Remove Bootstrap Completely

* Remove all Bootstrap dependencies:

  * Delete Bootstrap CSS and JS files
  * Remove CDN links in `_Layout.cshtml`
* Strip ALL Bootstrap classes from:

  ```
  WebApplication/Views/**/*.cshtml
  ```

  Examples:

  * `container`, `row`, `col-*`
  * `btn`, `btn-primary`, etc.
  * `form-control`, `card`, `navbar`, etc.
* Remove Bootstrap-specific JS behavior and attributes:

  * `data-bs-*`
  * modal, collapse, carousel scripts
* Result: plain, unstyled HTML structure (no design system attached)

---

### 3. Read Design System

* Open and strictly follow:

  ```
  WebApplication/wwwroot/DESIGN.md
  ```
* Extract:

  * Color palette (crimson red theme)
  * Typography (iOS-like + Inter bold for headings)
  * Spacing, radius (rounded iOS style)
  * Components (buttons, cards, navbar, inputs, etc.)
  * Dark mode + light mode behavior

---

### 4. Apply Redesign to ALL Views

Target directory:

```
WebApplication/Views
```

For every `.cshtml` file:

* Rebuild UI using Tailwind utility classes
* Do NOT reuse Bootstrap structure
* Apply:

  * Modern e-commerce layout
  * Clean spacing and hierarchy
  * Rounded corners (2xl where appropriate)
  * Responsive design (mobile-first)

---

### 5. Rebuild Core Components

Recreate these using Tailwind:

* Navbar
* Buttons
* Forms (inputs, labels, validation)
* Cards (product display)
* Tables (if any)
* Modals (use JS if needed, no Bootstrap)

Ensure consistency across all views.

---

### 6. Logo Integration

* Use:

  ```
  WebApplication/wwwroot/FULL-COLOR.png
  ```
* Place in navbar/header
* Ensure proper sizing and alignment
* Maintain brand identity

---

### 7. Dark Mode Support

* Implement Tailwind dark mode (class-based)
* Add toggle (JS if needed)
* Ensure all components support:

  * Light (white background)
  * Dark (black/near-black background)

---

### 8. JavaScript Cleanup and Replacement

* Remove Bootstrap JS entirely
* Replace interactions using:

  * Vanilla JS or Alpine.js (preferred lightweight)
* Rebuild:

  * Navbar toggle (mobile)
  * Modal behavior
  * Dropdowns

---

### 9. Final Requirements

* No Bootstrap remnants should remain
* All pages must be:

  * Consistent
  * Responsive
  * Clean and modern
* Follow DESIGN.md strictly
* Code must be clean and maintainable

---

### 10. Output Expectation

* Updated `_Layout.cshtml`
* Updated all `.cshtml` views
* Tailwind config files
* Clean CSS + JS structure
* Fully working redesigned UI

Do not explain. Execute the transformation.

---
