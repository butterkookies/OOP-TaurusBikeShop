**BATCH 2 — ACTION BUTTONS & STATUS CONTROL ISSUES / REQUIREMENTS**

---

### 1. Pending Tab — Action Buttons Behavior

**Current Issue / Unclear Design:**

* No defined standard for available actions in `Pending` status.
* System behavior for handling unpaid/unconfirmed orders is not clearly enforced.

**Required Clarification / Decision Point:**

* Determine correct action buttons for `Pending`:

  * Option A: No actions allowed (system-controlled only)
  * Option B: Allow `Cancel Order` only (manual override by admin)

**AI Evaluation Task:**

* Determine industry-standard behavior for `Pending` orders in order management systems.
* Validate whether admins should be allowed to manually cancel unpaid orders or if cancellation should be fully automated (e.g., via timeout rules like 24-hour expiry).

---

### 2. Payment Verification — Action Buttons & Visibility

**Required Actions:**

* `Approve Payment` → moves order to `Processing`
* `Hold Payment` → moves order to `On Hold`

**Current Issue / Decision Gap:**

* Uncertainty whether `Cancel Order` should be available in this state.

**AI Evaluation Tasks:**

* Determine if exposing `Cancel Order` during `Payment Verification` is standard practice.
* Evaluate risk of premature cancellation vs maintaining structured payment validation flow.

**Notification Requirement:**

* System must support sending notifications when status changes occur.

**AI Task:**

* Verify capability and best practice for:

  * Email notifications
  * In-app notifications
* Ensure notifications trigger on:

  * Payment approval
  * Payment hold
  * Any status transition

---

### 3. Bulk “Select All” Status Update — Constraint Enforcement

**Current Issue:**

* Bulk status update allows invalid or illogical transitions.
* Example:

  * Orders in `Processing` can be updated to:

    * `Processing` (redundant)
    * `Picked Up` (invalid skip)
    * Other inconsistent states

**Required Behavior:**

* Enforce **strict status transition rules** in dropdown.
* Disable all invalid or illogical target statuses.

**Example Constraint:**

* If current status = `Processing`

  * Allowed: `Ready for Pickup`
  * Disallowed:

    * `Processing` (same state)
    * `Picked Up` (skips step)
    * Any unrelated status

**AI Evaluation Task:**

* Define a **valid state transition map** for all order statuses.
* Ensure:

  * No backward transitions unless explicitly allowed
  * No skipping of required intermediate states
  * No redundant selections

---

### 4. Global Question — Cancel Order Availability

**System-Wide Concern:**

* Whether `Cancel Order` should exist across multiple statuses or be restricted.

**AI Evaluation Task:**

* Determine standard practices for:

  * When cancellation is allowed
  * Which statuses permit cancellation (e.g., Pending, Payment Verification, Processing)
* Identify:

  * Risks of allowing cancellation at later stages
  * Whether cancellation should be replaced with other states (e.g., “Return”, “Failed”, “Rejected”)

---

**Summary of Batch 2 Focus:**

* Define correct action buttons per status
* Enforce strict and logical status transitions
* Standardize cancellation rules
* Ensure notification system integration
* Prevent invalid bulk updates through constraints
