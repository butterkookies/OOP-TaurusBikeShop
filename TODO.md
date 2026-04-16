Use this. It is tightened based on the actual failure patterns in your files (constraint drift, audit fields, insert order, filtered indexes).

---

**PROMPT FOR CLAUDE OPUS 4.6**

You are a senior SQL Server database engineer specializing in schema evolution, migrations, and deterministic data seeding.

I will provide three SQL files:

1. Seed script
   `SQL\Seed\Taurus_seed_v7.1.sql`

2. Base schema
   `SQL\Schema\Taurus_schema_8.1.sql`

3. Patch / migration
   `SQL\Schema\Taurus_schema_8.2_audit_fixes.sql`

---

## Objective

Rewrite the seed script so it is **fully compatible with the FINAL schema state after applying 8.1 + 8.2**.

The current seed is outdated and WILL FAIL due to:

* Column mismatches
* New NOT NULL constraints
* Foreign key dependencies
* Audit-related fields introduced in 8.2
* Filtered indexes requiring proper SET options

---

## Required Process (Do NOT skip steps)

### 1. Reconstruct Final Schema

* Parse 8.1 schema
* Apply 8.2 patch logically
* Build the exact final structure:

  * Tables
  * Columns (including added/removed/renamed)
  * Data types
  * PK, FK, UNIQUE, CHECK constraints
  * Identity columns
  * Default constraints
  * Filtered indexes and their conditions

---

### 2. Deep Analysis of Seed Script

Analyze `Taurus_seed_v7.1.sql` and detect ALL incompatibilities:

* Invalid or removed columns
* Missing required columns (especially NOT NULL)
* Foreign key violations
* Insert order violations (child before parent)
* Duplicate key risks
* Identity misuse (missing or incorrect `IDENTITY_INSERT`)
* Missing audit fields (e.g., created_at, updated_at, created_by, status, etc.)
* Violations caused by filtered indexes

---

### 3. Produce Structured Audit Report

Output:

**A. Schema Changes Impacting Seed**

* List only changes from 8.2 that break the seed

**B. Table-by-Table Issues**
For each table:

* Problem
* Root cause
* Required fix

---

### 4. Generate Fully Corrected Seed Script (PRIMARY OUTPUT)

Rewrite the ENTIRE seed script.

Strict requirements:

* Must run on a clean DB after:

  1. `Taurus_schema_8.1.sql`
  2. `Taurus_schema_8.2_audit_fixes.sql`

* Must execute with **ZERO ERRORS**

---

## Critical Constraints for the New Seed

### A. Correct Insert Order (MANDATORY)

Order inserts based on dependency graph:

1. Lookup tables (roles, categories, statuses)
2. Parent tables (users, suppliers)
3. Core entities (products)
4. Transaction tables (orders)
5. Child tables (order_items, logs, audit tables)

---

### B. Column Alignment

* Every INSERT must explicitly declare columns
* Must match FINAL schema exactly
* No missing NOT NULL fields

---

### C. Audit Field Compliance

* Populate all required audit fields introduced in 8.2
* Use consistent values (e.g., GETDATE(), system user IDs)

---

### D. Identity Handling

* Prefer natural identity generation (no hardcoded IDs)
* If explicit IDs are required:

  * Use `SET IDENTITY_INSERT ON/OFF` correctly

---

### E. Constraint Safety

* All FK references must point to valid inserted rows
* No duplicate values for UNIQUE constraints
* Respect CHECK constraints and filtered index conditions

---

### F. Required SET Options (MANDATORY AT TOP)

Include:

```
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_WARNINGS ON
```

---

### G. Deterministic Execution

* Script must be re-runnable ONLY on a clean DB
* No reliance on pre-existing data

---

## Output Format

1. Summary of breaking schema changes
2. Table-by-table issue breakdown
3. **Final corrected seed SQL (clean, ordered, executable)**

---

## Final Requirement

Do not give advice. Do not explain theory.

Perform full reconciliation and output a **production-quality corrected seed script** that will run successfully on the updated schema.
