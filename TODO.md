# Prompt: Fix SELECT * Violations in AdminSystem_v2

**Context:** The Case Study Guidelines strictly prohibit the use of `SELECT *` or `SELECT [alias].*` in SQL queries. 
**Task:** Please audit all repositories in `AdminSystem_v2/Repositories` (especially `ProductRepository.cs`). Find any raw SQL queries or Dapper queries using `SELECT *` or `SELECT p.*` and replace them with explicitly defined column names (e.g., `SELECT p.ProductId, p.CategoryId, p.Name, p.Price...`). 
**Requirement:** Ensure that the explicitly named columns map correctly to the C# model properties so that no data is left unpopulated when Dapper maps the results.
