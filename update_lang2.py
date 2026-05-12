import re

with open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Scripts/lang.js', 'r', encoding='utf-8') as f:
    content = f.read()

# Add missing keys for alerts
new_vi_keys = '''
        /* Alerts */
        alert_insufficient_wallet: "Số dư ví Saigon EcoRide không đủ. Vui lòng chọn phương thức khác (MoMo/Zalo/Visa)!",
        alert_payment_success: "Thanh toán thành công! Bạn vừa tích lũy thêm được {0} điểm xếp hạng.",
        alert_db_error: "Lỗi từ CSDL: {0}",
        alert_conn_error: "Lỗi kết nối Server! Vui lòng thử lại.",
        wallet_history: "Lịch sử nạp tiền",
        linked_accounts: "Tài khoản liên kết",
'''

new_en_keys = '''
        /* Alerts */
        alert_insufficient_wallet: "Saigon EcoRide wallet balance is insufficient. Please choose another method (MoMo/Zalo/Visa)!",
        alert_payment_success: "Payment successful! You earned {0} reward points.",
        alert_db_error: "Database error: {0}",
        alert_conn_error: "Server connection error! Please try again.",
        wallet_history: "Top-up History",
        linked_accounts: "Linked Accounts",
'''

if 'alert_insufficient_wallet:' not in content:
    content = content.replace('/* Payment & Linked Accounts */', new_vi_keys + '\n        /* Payment & Linked Accounts */', 1)
    # The second occurrence is for english
    parts = content.split('/* Payment & Linked Accounts */')
    if len(parts) == 3:
        content = parts[0] + '/* Payment & Linked Accounts */' + parts[1] + new_en_keys + '\n        /* Payment & Linked Accounts */' + parts[2]

with open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Scripts/lang.js', 'w', encoding='utf-8') as f:
    f.write(content)
print("Updated lang.js")
