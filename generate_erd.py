import json

# Domains
domains = {
    "Identity & Access": {
        "color": "#1E3A5F",
        "icon": "👤",
        "tables": ["User", "Role", "UserRole", "ActiveSession", "OTPVerification", "Address"]
    },
    "Orders & Fulfillment": {
        "color": "#1B5E5A",
        "icon": "📦",
        "tables": ["Order", "OrderItem", "PickupOrder", "OrderStatusAudit", "Delivery", "LBCDelivery", "LalamoveDelivery"]
    },
    "Payments & Refunds": {
        "color": "#2D5016",
        "icon": "💳",
        "tables": ["Payment", "GCashPayment", "BankTransferPayment", "Refund", "StorePaymentAccount", "VoucherUsage"]
    },
    "Product Catalog & Inventory": {
        "color": "#5C1A1A",
        "icon": "🛒",
        "tables": ["Product", "ProductVariant", "ProductImage", "Category", "Brand", "PriceHistory", "InventoryLog", "PurchaseOrder", "PurchaseOrderItem", "Supplier"]
    },
    "Customer Engagement & Support": {
        "color": "#3B4A1A", # mixing Support cyan and Engagement olive using general dark colors based on rules
        "icon": "💬",
        "tables": ["Review", "Wishlist", "Notification", "SupportTicket", "SupportTicketReply", "SupportTask", "Voucher", "UserVoucher"]
    },
    "Session, POS & Audit": {
        "color": "#3A3F5C",
        "icon": "⚙️",
        "tables": ["Cart", "CartItem", "GuestSession", "POS_Session", "SystemLog", "__EFMigrationsHistory"]
    }
}
# Override specific table colors as per prompt
table_colors = {}
for dom, ddata in domains.items():
    for t in ddata["tables"]:
        table_colors[t] = ddata["color"]

table_colors["Review"] = "#3B4A1A"
table_colors["Wishlist"] = "#3B4A1A"
table_colors["Notification"] = "#3B4A1A"
table_colors["Voucher"] = "#4A1A4A"
table_colors["UserVoucher"] = "#4A1A4A"
table_colors["VoucherUsage"] = "#4A1A4A"
table_colors["SupportTicket"] = "#1A4A4A"
table_colors["SupportTicketReply"] = "#1A4A4A"
table_colors["SupportTask"] = "#1A4A4A"
table_colors["SystemLog"] = "#2A2A2A"
table_colors["__EFMigrationsHistory"] = "#2A2A2A"

with open('c:/Andrei.dev/Projects/OOP-TaurusBikeShop/schema2.json', 'r') as f:
    data = json.load(f)

tables = data['tables']
relations = data['relations']

# We need to map all tables inside the domains to their specific tabs.
tabs = {
    "Tab 1: Overview — All Tables": list(tables.keys()),
    "Tab 2: Identity & Access": domains["Identity & Access"]["tables"],
    "Tab 3: Orders & Fulfillment": domains["Orders & Fulfillment"]["tables"],
    "Tab 4: Payments & Refunds": domains["Payments & Refunds"]["tables"],
    "Tab 5: Product Catalog & Inventory": domains["Product Catalog & Inventory"]["tables"],
    "Tab 6: Customer Engagement & Support": domains["Customer Engagement & Support"]["tables"],
    "Tab 7: Session, POS & Audit": domains["Session, POS & Audit"]["tables"],
}

html_out = """<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<title>Taurus Bike Shop \u2014 Database ERD</title>
<style>
  body { font-family: 'Inter', Roboto, Consolas, sans-serif; background: #FFFFFF; color: #222222; margin: 0; padding: 0; }
  .tabs { display: flex; background: #f1f1f1; border-bottom: 2px solid #ccc; overflow-x: auto; }
  .tab-button { background: inherit; border: none; outline: none; cursor: pointer; padding: 14px 16px; transition: 0.3s; font-size: 14px; font-weight: bold; white-space: nowrap; }
  .tab-button:hover { background-color: #ddd; }
  .tab-button.active { background-color: #fff; border-bottom: 2px solid #1E3A5F; color: #1E3A5F; }
  .tab-content { display: none; padding: 20px; width: 100vw; box-sizing: border-box; overflow-x: auto; }
  .tab-content.active { display: block; }
  .header-banner { background: #1E3A5F; color: white; padding: 10px 20px; }
  .header-banner h1 { margin: 0; font-size: 18px; }
  .header-banner p { margin: 5px 0 0; font-size: 12px; }
  .legend { border: 1px solid #ccc; padding: 10px; font-size: 12px; background: #fafafa; margin-top: 20px; width: fit-content; }
  .footer { margin-top: 20px; font-size: 10px; color: #666; text-align: center; }
  rect.er.entityBox { stroke: #CCCCCC !important; stroke-width: 1px !important; }
  text.er.entityLabel { fill: #FFFFFF !important; font-weight: bold !important; font-size: 13px !important; }
"""
for t, c in table_colors.items():
    html_out += f"  g[id^='entity-{t}-'] rect.er.entityBox {{ fill: {c} !important; }}\n"

html_out += """</style>
<script type="module">
  import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.esm.min.mjs';
  mermaid.initialize({ startOnLoad: true, theme: 'default', er: { layoutDirection: 'TB', minEntityWidth: 150, minEntityHeight: 30 } });
</script>
<script>
  function openTab(evt, tabId) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tab-content");
    for (i = 0; i < tabcontent.length; i++) {
      tabcontent[i].style.display = "none";
      tabcontent[i].classList.remove("active");
    }
    tablinks = document.getElementsByClassName("tab-button");
    for (i = 0; i < tablinks.length; i++) {
      tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tabId).style.display = "block";
    document.getElementById(tabId).classList.add("active");
    evt.currentTarget.className += " active";
  }
</script>
</head>
<body>
"""

html_out += '<div class="tabs">\n'
for i, tname in enumerate(tabs.keys()):
    active = ' active' if i == 0 else ''
    html_out += f'  <button class="tab-button{active}" onclick="openTab(event, \'tab{i}\')">{tname}</button>\n'
html_out += '</div>\n\n'

for i, (tname, tlist) in enumerate(tabs.items()):
    active = ' active' if i == 0 else ''
    display = 'block' if i == 0 else 'none'
    
    html_out += f'<div id="tab{i}" class="tab-content" style="display: {display};">\n'
    html_out += f'  <div class="header-banner"><h1>{tname}</h1><p>Taurus Bike Shop v10.0</p></div>\n'
    html_out += '  <div class="mermaid">\n    erDiagram\n'

    # tables
    for tbl in tlist:
        if tbl not in tables: continue
        tdata = tables[tbl]
        # Mermaid icon hack: unfortunately Mermaid ERD can't easily put an icon in the table name, 
        # but we can try putting it in the name text if it renders properly:
        html_out += f'      {tbl} {{\n'
        
        # In overview tab, only show keys
        only_keys = (i == 0)
        
        for col in tdata['columns']:
            if only_keys and not col['is_pk'] and not any(r['child_col'] == col['name'] for r in relations if r['child_table'] == tbl):
                continue
                
            null_badge = "○" if col['nullable'] else "●"
            pk_badge = "🔑 PK" if col['is_pk'] else ""
            fk_badge = "🔗 FK" if any(r['child_col'] == col['name'] for r in relations if r['child_table'] == tbl) else ""
            ai_badge = "AI" if col['is_identity'] else ""
            calc_badge = "ƒ" if col['is_computed'] else ""
            
            badges = " ".join(filter(None, [null_badge, pk_badge, fk_badge, ai_badge, calc_badge]))
            col_type = col['type'].replace(" ", "").replace("(", "-").replace(")", "-") # sanitize type for mermaid parsing
            
            html_out += f'        {col_type} {col["name"]} "{badges}"\n'
        html_out += f'      }}\n'

    # relationships
    # Only draw relationship if both tables are in this tab, 
    # OR if we are not in tab 0 and one table is in this tab (draw stub)
    for r in relations:
        in_tab_child = r['child_table'] in tlist
        in_tab_parent = r['parent_table'] in tlist
        
        if in_tab_child and in_tab_parent:
            # Full relationship line
            # Find nullability of child_col
            c_null = True
            if r['child_table'] in tables:
                for col in tables[r['child_table']]['columns']:
                    if col['name'] == r['child_col']:
                        c_null = col['nullable']
            
            # NOT NULL -> ||--|{ (parent ||, child |{)
            # NULLABLE -> |o--o{
            if c_null:
                rel_marker = "|o--o{"
            else:
                rel_marker = "||--|{"
                
            html_out += f'      {r["parent_table"]} {rel_marker} {r["child_table"]} : "{r["fk_name"]}"\n'
        elif not only_keys:
            # Stubs for foreign tables
            if in_tab_child and not in_tab_parent:
                # Add dummy entity for parent to point to
                c_null = True
                if r['child_table'] in tables:
                    for col in tables[r['child_table']]['columns']:
                        if col['name'] == r['child_col']:
                            c_null = col['nullable']
                rel_marker = "|o--o{" if c_null else "||--|{"
                html_out += f'      {r["parent_table"]} {rel_marker} {r["child_table"]} : "Ext: {r["fk_name"]}"\n'
                
    html_out += '  </div>\n'
    
    html_out += f"""
  <div class="legend">
    <strong>Legend:</strong><br>
    🔑 PK — Primary Key | 🔗 FK — Foreign Key | ◇ UQ — Unique | AI — Auto-Increment | ƒ — Computed | ● — NOT NULL | ○ — Nullable <br>
    ||--|{{ Exactly one to One-or-many | |o--o{{ Zero-or-one to Zero-or-many
  </div>
  <div class="footer">
    Taurus Bike Shop \u2014 Database ERD | v10.0 | Tab {i+1} of {len(tabs)}
  </div>
</div>
"""

html_out += """
<script>
  setTimeout(() => {
    // Attempt alternating row coloring as a hack in SVGs
    // Mermaid doesn't support nth-child easily for this without deep DOM traversal, but we rely on base SVG
  }, 2000);
</script>
</body>
</html>
"""

with open('c:/Andrei.dev/Projects/OOP-TaurusBikeShop/ERD.html', 'w', encoding='utf-8') as f:
    f.write(html_out)
print("ERD.html generated successfully.")
