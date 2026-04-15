AdminSystem_v2 – Orders Module Fix List
	1.	Status Progression Lock (Non-Reversible)

	•	Enforce one-directional status flow (e.g., Pending → Processing → Completed).
	•	Disallow reverting to any previous status (e.g., Processing → Pending = blocked).
	•	Treat terminal states (e.g., Cancelled, Completed) as permanent.

	2.	Automatic Validation on Update

	•	On every status change, validate against allowed forward transitions.
	•	Reject invalid transitions at backend level (not UI-only).

	3.	Disable Invalid Status Actions (UI Guardrail)

	•	Disable all buttons representing previous statuses once advanced.
	•	Disable all actions if order is in a terminal state.
	•	Only allow the immediate next valid status.

	4.	Clear Error Handling

	•	Return explicit system message on invalid attempts (e.g., “Reverting status is not allowed”).
	•	Log rejected transitions for audit.

	5.	Order Type Segregation (Delivery vs Pickup)

	•	Separate views or tabs:
	•	Delivery Orders
	•	In-Store Pickup Orders
	•	Ensure filtering is persistent and clear to admin.
	•	Prevent visual mixing of order types.

	6.	Admin Workflow Optimization

	•	Add labels/tags per order type (Delivery / Pickup) visible in list.
	•	Ensure POS view aligns with pickup flow only.

	7.	Payment Column Fix

	•	Replace generic “Online” with structured display:
	•	Format: Online > GCash
	•	Format: Online > Bank Transfer
	•	Ensure mapping reflects actual payment method used.
	•	Normalize values from database (no inconsistent strings).

	8.	Data Consistency Enforcement

	•	Standardize enums/values for:
	•	Order Status
	•	Order Type (Delivery / Pickup)
	•	Payment Method (GCash, Bank Transfer, etc.)
	•	Prevent free-text inconsistencies.

	9.	UI Layout Stability

	•	Ensure columns (Status, Payment, Order Type) do not overlap or truncate.
	•	Maintain consistent alignment and spacing across Orders table.

	10.	Audit Trail (Optional but Recommended)

	•	Track status changes with timestamp and admin ID.
	•	Useful for debugging and accountability.