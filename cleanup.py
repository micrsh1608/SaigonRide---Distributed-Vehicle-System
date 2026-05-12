import os

files = [
    'C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/Payment.cshtml',
    'C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/UserDashboard.cshtml',
    'C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Scripts/lang.js'
]

for file_path in files:
    with open(file_path, 'r', encoding='utf-8', errors='ignore') as f:
        content = f.read()
    
    # Remove the replacement character
    content = content.replace('\ufffd', '')
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(content)

print("Cleaned up unicode replacement characters.")
