Good call—here’s a **refined, model-optimized version** of your prompt, specifically tuned for **Claude / GPT / Gemini**, plus **auto-ERD generation (Mermaid)** so you can directly render diagrams.

---

# **Prompt: System-Accurate Documentation Generator (Optimized Version)**

## **Context**

You are given the **full codebase** of the *Taurus Bike Shop Ordering System*, including:

* Frontend code
* Backend logic
* Database schema (`.sql`)

Your task is to generate **accurate, implementation-based documentation** that strictly reflects the **actual system**, not assumptions.

---

## **Critical Rules (NON-NEGOTIABLE)**

* Base everything on the **actual codebase and schema**
* ❌ Do NOT assume missing features
* ❌ Do NOT generate generic/template answers
* ❌ Do NOT fabricate relationships or logic

If something is missing:

> Explicitly say: **"Not found in the current codebase"**

---

## **Execution Strategy (IMPORTANT FOR MODEL BEHAVIOR)**

### **Step 1 — Deep Analysis**

* Parse all `.sql` files → extract:

  * Tables
  * Columns
  * Keys (PK, FK)
* Scan backend:

  * Controllers / services
  * Database queries
* Identify:

  * Real workflows
  * Actual system features

---

## **Step 2 — Generate Documentation**

---

### **1. Database Design (ACTUAL)**

* Explain:

  * Each table’s real purpose
  * Relationships based on foreign keys
  * How the system uses the database in real flows

---

### **2. ERD (AUTO-GENERATED – MERMAID)**

Generate a **valid Mermaid ER Diagram**:

```
erDiagram
    TABLE_NAME {
        int id PK
        varchar name
    }

    TABLE_A ||--o{ TABLE_B : has
```

**Rules:**

* Must compile in Mermaid (no syntax errors)
* Include ALL tables
* Reflect REAL relationships only
* Use correct cardinality

---

### **3. Data Dictionary (STRICT)**

For EACH table:

| Column Name | Data Type | Constraints | Description |
| ----------- | --------- | ----------- | ----------- |

Descriptions must be based on **actual usage in code**, not guesses.

---

### **4. System Capabilities (REAL ONLY)**

List features that are:

* Actually implemented
* Observable in code

Example format:

* “Processes customer orders via [OrderService]”
* “Stores transaction data in [Orders table]”

---

### **5. System Limitations (CODE-BASED)**

Identify:

* Missing features
* Hardcoded logic
* Lack of validation
* UI/backend constraints

Must come from **real observations**

---

### **6. Unique Features (REAL DIFFERENTIATORS)**

Only include features that:

* Exist in the system
* Are somewhat uncommon

---

### **7. SQL Command Extraction (REAL QUERIES)**

Extract actual queries from code:

* `SELECT`
* `INSERT`
* `UPDATE`
* `DELETE`
* `JOIN`

Provide:

* Real query
* Short explanation of its role in the system

---

### **8. System Flow / Storyboard**

Describe REAL flow:

Example:

1. User logs in
2. System validates credentials
3. Redirect to dashboard
4. User creates order
5. Order saved to database

Must match actual logic.

---

### **9. Tech Stack (AUTO-DETECTED)**

List:

* Languages (e.g., C#)
* Frameworks (e.g., WPF)
* Database (MySQL)
* Tools (Git, GitHub)

---

## **Output Format Requirements**

* Use **clean headings**
* Use **tables where needed**
* Keep it **formal and documentation-ready**
* Ensure **consistency with your case study format**

---

## **Model-Specific Optimization**

### **For Claude**

* Be extremely detailed in reasoning
* Double-check relationships before output
* Prefer correctness over verbosity

### **For GPT**

* Keep structure strict and organized
* Avoid hallucination by re-checking schema references

### **For Gemini**

* Focus on structured outputs
* Ensure Mermaid diagram is clean and valid

---

## **Final Goal**

Produce documentation that:

* Matches the **actual system behavior**
* Aligns perfectly with the **database**
* Can be used directly in a **capstone/case study submission**

---

If you want next, I can:

* 🔧 Add **auto-generated screenshots mapping (based on UI code)**
* 🧠 Or create a **grading-rubric optimized version (pang prof-ready)**
