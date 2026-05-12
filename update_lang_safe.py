import codecs

with codecs.open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Scripts/lang.js', 'r', encoding='utf-8-sig') as f:
    content = f.read()

new_vi_keys = '''
        /* Alerts & Payment */
        alert_insufficient_wallet: "Số dư ví Saigon EcoRide không đủ. Vui lòng chọn phương thức khác (MoMo/Zalo/Visa)!",
        alert_payment_success: "Thanh toán thành công! Bạn vừa tích lũy thêm được {0} điểm xếp hạng.",
        alert_db_error: "Lỗi từ CSDL: {0}",
        alert_conn_error: "Lỗi kết nối Server! Vui lòng thử lại.",
        wallet_history: "Lịch sử nạp tiền",
        linked_accounts: "Tài khoản liên kết",
        payment_title: "Thanh toán",
        payment_vehicle_type: "Loại xe",
        payment_duration: "Thời gian thuê",
        payment_rate: "Giá cước",
        payment_total: "Tổng cộng",
        payment_final: "Thành tiền",
        payment_method: "Phương thức thanh toán",
        payment_wallet: "Ví Saigon EcoRide",
        payment_insufficient_balance: "(Không đủ số dư)",
        payment_discount: "Giảm giá",
        payment_btn_pay: "Thanh toán",
        payment_processing: "Đang xử lý thanh toán...",
        payment_success: "Thanh toán thành công!",
        payment_error: "Có lỗi xảy ra khi thanh toán.",
        link_account_btn: "Liên kết",
        link_account_title: "Liên kết tài khoản",
        link_account_desc: "Nhập thông tin tài khoản để liên kết thanh toán.",
        link_account_submit: "Xác nhận liên kết",
        link_account_success: "Liên kết tài khoản thành công!",
        link_account_required: "Bạn cần liên kết tài khoản này trước khi thanh toán.",
        linked_status: "Đã liên kết",
        unlinked_status: "Chưa liên kết",
        promo_code_placeholder: "Nhập mã khuyến mãi...",
        promo_code_btn: "Áp dụng",
        promo_code_applied: "Áp dụng thành công!",
        promo_code_invalid: "Mã khuyến mãi không hợp lệ.",
        invoice_id: "Mã hóa đơn"
'''

new_en_keys = '''
        /* Alerts & Payment */
        alert_insufficient_wallet: "Saigon EcoRide wallet balance is insufficient. Please choose another method (MoMo/Zalo/Visa)!",
        alert_payment_success: "Payment successful! You earned {0} reward points.",
        alert_db_error: "Database error: {0}",
        alert_conn_error: "Server connection error! Please try again.",
        wallet_history: "Top-up History",
        linked_accounts: "Linked Accounts",
        payment_title: "Payment",
        payment_vehicle_type: "Vehicle Type",
        payment_duration: "Duration",
        payment_rate: "Rate",
        payment_total: "Total",
        payment_final: "Final Amount",
        payment_method: "Payment Method",
        payment_wallet: "Saigon EcoRide Wallet",
        payment_insufficient_balance: "(Insufficient balance)",
        payment_discount: "Discount",
        payment_btn_pay: "Pay Now",
        payment_processing: "Processing payment...",
        payment_success: "Payment successful!",
        payment_error: "An error occurred during payment.",
        link_account_btn: "Link",
        link_account_title: "Link Account",
        link_account_desc: "Enter your account information to link.",
        link_account_submit: "Confirm Link",
        link_account_success: "Account linked successfully!",
        link_account_required: "You need to link this account before paying.",
        linked_status: "Linked",
        unlinked_status: "Unlinked",
        promo_code_placeholder: "Enter promo code...",
        promo_code_btn: "Apply",
        promo_code_applied: "Promo code applied!",
        promo_code_invalid: "Invalid promo code.",
        invoice_id: "Invoice ID"
'''

# Insert the keys before the closing bracket of i and en
content = content.replace('nav_tab_account: "Tài khoản",', 'nav_tab_account: "Tài khoản",' + new_vi_keys)
content = content.replace('nav_tab_account: "Account",', 'nav_tab_account: "Account",' + new_en_keys)

with codecs.open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Scripts/lang.js', 'w', encoding='utf-8') as f:
    f.write(content)

print("lang.js updated safely.")
