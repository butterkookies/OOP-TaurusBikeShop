import os

file_path = r'c:\Andrei.dev\Projects\OOP-TaurusBikeShop\Documentation\OOP-DB-Document.md'

if os.path.exists(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Replace PK, FK with PK "FK" to ensure absolute compatibility with all Mermaid versions
    content = content.replace('PK, FK', 'PK "FK"')
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(content)
