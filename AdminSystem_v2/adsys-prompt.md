
---

### Voucher Input Component Requirement (C# WPF .NET 8)

**Context:**  
- System: `AdminSystem_v2`  
- Feature: Voucher usage for walk-in customers  
- Environment: POS integration with voucher codes  

**Problem Statement:**  
- Voucher codes are generated and displayed in the POS system.  
- No input field or selection component exists in the WPF interface.  
- Users cannot type or select vouchers, leaving them unable to apply discounts or rewards.  

**Solution Requirement:**  
- Implement a **voucher input component** in the WPF UI.  
- The component must:  
  1. Accept manual text input for voucher codes.  
  2. Provide a dropdown or auto-complete list of available vouchers (if applicable).  
  3. Validate the voucher against the POS voucher database.  
  4. Reflect the corresponding discount or reward immediately upon successful validation.  
  5. Display error feedback if the voucher is invalid, expired, or already used.  

**Expected Behavior:**  
- Walk-in customers can present a voucher.  
- Staff enters or selects the voucher in the new component.  
- The system applies the discount/reward dynamically to the transaction.  
- The UI updates totals and rewards in real time.  

**Technical Notes:**  
- Framework: `.NET 8` with WPF.  
- Component type: `TextBox` + `ComboBox` (hybrid input).  
- Data binding: Connect to voucher repository via POS API.  
- Event handling: `OnVoucherSubmit` → triggers validation + discount application.  

---

This format makes it **actionable**: clear problem, structured solution, and precise technical expectations. Claude Code should now be able to generate the WPF component and bind it properly.
