**PROMPT — BATCH 2 BUG FIXES (WEB APP + ADMIN SYSTEM V2)**

You are tasked with diagnosing and fixing the following issues across the Web Application (frontend + UX) and Admin System V2 (frontend + backend integration). Implement fixes with clean separation of concerns, consistent UI/UX behavior, and validated backend logic. Avoid regressions.

---

## **A. WEB APPLICATION (USER SIDE)**

### **1. Review Layout Structure**

* Issue: Review components are poorly structured.
* Required Fix:

  * Reorder layout strictly as:

    * **Top:** Customer Name
    * **Below:** Star Rating
    * **Below:** Review Comment
    * **Bottom:** Date Posted
  * Ensure consistent spacing and alignment across all review cards.

---

### **2. Wishlist Card Alignment**

* Issue: “Add to Cart” buttons are vertically inconsistent due to variable content height (e.g., product name length).
* Required Fix:

  * Enforce uniform card height across all wishlist items.
  * Anchor “Add to Cart” button to a fixed vertical position (e.g., bottom-aligned).
  * Use flex/grid layout to stabilize spacing regardless of content length.

---

### **3. Product Image Sizing**

* Issue: Product images are inconsistent in size and alignment.
* Required Fix:

  * Apply uniform width/height constraints to all product images.
  * Maintain aspect ratio (use object-fit: cover/contain as appropriate).
  * Prevent overflow or stretching.

---

### **4. Buy Now Button (Product Detail Page)**

* Issue: No direct purchase option exists.
* Required Fix:

  * Add a **“Buy Now”** button on product detail page.
  * Behavior:

    * On click → prompt user:

      * Option A: Proceed directly to checkout with selected product.
      * Option B: Continue browsing (redirect to catalog).
  * Ensure clean UX flow without breaking cart logic.

---

### **5. Search Bar + Filter Layout**

* Issue: Search bar and filters are oversized and disrupt layout.
* Required Fix:

  * Compress all filters into a **single horizontal line**.
  * Replace expanded filter lists with:

    * **Category → ComboBox (Dropdown)**
    * **Brand → ComboBox (Dropdown)**
  * Price Range:

    * Min + Max inputs inline in the same row.
  * Ensure responsive behavior and proper spacing.

---

## **B. ADMIN SYSTEM V2**

### **6. Password Error UI (Role Update)**

* Issue: When incorrect password is entered, error message breaks button layout (buttons split horizontally).
* Required Fix:

  * Display error message without altering button structure.
  * Maintain button integrity (no resizing, splitting, or overflow).
  * Use proper container spacing instead of layout-breaking inserts.

---

### **7. Voucher Dropdown — Default Display**

* Issue: Dropdown shows raw model output (e.g., `AdminSystemV2.Models.VoucherListItem`).
* Required Fix:

  * Replace with user-friendly default label:

    * “Select Voucher” or “Choose Voucher”
  * Ensure proper binding of display values.

---

### **8. Voucher Dropdown — Data Presentation**

* Issue: Dropdown only shows voucher ID/code.
* Required Fix:

  * Each dropdown option must display structured, readable data in one line:

    * Voucher Code
    * Description
    * Max Usage / Total Uses
    * (Optional: Expiry Date, Status)
  * Format clearly (e.g., column-like or spaced inline format).
  * Ensure readability and no UI overflow.

---

### **9. Voucher Assignment Failure (Critical Backend Issue)**

* Issue: Assigning voucher to customer fails with error:

  * “Assignment failed. Check your database connection.”
* Required Fix:

  * Investigate backend flow:

    * Database connection string validity
    * API/service call handling
    * Transaction execution and commit
  * Validate:

    * Voucher exists
    * Customer exists
    * No constraint violations
  * Ensure:

    * Proper error handling (no generic DB error unless true)
    * Success response triggers email/notification correctly
  * Log detailed error for debugging.

---

## **GENERAL REQUIREMENTS**

* Maintain strict frontend-backend consistency.
* Apply validation on both client and server sides.
* Ensure UI responsiveness and visual stability.
* Prevent layout shifts during error states.
* Test all flows:

  * Edge cases
  * Invalid inputs
  * Real user scenarios

---

**EXPECTED OUTPUT FROM YOU (CLAUDE CODE):**

* Root cause analysis per issue
* Exact code-level fixes (frontend + backend)
* Refactored snippets where necessary
* Brief explanation per fix (no verbosity)
