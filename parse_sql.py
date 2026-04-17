import re

def convert():
    with open(r'c:\Andrei.dev\Projects\OOP-TaurusBikeShop\SQL\Schema\Taurus_schema_10.0.utf8.sql', 'r', encoding='utf-8') as f:
        sql = f.read()

    # Find table definitions
    table_pattern = re.compile(r'CREATE TABLE \s*\[dbo\]\.\[([a-zA-Z0-9_\-]+)\]\((.*?)\)(?=\s*ON \[PRIMARY\])', re.DOTALL | re.IGNORECASE)

    dbml_lines = []

    for match in table_pattern.finditer(sql):
        table_name = match.group(1)
        body = match.group(2)
        
        dbml_lines.append(f"Table {table_name} {{")
        
        for line in body.split('\n'):
            line = line.strip()
            if not line: continue
            if line.startswith('CONSTRAINT') or line.startswith('UNIQUE') or line.startswith('PRIMARY KEY'): continue
                
            col_match = re.match(r'^\[([a-zA-Z0-9_\-]+)\]\s+\[([a-zA-Z0-9_\-]+)\]\s*(.*)', line)
            if col_match:
                col_name = col_match.group(1)
                col_type = col_match.group(2)
                rest = col_match.group(3).upper()

                if col_type in ("varchar", "nvarchar", "char", "nchar"): col_type = "varchar"
                elif col_type in ("datetime2", "datetime"): col_type = "timestamp"

                settings = []
                if "IDENTITY" in rest: settings.append("primary key")
                if "NOT NULL" in rest: settings.append("not null")
                
                settings_str = " [" + ", ".join(settings) + "]" if settings else ""
                dbml_lines.append(f"  {col_name} {col_type}{settings_str}")

        dbml_lines.append("}\n")
    
    # FK pattern targeting each individual ALTER TABLE line/block without jumping.
    fk_pattern = re.compile(r'ALTER TABLE \s*\[dbo\]\.\[([a-zA-Z0-9_\-]+)\].*?FOREIGN KEY\(\[([a-zA-Z0-9_\-]+)\]\)\s*(?:\r?\n)?.*?REFERENCES \s*\[dbo\]\.\[([a-zA-Z0-9_\-]+)\] \(\[([a-zA-Z0-9_\-]+)\]\)', re.IGNORECASE)
    
    # but wait, the syntax in the file is:
    # ALTER TABLE [dbo].[Table] WITH CHECK ADD CONSTRAINT [FK_Name] FOREIGN KEY([Col])
    # REFERENCES [dbo].[RefTable] ([RefCol])
    # so we need to process it line by line or use a safer regex.
    # let's just find "FOREIGN KEY([Col])\nREFERENCES [dbo].[RefTable] ([RefCol])"
    # and find the preceding "ALTER TABLE [dbo].[Table]"
    
    statements = re.split(r'\bGO\b', sql, flags=re.IGNORECASE)
    seen_fks = set()
    
    for stmt in statements:
        if 'FOREIGN KEY' in stmt:
            t_match = re.search(r'ALTER TABLE \s*\[dbo\]\.\[([a-zA-Z0-9_\-]+)\]', stmt, re.IGNORECASE)
            fk_match = re.search(r'FOREIGN KEY\(\[([a-zA-Z0-9_\-]+)\]\)', stmt, re.IGNORECASE)
            ref_match = re.search(r'REFERENCES \s*\[dbo\]\.\[([a-zA-Z0-9_\-]+)\] \(\[([a-zA-Z0-9_\-]+)\]\)', stmt, re.IGNORECASE)
            
            if t_match and fk_match and ref_match:
                table = t_match.group(1)
                col = fk_match.group(1)
                ref_table = ref_match.group(1)
                ref_col = ref_match.group(2)
                
                fk_str = f"Ref: {table}.{col} > {ref_table}.{ref_col}"
                if fk_str not in seen_fks:
                    dbml_lines.append(fk_str)
                    seen_fks.add(fk_str)
           
    with open(r'C:\Users\Andi\.gemini\antigravity\brain\dfff6fc1-effb-4ecb-8a84-f64d8e543f83\schema.dbml', 'w', encoding='utf-8') as f:
        f.write('\n'.join(dbml_lines))

if __name__ == '__main__':
    convert()
