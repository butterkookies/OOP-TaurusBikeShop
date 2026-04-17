import json
import re
import os
import shutil

md_file = r'c:\Andrei.dev\Projects\OOP-TaurusBikeShop\Documentation\OOP-DB-Document.md'

# Restore from git if changed
os.system(f'git checkout -- "{md_file}"')

# 1. Parse SQL
sql_file = r'c:\Andrei.dev\Projects\OOP-TaurusBikeShop\SQL\Schema\Taurus_schema_10.0_utf8.sql'
with open(sql_file, 'r', encoding='utf-8') as f:
    sql_lines = f.readlines()

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
                    'name': cname,
                    'type': dtype,
                    'nullable': not is_not_null,
                    'is_pk': is_pk,
                    'is_identity': is_ident,
                    'is_computed': is_computed,
                    'is_fk': False,
                    'fk_ref': ''
                }

# Parse basic FK constraints locally for completeness
for l in sql_lines:
    l = l.strip()
    if "FOREIGN KEY(" in l:
        try:
            cl_parts = l.split("] FOREIGN KEY([")
            child_table = cl_parts[0].split("ALTER TABLE [dbo].[")[1].split("]")[0]
            child_col = cl_parts[1].split("])")[0]
            
            ref_parts = l.split("REFERENCES [dbo].[")[1].split("] ([")
            parent_table = ref_parts[0]
            if child_table in sql_tables and child_col in sql_tables[child_table]['columns']:
                sql_tables[child_table]['columns'][child_col]['is_fk'] = True
                sql_tables[child_table]['columns'][child_col]['fk_ref'] = parent_table
        except:
            pass

with open(md_file, 'r', encoding='utf-8') as f:
    md_lines = f.readlines()

md_descriptions = {}
current_md_table = None
in_table = False

for line in md_lines:
    l = line.strip('\r\n')
    if l.startswith("### **") and l.endswith("**"):
        tname = l[6:-2]
        current_md_table = tname
        md_descriptions[tname] = {}
        in_table = False
        continue
        
    if current_md_table and l.startswith("|") and "Column Name" in l:
        in_table = True
        continue
        
    if current_md_table and in_table and l.startswith("|") and "---" not in l:
        parts = [x.strip() for x in l.split("|")[1:-1]]
        if len(parts) >= 4 and parts[0] != "Column Name":
            cname = parts[0]
            desc = parts[3]
            md_descriptions[current_md_table][cname] = desc

output_lines = []
in_dict_section = False
current_md_table = None
skip_table = False

for i, o_line in enumerate(md_lines):
    line = o_line.rstrip('\r\n')
    l = line.strip()
    
    if l == "## **2.3 Data Dictionary (Per Table)**":
        in_dict_section = True
        output_lines.append(line)
        continue
        
    if in_dict_section and l.startswith("## **2.4 Storyboard"):
        for tname in list(sql_tables.keys()):
            if tname == '__EFMigrationsHistory':
                output_lines.append("")
                output_lines.append(f"### **{tname}**")
                output_lines.append("| Column Name | Data Type | Constraints | Description |")
                output_lines.append("|---|---|---|---|")
                for cname, sql_col in sql_tables[tname]['columns'].items():
                    c_type = sql_col['type']
                    c_consts = []
                    if sql_col['is_pk']: c_consts.append("PK")
                    if sql_col['is_fk']: c_consts.append(f"FK \u2192 {sql_col['fk_ref']}")
                    if sql_col['is_identity']: c_consts.append("IDENTITY")
                    c_consts.append("NULLABLE" if sql_col['nullable'] else "NOT NULL")
                    const_str = ", ".join(c_consts)
                    output_lines.append(f"| {cname} | {c_type} | {const_str} | EF Core Migration History |")
        
        in_dict_section = False
        output_lines.append("")
        output_lines.append(line)
        continue
        
    if in_dict_section:
        if l.startswith("### **") and l.endswith("**"):
            tname = l[6:-2]
            current_md_table = tname
            if tname in sql_tables:
                output_lines.append(line)
                output_lines.append("| Column Name | Data Type | Constraints | Description |")
                output_lines.append("|---|---|---|---|")
                
                # Write rows from SQL
                for cname, sql_col in sql_tables[tname]['columns'].items():
                    c_type = sql_col['type']
                    if c_type == "computed":
                        c_type = "computed"
                        
                    c_consts = []
                    if sql_col['is_pk']: c_consts.append("PK")
                    if sql_col['is_fk']: c_consts.append(f"FK \u2192 {sql_col['fk_ref']}") # Right arrow
                    if sql_col['is_identity']: c_consts.append("IDENTITY")
                    c_consts.append("NULLABLE" if sql_col['nullable'] else "NOT NULL")
                    const_str = ", ".join(c_consts)
                    
                    desc = md_descriptions.get(tname, {}).get(cname, "")
                    if not desc:
                        if c_type == "computed":
                            desc = "Computed Column"
                        else:
                            desc = "(Missing description)"
                            
                    output_lines.append(f"| {cname} | {c_type} | {const_str} | {desc} |")
                
                skip_table = True # skip reading the old table lines
                
            else:
                # Table not in SQL?? Just keep it
                output_lines.append(line)
                skip_table = False
            continue
            
        if skip_table and l.startswith("|"):
            continue # ignore old table rows
        if skip_table and not l.startswith("|") and not l == "":
            skip_table = False
            output_lines.append(line)
            continue
            
        if skip_table and l == "":
            output_lines.append(line)
            continue
            
        if not skip_table:
            output_lines.append(line)
    else:
        output_lines.append(line)

# Since git restore could have changed md_lines count, make sure output is single spaced
with open(md_file, 'w', encoding='utf-8') as f:
    f.write("\n".join(output_lines) + "\n")
