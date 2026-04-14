---
description: Code quality suite for AdminSystem_v2 — build verification, XAML audit, style consistency check, and bug fix workflow
---

# /code-quality-suite Workflow

A structured quality sweep for the **AdminSystem_v2** (and optionally **WebApplication**) project.
Run this suite whenever fixing UI bugs, refactoring styles, or after merging changes.

---

## Phase 1 — Pre-flight: Read the TODO

1. Read both TODO files to understand pending work:
   - `AdminSystem_v2/TODO.md` — security & remediation tasks
   - `TODO.md` (root) — cross-system bug list (sections A, B)
2. Identify which task item(s) you are targeting (e.g., `B.4`, `B.5`).

---

## Phase 2 — Codebase Audit

3. **XAML Style Audit**: Verify that all `TextBox`, `ComboBox`, `DatePicker`, and `PasswordBox` controls use the correct shared styles from `App.xaml`:
   - `TextBox` → `{StaticResource FormTextBox}` (Foreground: `#F9FAFB`, Background: `#111827`)
   - `ComboBox` → `{StaticResource FormComboBox}`
   - Buttons → `{StaticResource PrimaryButton}`, `SecondaryButton`, or `DangerButton`

4. **Foreground / Background Contrast Check**: Scan all Views for any inline `Foreground` or `Background` attributes on TextBox-type controls that deviate from the theme (dark: `#111827`, text: `#F9FAFB`).

5. **Placeholder Pattern Audit**: Ensure all search/filter TextBoxes use the verified overlay-TextBlock pattern (not `TextBox.Resources` style overrides):
   ```xml
   <Grid>
       <TextBox Style="{StaticResource FormTextBox}" Text="{Binding Prop, ...}" />
       <TextBlock Text="Placeholder..." Foreground="#4B5563"
                  IsHitTestVisible="False"
                  Visibility="{Binding IsPropEmpty, Converter={StaticResource BoolToVis}}"/>
   </Grid>
   ```

6. **DatePicker Audit**: Ensure all `DatePicker` controls have:
   - `Foreground="#F9FAFB"` (makes selected-date text visible against dark background)
   - `Background="#111827"`
   - `BorderBrush="#374151"`

---

## Phase 3 — Build Verification

// turbo
7. Build the AdminSystem_v2 project to confirm there are no compile errors:
   ```
   dotnet build "c:\Users\user\Documents\ANDREI_FILES\PDM_FILES\2ND YEAR\2ND SEM\OOP\OOP-TaurusBikeShop\AdminSystem_v2\AdminSystem_v2.csproj" --configuration Debug
   ```

8. Check build output — confirm `0 Error(s)`. If errors exist, fix them before proceeding.

---

## Phase 4 — Apply Fixes

9. Apply XAML/code fixes for the identified TODO items:
   - Fix each affected View file
   - Do **not** modify `App.xaml` shared styles unless strictly necessary — prefer fixing individual Views
   - Keep changes minimal and scoped to the identified issue

10. After each fix, re-run the build (Step 7) to confirm no regression.

---

## Phase 5 — Verification & Walkthrough

11. Create or update `walkthrough.md` in the brain artifacts directory documenting:
    - What was fixed and why
    - Files changed (with diff summaries)
    - Build result confirmation
    - Any remaining manual test steps

12. Mark completed items in the relevant `TODO.md` with `✅ DONE`.
