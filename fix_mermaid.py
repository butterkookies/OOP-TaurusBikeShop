import os

files_to_check = [
    r'c:\Andrei.dev\Projects\OOP-TaurusBikeShop\Documentation\OOP-DB-Document.md',
    r'c:\Andrei.dev\Projects\OOP-TaurusBikeShop\ERD.html'
]

for file_path in files_to_check:
    if os.path.exists(file_path):
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Replace PK_FK with PK, FK
        if 'PK_FK' in content:
            content = content.replace('PK_FK', 'PK, FK')
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            print(f"Fixed PK_FK in {file_path}")
