
---

# Order Window Feedback & Requirements

## Issue 1: Window Rescaling
- **Problem:** Every time the Order window is opened, it must be manually rescaled to the farthest side.  
- **Proposed Solution:**  
  - The order list should automatically expand to the right edge of the screen by default.  
  - Ensure persistent sizing so the window remains maximized without repeated manual adjustment.

---

## Issue 2: Order List View Details
- **Problem:** The current list view lacks sufficient detail. Information is displayed on one side only, making it harder to scan.  
- **Proposed Solution:**  
  - Add additional columns to the list view with the following details:  
    - Number of items in the order  
    - Delivery method (e.g., courier, in-store pickup)  
    - Payment type (e.g., cash, card, online)  
    - Order type (Delivery or Pick-up)  
    - Status (e.g., Pending, Completed, Cancelled)  
  - Display these details per column for better readability and quick reference.

---

## Feature Request 1: Multi-Order Selection & Bulk Status Update
- **Problem:** Currently, there is no way to select multiple orders at once for batch updates.  
- **Proposed Solution:**  
  - Implement a **checkbox selection system** for each row in the order list.  
  - Allow admins to select multiple orders simultaneously.  
  - Provide bulk action options (e.g., change statuses, mark as completed, cancel orders).  
  - Follow familiar UX patterns from platforms like TikTok or Shopee to ensure intuitive use.  

---

## Feature Request 2: Status Badges
- **Problem:** Admins cannot quickly see how many orders fall under each status category.  
- **Proposed Solution:**  
  - Add **badge indicators** showing the count of orders per status (e.g., Pending: 12, Completed: 8, Cancelled: 3).  
  - Place badges prominently at the top of the Order window or within a filter bar.  
  - Ensure badges update dynamically as statuses change.  
  - Use color-coded badges (e.g., yellow for Pending, green for Completed, red for Cancelled) for instant recognition.

---

## Expected Outcome
- **Usability:** Users can open the Order window without resizing it each time.  
- **Clarity:** Orders are displayed with comprehensive details in a structured, column-based format.  
- **Efficiency:** Faster scanning and management of orders due to improved layout and visibility.  
- **Control:** Admins gain the ability to manage multiple orders at once, reducing repetitive actions and improving workflow speed.  
- **Visibility:** Status badges provide at-a-glance insights into order distribution, helping admins prioritize and monitor workload.

---

