import json
import re

sql_file = r'c:\Andrei.dev\Projects\OOP-TaurusBikeShop\SQL\Schema\Taurus_schema_10.0_utf8.sql'
with open(sql_file, 'r', encoding='utf-8') as f:
    sql_lines = f.readlines()

# Parse SQL 
sql_tables = {}
current_table = None
for i, line in enumerate(sql_lines):
    l = line.strip()
    if l.startswith("CREATE TABLE [dbo].["):
        tname = l.split("].[")[1].split("]")[0]
        current_table = tname
        sql_tables[tname] = {'columns': {}}
        continue
    
    if current_table and l.startswith(") ON [PRIMARY]"):
        current_table = None
        continue
        
    if current_table:
        if l.startswith("[") and not l.startswith("[PK_") and not l.startswith("CONSTRAINT"):
            if " ASC" in l or " DESC" in l:
                cname = l.split("]")[0][1:]
                if cname in sql_tables[current_table]['columns']:
                    sql_tables[current_table]['columns'][cname]['is_pk'] = True
                continue
                
            cname = l.split("]")[0][1:]
            
            is_computed = False
            if " AS " in l.upper() or " AS(" in l.upper() or " AS (" in l.upper():
                is_computed = True
                
            is_pk = False
            is_ident = "IDENTITY" in l.upper()
            is_not_null = "NOT NULL" in l.upper()
            
            dtype = "computed"
            if not is_computed:
                parts = l.split("] [")
                if len(parts) > 1:
                    type_part = parts[1].split("]")[0]
                    dtype = type_part
                    if type_part + "] (" in l or type_part + "](" in l:
                        try:
                            size = l.split("(")[1].split(")")[0]
                            dtype = f"{type_part}({size})"
                        except:
                            pass
            
            if cname not in sql_tables[current_table]['columns']:
                sql_tables[current_table]['columns'][cname] = {
                    'type': dtype,
                    'nullable': not is_not_null,
                    'is_pk': is_pk,
                    'is_identity': is_ident,
                    'is_computed': is_computed
                }

# Parse MD file
md_file = r'c:\Andrei.dev\Projects\OOP-TaurusBikeShop\Documentation\OOP-DB-Document.md'
with open(md_file, 'r', encoding='utf-8') as f:
    md_lines = f.readlines()

md_tables = {}
current_md_table = None
in_table = False

for line in md_lines:
    l = line.strip()
    # table header usually looks like: ### **TableName**
    if l.startswith("### **") and l.endswith("**"):
        tname = l[6:-2]
        current_md_table = tname
        in_table = False
        continue
        
    if current_md_table and l.startswith("|") and "Column Name" in l:
        md_tables[current_md_table] = {'columns': {}}
        in_table = True
        continue
        
    if current_md_table and in_table and l.startswith("|") and "---" not in l:
        parts = [x.strip() for x in l.split("|")[1:-1]]
        if len(parts) >= 3 and parts[0] != "Column Name":
            cname = parts[0]
            ctype = parts[1]
            cconst = parts[2].upper()
            
            is_pk = "PK" in cconst
            is_ident = "IDENTITY" in cconst
            is_not_null = "NOT NULL" in cconst or "PK" in cconst
            
            nullable = "NULLABLE" in cconst
            if "NULLABLE" not in cconst and "NOT NULL" not in cconst:
                if not is_pk:
                    nullable = True
                else:
                    nullable = False
            
            if "NOT NULL" in cconst:
                nullable = False
                
            md_tables[current_md_table]['columns'][cname] = {
                'type': ctype,
                'nullable': nullable,
                'is_pk': is_pk,
                'is_identity': is_ident
            }

output = []
for tname, sql_data in sql_tables.items():
    if tname not in md_tables:
        output.append(f"[SQL -> MD] Table '{tname}' exists in SQL but is missing from MD Data Dictionary.")
        continue
        
    md_data = md_tables[tname]
    for cname, sql_col in sql_data['columns'].items():
        if cname not in md_data['columns']:
            output.append(f"[SQL -> MD] Table '{tname}': Column '{cname}' is in SQL but missing from MD.")
            continue
            
        md_col = md_data['columns'][cname]
        
        # Check type loosely
        sql_type = sql_col['type'].lower().replace(" ", "")
        md_type = md_col['type'].lower().replace(" ", "").replace(")", "").replace("(", "")
        sql_type_base = sql_type.split("(")[0]
        md_type_base = md_type.split("(")[0]
        
        if sql_col['is_computed']:
            pass
        else:
            if not sql_type_base.startswith(md_type_base) and not md_type_base.startswith(sql_type_base):
                # special case for varchar vs nvarchar or max
                if not (("nvarchar" in sql_type_base and "varchar" in md_type_base) or ("datetime" in sql_type_base and "datetime" in md_type_base)):
                    output.append(f"[SQL -> MD] Table '{tname}', Col '{cname}': Type mismatch. SQL={sql_type}, MD={md_col['type']}")
            
        # Check nullability
        if sql_col['nullable'] != md_col['nullable']:
            output.append(f"[SQL -> MD] Table '{tname}', Col '{cname}': Nullability mismatch. SQL Nullable={sql_col['nullable']}, MD Nullable={md_col['nullable']}")
            
for tname, md_data in md_tables.items():
    if tname not in sql_tables:
        output.append(f"[MD -> SQL] Table '{tname}' is defined in MD but NOT in SQL!")
        continue
        
    sql_data = sql_tables[tname]
    for cname, md_col in md_data['columns'].items():
        if cname not in sql_data['columns']:
            output.append(f"[MD -> SQL] Table '{tname}': Column '{cname}' is in MD but NOT in SQL.")

with open(r'c:\Andrei.dev\Projects\OOP-TaurusBikeShop\audit_report.txt', 'w', encoding='utf-8') as f:
    if not output:
        f.write("Validation Success: The physical SQL schema and the Data Dictionary MATCH completely.")
    else:
        f.write("Discrepancies Found:\n")
        f.write("\n".join(output))
