Use this prompt for Claude:

---

## PROMPT (Render + ASP.NET production hardening patch)

You are working inside a .NET solution named `OOP-TaurusBikeShop`.

There are multiple projects:

* `WebApplication` → THIS is the only project intended for Render deployment
* `AdminSystem_v2` → IGNORE completely

Your task is to FIX and HARDEN the existing Render deployment configuration in `WebApplication`.

Do NOT restructure the solution. Do NOT add unnecessary packages.

---

# CRITICAL ISSUES TO FIX

## 1. Correct Render PORT binding (fix incorrect implementation)

The current implementation may be incorrect or non-standard.

### REQUIREMENT:

* Use the correct ASP.NET Core approach for Render:

  * Read `PORT` from environment variables
  * Bind using `app.Urls.Add("http://0.0.0.0:{PORT}")`
* Ensure this works ONLY when PORT is set
* Ensure local development still works normally

DO NOT use `--urls` arguments in builder configuration.

---

## 2. Fix configuration key naming (IMPORTANT)

Any configuration keys must be safe for .NET binding.

### REQUIRED FIX:

* Replace any connection string or environment variable keys that contain hyphens (`-`)
* Convert them into valid .NET configuration keys using:

  * PascalCase or camelCase only

Example:

* WRONG: `Taurus-bike-shop-sqlserver-2026`
* CORRECT: `TaurusBikeShopSqlServer2026`

Ensure `appsettings.json`, environment variables usage, and any references are consistent.

---

## 3. Add proper CORS policy (required for future frontend deployment)

Add a safe development + production-ready CORS setup:

### REQUIREMENTS:

* Allow frontend integration from external domains (Render + Vercel/Netlify later)
* Must NOT block API calls in production
* Use a named policy (e.g., "AllowAll" or "CorsPolicy")

Example requirement:

* Allow any origin, header, and method (for case study simplicity)

---

## 4. HTTPS redirection correctness review

Ensure:

* HTTPS redirection does NOT break Render deployment
* It is disabled or safely conditioned for production behind reverse proxy

Fix if necessary:

* Render terminates TLS externally

---

## 5. SendGrid / OTP readiness (configuration safety)

Ensure:

* SendGrid API key is read from environment variables using IConfiguration
* No hardcoded API keys exist
* Add safe placeholder usage if needed

Example:

* `builder.Configuration["SendGridApiKey"]`

---

# SAFETY CONSTRAINTS

* Do NOT modify `AdminSystem_v2`
* Do NOT change solution structure
* Do NOT add unnecessary dependencies
* Keep changes minimal, targeted, production-safe
* Do NOT introduce breaking changes to local development

---

# OUTPUT FORMAT

Provide:

1. Exact code changes (before/after or patch style)
2. Explanation of why each fix is required for Render deployment stability
3. Final checklist confirming:

   * Render compatibility
   * Local dev compatibility
   * API readiness for frontend integration

---

# GOAL

Make `WebApplication`:

* 100% Render deployable
* CORS-safe for frontend integration
* Configuration-safe for production
* Stable for OTP email system (SendGrid)

---

END OF TASK

---
