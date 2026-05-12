import re
with open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/Payment.cshtml', 'r', encoding='utf-8') as f:
    content = f.read()

# 1. Dark mode styles
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

# Replace hardcoded text with data-i18n
content = content.replace('Thanh toAn chuyn i', 'Payment')
content = content.replace('<h1 class="title">Thanh toAn</h1>', '<h1 class="title" data-i18n="payment_title">Thanh toán</h1>')
content = content.replace('M HA3A ? N:', '<span data-i18n="invoice_id">Mã hóa đơn</span>:')
content = content.replace('>Loi xe<', ' data-i18n="payment_vehicle_type">Loại xe<')
content = content.replace('>Th?i gian thuA<', ' data-i18n="payment_duration">Thời gian thuê<')
content = content.replace('>GiA c>c<', ' data-i18n="payment_rate">Giá cước<')
content = content.replace('>T ng cTng<', ' data-i18n="payment_total">Tổng cộng<')
content = content.replace('>ThAnh ti?n<', ' data-i18n="payment_final">Thành tiền<')
content = content.replace('>Phng thcc thanh toAn<', ' data-i18n="payment_method">Phương thức thanh toán<')

# Promo code
content = content.replace('placeholder="Nh-p mA khuyn mAi..."', 'placeholder="Nhập mã khuyến mãi..." data-i18n="promo_code_placeholder"')
content = content.replace('>A?p dng<', ' data-i18n="promo_code_btn">Áp dụng<')

# Payment methods texts
content = content.replace('VA- Saigon EcoRide', '<span data-i18n="payment_wallet">Ví Saigon EcoRide</span>')

# Modify Payment Options logic to show Linked Account status
# We will inject some JS in the window.onload and fetch accounts
script_injection = '''
        // Setup lang and dark mode initially
        let currentLang = localStorage.getItem("saigonride_lang") || "vi";
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

content = content.replace('// KhYi to d_ liu t LocalStorage', script_injection + '\n        // KhYi to d_ liu t LocalStorage')

# Fix Vietnamese encoding artifacts
content = content.replace('A', 'á')
content = content.replace('<', 'ị')
content = content.replace('A?p dng', 'Áp dụng')
content = content.replace('ThAnh ti?n', 'Thành tiền')
content = content.replace('T ng cTng', 'Tổng cộng')
content = content.replace('GiA c>c', 'Giá cước')
content = content.replace('Th?i gian thuA', 'Thời gian thuê')
content = content.replace('Loi xe', 'Loại xe')
content = content.replace('M HA3A ? N', 'Mã hóa đơn')

# Update logic in processPayment
process_payment_injection = '''
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
                return;
            }
'''
content = content.replace("const startStationId = localStorage.getItem('start_station_id') || 0;", process_payment_injection + "\n            const startStationId = localStorage.getItem('start_station_id') || 0;")

# Make sure lang.js is imported
if 'lang.js' not in content:
    content = content.replace('</head>', '    <script src="/Scripts/lang.js"></script>\n</head>')

with open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/Payment.cshtml', 'w', encoding='utf-8') as f:
    f.write(content)
print("Updated Payment.cshtml")
