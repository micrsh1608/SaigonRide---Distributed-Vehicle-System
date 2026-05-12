import codecs

with codecs.open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/Payment.cshtml', 'r', encoding='utf-8-sig') as f:
    content = f.read()

# 1. Add lang.js include
if 'lang.js' not in content:
    content = content.replace('</head>', '    <script src="/Scripts/lang.js"></script>\n</head>')

# 2. Add modern dark mode
dark_mode_css = '''
        /* Dark Mode */
        body.dark-mode {
            --sg-bg: #0f172a;
            --sg-text: #f8fafc;
            background-color: var(--sg-bg);
            color: var(--sg-text);
        }
        body.dark-mode .top-bar {
            background-color: #1e293b;
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.5);
            border-bottom: 1px solid #334155;
        }
        body.dark-mode .top-bar .back-btn { color: #f8fafc; }
        body.dark-mode .top-bar .back-btn:hover { background-color: #334155; }
        body.dark-mode .invoice-card {
            background: #1e293b;
            box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.5);
            border: 1px solid #334155;
        }
        body.dark-mode .invoice-header { border-bottom-color: #334155; }
        body.dark-mode .invoice-row.total { border-top-color: #334155; }
        body.dark-mode .method-title { color: #94a3b8; }
        body.dark-mode .method-option {
            background: #1e293b;
            border-color: #334155;
        }
        body.dark-mode .method-option:hover { border-color: #475569; }
        body.dark-mode .method-option.selected {
            background: rgba(0, 137, 123, 0.15);
            border-color: var(--sg-primary);
        }
        body.dark-mode .method-name { color: #f8fafc; }
        body.dark-mode .method-desc { color: #94a3b8; }
        body.dark-mode .bottom-action {
            background: #1e293b;
            border-top: 1px solid #334155;
            box-shadow: 0 -4px 6px -1px rgba(0, 0, 0, 0.5);
        }
        body.dark-mode .text-dark { color: #f8fafc !important; }
        body.dark-mode .text-muted { color: #94a3b8 !important; }
        body.dark-mode .bg-light { background-color: #334155 !important; }
        body.dark-mode input.form-control {
            background-color: #0f172a;
            border-color: #334155;
            color: #f8fafc;
        }
        body.dark-mode input.form-control:focus {
            background-color: #0f172a;
            color: #f8fafc;
            border-color: var(--sg-primary);
        }
'''
if '/* Dark Mode */' not in content:
    content = content.replace('</style>', dark_mode_css + '\n    </style>')

# 3. Handle i18n
# Replace exact text inside tags
content = content.replace('<title>Saigon EcoRide - Thanh toán chuyến đi</title>', '<title data-i18n="payment_title">Saigon EcoRide - Thanh toán chuyến đi</title>')
content = content.replace('<h1 class="title">Thanh toán chuyến đi</h1>', '<h1 class="title" data-i18n="payment_title">Thanh toán chuyến đi</h1>')
content = content.replace('Mã giao dịch:', '<span data-i18n="invoice_id">Mã giao dịch</span>:')
content = content.replace('Loại xe:', '<span data-i18n="payment_vehicle_type">Loại xe</span>:')
content = content.replace('Thời gian sử dụng:', '<span data-i18n="payment_duration">Thời gian sử dụng</span>:')
content = content.replace('Đơn giá:', '<span data-i18n="payment_rate">Đơn giá</span>:')
content = content.replace('Tổng cần thanh toán:', '<span data-i18n="payment_total">Tổng cần thanh toán</span>:')
content = content.replace('Mã giảm giá (Khuyến mãi)', '<span data-i18n="payment_discount">Mã giảm giá (Khuyến mãi)</span>')
content = content.replace('placeholder="Nhập mã..."', 'placeholder="Nhập mã..." data-i18n="promo_code_placeholder"')
content = content.replace('Áp dụng', '<span data-i18n="promo_code_btn">Áp dụng</span>')
content = content.replace('Nguồn tiền thanh toán', '<span data-i18n="payment_method">Nguồn tiền thanh toán</span>')
content = content.replace('Thanh toán tự động qua ứng dụng', '<span data-i18n="linked_status">Thanh toán tự động qua ứng dụng</span>')
content = content.replace('Thanh toán nhanh một chạm', '<span data-i18n="linked_status">Thanh toán nhanh một chạm</span>')
content = content.replace('Dành cho du khách (For Foreigners)', '<span data-i18n="linked_status">Dành cho du khách (For Foreigners)</span>')
content = content.replace('Thanh toán ngay', '<span data-i18n="payment_btn_pay">Thanh toán ngay</span>')
content = content.replace('Đang kết nối cổng thanh toán...', '<span data-i18n="payment_processing">Đang kết nối cổng thanh toán...</span>')

# 4. Handle Linked Accounts Javascript Injection
js_setup = '''
        // Setup lang and dark mode initially
        if (localStorage.getItem('dark_mode') === 'true') {
            document.body.classList.add('dark-mode');
        }

        let linkedAccounts = { momo: null, zalo: null, visa: null };
        
        function updateMethodUI() {
            ['momo', 'zalo', 'visa'].forEach(m => {
                let descEl = document.getElementById(m + '-desc');
                if (descEl) {
                    if (linkedAccounts[m]) {
                        descEl.innerHTML = <span class="text-success"><i class="fas fa-check-circle me-1"></i> ()</span>;
                    } else {
                        descEl.innerHTML = <span class="text-muted"></span>;
                    }
                }
            });
        }

        function loadLinkedAccounts() {
            fetch('/Account/GetLinkedAccounts')
                .then(r => r.json())
                .then(d => {
                    if (d.success) {
                        linkedAccounts.momo = d.momo;
                        linkedAccounts.zalo = d.zalo;
                        linkedAccounts.visa = d.visa;
                        updateMethodUI();
                    }
                });
        }
'''
if 'let linkedAccounts' not in content:
    content = content.replace('// Khởi tạo dữ liệu từ LocalStorage', js_setup + '\n        // Khởi tạo dữ liệu từ LocalStorage')
    
    # We also need to add IDs to method descriptions to be updated by updateMethodUI
    content = content.replace('<p class="method-desc">Thanh toán tự động qua ứng dụng</p>', '<p class="method-desc" id="momo-desc">Thanh toán tự động qua ứng dụng</p>')
    content = content.replace('<p class="method-desc">Thanh toán nhanh một chạm</p>', '<p class="method-desc" id="zalo-desc">Thanh toán nhanh một chạm</p>')
    content = content.replace('<p class="method-desc">Dành cho du khách (For Foreigners)</p>', '<p class="method-desc" id="visa-desc">Dành cho du khách (For Foreigners)</p>')

    # Hook loadLinkedAccounts
    content = content.replace('window.onload = function () {', 'window.onload = function () {\n            loadLinkedAccounts();')

# 5. Handle Linking account on click processPayment
js_payment_injection = '''
            if (selectedMethod !== 'wallet' && !linkedAccounts[selectedMethod]) {
                alert(t('link_account_required'));
                // Prompt to link
                let accountInfo = prompt(t('link_account_desc'));
                if (accountInfo) {
                    fetch('/Account/LinkAccount', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        body: 'method=' + selectedMethod + '&accountInfo=' + encodeURIComponent(accountInfo)
                    }).then(r => r.json()).then(d => {
                        if (d.success) {
                            alert(t('link_account_success'));
                            linkedAccounts[selectedMethod] = accountInfo;
                            updateMethodUI();
                        }
                    });
                }
                document.getElementById('loading-overlay').style.display = 'none';
                return;
            }
'''
if "if (selectedMethod !== 'wallet' && !linkedAccounts[selectedMethod])" not in content:
    content = content.replace("const startStationId = localStorage.getItem('start_station_id') || 0;", js_payment_injection + "\n            const startStationId = localStorage.getItem('start_station_id') || 0;")

# 6. Replace alerts with i18n
content = content.replace('alert("Số dư ví Saigon EcoRide không đủ. Vui lòng chọn phương thức khác (MoMo/Zalo/Visa)!");', 'alert(t("alert_insufficient_wallet"));')
content = content.replace('alert("Thanh toán thành công! Bạn vừa tích lũy thêm được " + earnedPoints + " điểm xếp hạng.");', 'alert(t("alert_payment_success").replace("{0}", earnedPoints));')
content = content.replace('alert("Lỗi từ CSDL: " + data.message);', 'alert(t("alert_db_error").replace("{0}", data.message));')
content = content.replace('alert("Lỗi kết nối Server! Vui lòng thử lại.");', 'alert(t("alert_conn_error"));')

with codecs.open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/Payment.cshtml', 'w', encoding='utf-8') as f:
    f.write(content)

print("Payment.cshtml updated safely.")
