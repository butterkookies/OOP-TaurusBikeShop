Here is your **updated ready-to-run Claude Code prompt** with the payment expansion properly integrated and engineered cleanly:

---

# 🧠 CLAUDE CODE PROMPT — POS SYSTEM FOR TAURUS BIKE SHOP

You are a senior full-stack engineer and software architect working on an existing enterprise project called **Taurus Bike Shop Admin System + WebApplication** built with ASP.NET Core MVC, EF Core, and a layered architecture.

---

## 🎯 TASK

Implement a **new Point of Sale (POS) module** inside the Admin System as a fully functional feature.

This POS system must be integrated properly into the existing codebase without breaking current functionality.

---

# 📍 INTEGRATION REQUIREMENTS

## 1. Sidebar Integration

* Add a new sidebar tab in the Admin System:

  * Name: **POS**
  * Icon: appropriate (cash register / cart)
* Must follow existing sidebar UI patterns and styling.

---

## 2. Architecture Rules (STRICT)

Follow the existing project structure:

* Controllers must be thin
* Business logic must go into Services (create `POSService`)
* Use ViewModels for POS UI data
* Reuse existing:

  * Product model
  * Customer model
  * Order system (if applicable)
  * Inventory system

❌ Do NOT duplicate order logic
❌ Do NOT break WebApplication checkout flow
❌ Do NOT modify unrelated modules unless required

---

## 3. POS CORE FEATURES

### 🛒 Sales Workflow

Implement a fast cashier-style POS flow:

* Product search (fast lookup by name / SKU / barcode if available)
* Add product to cart
* Update quantity
* Remove items
* Cart summary panel

---

### 👤 Customer Handling

* Select existing customer OR use “Walk-in Customer”
* Must integrate with existing Customer table

---

## 💳 4. PAYMENT PROCESSING (UPDATED REQUIREMENTS)

Support multiple payment methods for POS transactions:

### ✅ Currently ACTIVE payment methods:

* Cash
* GCash
* Bank Transfer

### 🧪 ADDITIONAL PAYMENT METHOD (PLANNED / NOT YET ACTIVE LOGIC):

* Card Payment (Credit/Debit)

⚠️ Important behavior for Card Payment:

* Must be INCLUDED in the system design (enum/model/UI/support structure)
* Must be selectable in UI
* Must be fully wired in architecture
* BUT:

  * ❌ No processing logic required yet
  * ❌ No validation or gateway integration yet
  * It is a **placeholder/disabled-ready feature**

You must design it so it can be enabled later with minimal changes.

---

### 💰 Payment Calculation Requirements:

System must compute:

* Subtotal
* Discounts (if applicable)
* Total
* Cash received (only for cash payments)
* Change (only for cash payments)

For non-cash payments:

* Skip cash/change logic safely

---

## 📦 5. INVENTORY HANDLING

* Deduct stock ONLY after successful transaction
* Prevent checkout if stock is insufficient
* Ensure inventory consistency with existing system

---

## 🧾 6. ORDER CREATION

* POS checkout must create an Order record compatible with existing order system
* Must NOT conflict with WebApplication checkout logic
* Must support all payment types including the new Card placeholder

---

## 🧾 7. RECEIPT GENERATION

Create printable receipt view including:

* Order ID
* Items purchased
* Quantities
* Prices
* Payment method
* Total breakdown
* Date/time
* Cashier name

---

## 🔒 8. TRANSACTION SAFETY (CRITICAL)

Use database transaction scope to ensure atomicity:

* Order creation
* Inventory deduction
* Payment finalization

If any step fails → rollback everything.

---

## 🎨 9. UI/UX REQUIREMENTS

* Dark theme consistent with Admin System
* Fast cashier-friendly workflow (minimal clicks)
* Clean cart layout
* Fast product search experience
* Clear payment method selector including Card (disabled-ready visually or logically)

---

## 📁 10. FILES TO CREATE / MODIFY

You are expected to:

* Create `POSController`
* Create `POSService`
* Create POS ViewModels
* Create POS Views (Razor)
* Modify sidebar navigation
* Extend payment method enum/model safely
* Add only necessary database changes (if required)

---

## ⚡ 11. PERFORMANCE REQUIREMENTS

* Product search must be optimized (fast queries, no full table loads)
* Efficient filtering and indexing assumptions
* POS must feel instant (retail-grade responsiveness)

---

## 🎯 FINAL GOAL

Deliver a production-ready POS module that:

* Works like a real retail system
* Integrates seamlessly with Taurus Bike Shop architecture
* Supports multiple payment methods including future-ready Card support
* Does NOT break existing checkout or order systems
* Is clean, scalable, and maintainable

---
