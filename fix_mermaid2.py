import os

file_path = r'c:\Andrei.dev\Projects\OOP-TaurusBikeShop\Documentation\erd_professional.html'

if os.path.exists(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Replace PK_FK with PK, FK
    if 'PK_FK' in content:
        content = content.replace('PK_FK', 'PK, FK')
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"Fixed PK_FK in {file_path}")
    else:
        print(f"No PK_FK found in {file_path}")
