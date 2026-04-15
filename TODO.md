Use this prompt. It is structured to force deep inspection, strict criticism, and alignment with your actual system.

---

**PROMPT:**

You are a senior database architect and software auditor. Your task is to perform a strict, production-level review of a SQL schema and identify all structural, logical, and implementation flaws.

### Context

* The system is a full-stack application (Admin + Customer system).
* Tech stack includes .NET (WPF + Web), likely using SQL Server.
* The schema is intended for real-world production use (not academic).
* The system includes features like orders, users, payments, inventory, and possibly delivery vs pickup logic.

### Task Instructions

1. **Load and Analyze Schema**

   * Read the SQL file from this relative path:

     ```
     SQL\Schema\Taurus_schema_8.1.sql
     ```
   * Parse all tables, constraints, relationships, indexes, and data types.

2. **Strict Structural Review**
   Identify and explain:

   * Missing or incorrect primary keys
   * Foreign key issues (missing, incorrect references, cascade problems)
   * Improper normalization (1NF, 2NF, 3NF violations)
   * Redundant or duplicated data
   * Inconsistent naming conventions
   * Bad data types (e.g., using NVARCHAR where INT/DATE is appropriate)
   * Nullability issues (columns that should/shouldn’t allow NULL)

3. **Logic and Business Rule Validation**
   Evaluate whether the schema properly supports:

   * Separation or unification of Admin vs Customer users
   * Order flows (online delivery vs in-store pickup)
   * Payment methods (e.g., GCash, bank transfer, cash)
   * Inventory tracking and stock consistency
   * Audit/history tracking where needed

   Identify:

   * Missing tables or relationships
   * Incorrect assumptions in schema design
   * Logical inconsistencies that will break real system behavior

4. **Security Review**

   * Detect unsafe practices (e.g., storing plaintext passwords)
   * Evaluate use of sensitive fields
   * Suggest improvements (hashing, salting, secrets handling)

5. **Performance Review**

   * Missing indexes on frequently queried fields
   * Over-indexing
   * Inefficient relationships or table structures
   * Potential query bottlenecks

6. **Scalability Assessment**

   * Will this schema scale with thousands/millions of records?
   * Identify design decisions that will fail under load

7. **Real System Reflection**

   * Based on the schema, infer how the actual system behaves
   * Point out mismatches between expected system behavior and schema design
   * Example: If delivery vs pickup exists in UI but not properly modeled in DB, flag it

8. **Output Format**
   Structure your response as:

   * **Critical Issues (Must Fix)**
   * **Major Design Flaws**
   * **Minor Issues / Improvements**
   * **Security Risks**
   * **Performance Concerns**
   * **Scalability Risks**
   * **Mismatch Between Schema and Real System**
   * **Recommended Refactored Design (if necessary)**

9. **Tone Requirement**

   * Be brutally honest
   * Do not soften criticism
   * Treat this as a production system that could fail

---

This forces the AI to behave like a real auditor instead of giving surface-level feedback.
