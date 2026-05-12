files = [
    'C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/Payment.cshtml',
    'C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/UserDashboard.cshtml',
    'C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Scripts/lang.js'
]
for file_path in files:
    with open(file_path, 'rb') as f:
        content = f.read()
    content = content.replace(b'\xe1\xbb\x8b<', b'<')
    content = content.replace(b'\xe1\xbb\x8b', b'')
    with open(file_path, 'wb') as f:
        f.write(content)
print("Cleaned up.")
