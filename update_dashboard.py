import re

with open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/UserDashboard.cshtml', 'r', encoding='utf-8') as f:
    content = f.read()

# Replace the existing body.dark-mode block
# We use regex to find body.dark-mode block up to the end of the <style>
pattern = re.compile(r'body\.dark-mode\s*\{.*?(?=\s*</style>)', re.DOTALL)

modern_dark_mode_css = '''body.dark-mode {
            background-color: #0f172a !important; /* slate-900 */
            color: #f8fafc !important; /* slate-50 */
        }
        
        body.dark-mode .sidebar {
            background-color: #1e293b !important; /* slate-800 */
            border-right: 1px solid #334155 !important;
        }

        body.dark-mode .topbar {
            background-color: rgba(30, 41, 59, 0.85) !important;
            backdrop-filter: blur(10px);
            border-bottom: 1px solid #334155 !important;
            box-shadow: 0 4px 6px -1px rgba(0,0,0,0.5) !important;
        }

        body.dark-mode .card,
        body.dark-mode .station-card,
        body.dark-mode .trip-status-card,
        body.dark-mode .wallet-card,
        body.dark-mode .bg-white,
        body.dark-mode .offcanvas {
            background-color: #1e293b !important;
            border: 1px solid #334155 !important;
            box-shadow: 0 10px 15px -3px rgba(0,0,0,0.5) !important;
        }

        body.dark-mode .text-dark,
        body.dark-mode h1, body.dark-mode h2, body.dark-mode h3, 
        body.dark-mode h4, body.dark-mode h5, body.dark-mode h6,
        body.dark-mode .station-title, body.dark-mode .station-name {
            color: #f8fafc !important;
        }

        body.dark-mode .text-muted,
        body.dark-mode .capacity-label, body.dark-mode .distance-label {
            color: #94a3b8 !important;
        }

        body.dark-mode .btn-light {
            background-color: #334155 !important;
            color: #f8fafc !important;
            border-color: #475569 !important;
        }

        body.dark-mode .btn-light:hover {
            background-color: #475569 !important;
        }

        body.dark-mode .dropdown-menu {
            background-color: #1e293b !important;
            border: 1px solid #334155 !important;
        }

        body.dark-mode .dropdown-item {
            color: #f8fafc !important;
        }

        body.dark-mode .dropdown-item:hover {
            background-color: #334155 !important;
        }

        body.dark-mode .form-control,
        body.dark-mode .form-select {
            background-color: #0f172a !important;
            border-color: #334155 !important;
            color: #f8fafc !important;
        }

        body.dark-mode .form-control:focus,
        body.dark-mode .form-select:focus {
            border-color: #0d6efd !important;
            box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.25) !important;
        }

        body.dark-mode .modal-content {
            background-color: #1e293b !important;
            border: 1px solid #334155 !important;
        }

        body.dark-mode .modal-header {
            border-bottom: 1px solid #334155 !important;
        }

        body.dark-mode .modal-footer {
            border-top: 1px solid #334155 !important;
        }
        
        body.dark-mode .bottom-nav {
            background-color: rgba(30, 41, 59, 0.9) !important;
            backdrop-filter: blur(10px);
            border-top: 1px solid #334155 !important;
        }
        
        body.dark-mode hr {
            border-color: #334155 !important;
        }
        
        /* Linked Accounts in Dark Mode */
        body.dark-mode .linked-account-item {
            background: #0f172a;
            border: 1px solid #334155;
        }
        body.dark-mode .linked-account-item:hover {
            border-color: #475569;
        }'''

content = pattern.sub(modern_dark_mode_css, content)

# Update Wallet tab UI to include Linked Accounts
wallet_html_replacement = '''
                    <div class="card mb-4 border-0 shadow-sm wallet-card">
                        <div class="card-body p-4">
                            <h5 class="fw-bold mb-4" data-i18n="linked_accounts">Tài khoản liên kết</h5>
                            
                            <div class="linked-account-item d-flex justify-content-between align-items-center mb-3 p-3 border rounded" id="momo-link-container">
                                <div>
                                    <img src="https://upload.wikimedia.org/wikipedia/vi/f/fe/MoMo_Logo.png" height="30" class="me-2" />
                                    <span class="fw-bold">Ví MoMo</span>
                                    <div class="small text-muted mt-1" id="momo-status" data-i18n="unlinked_status">Chưa liên kết</div>
                                </div>
                                <button class="btn btn-outline-primary btn-sm" onclick="promptLinkAccount('momo')" id="momo-btn" data-i18n="link_account_btn">Liên kết</button>
                            </div>

                            <div class="linked-account-item d-flex justify-content-between align-items-center mb-3 p-3 border rounded" id="zalo-link-container">
                                <div>
                                    <img src="https://cdn.haitrieu.com/wp-content/uploads/2022/10/Logo-ZaloPay-Square.png" height="30" class="me-2" />
                                    <span class="fw-bold">ZaloPay</span>
                                    <div class="small text-muted mt-1" id="zalo-status" data-i18n="unlinked_status">Chưa liên kết</div>
                                </div>
                                <button class="btn btn-outline-primary btn-sm" onclick="promptLinkAccount('zalo')" id="zalo-btn" data-i18n="link_account_btn">Liên kết</button>
                            </div>

                            <div class="linked-account-item d-flex justify-content-between align-items-center mb-3 p-3 border rounded" id="visa-link-container">
                                <div>
                                    <img src="https://upload.wikimedia.org/wikipedia/commons/4/41/Visa_Logo.png" height="20" class="me-2" />
                                    <span class="fw-bold">Thẻ Visa/Mastercard</span>
                                    <div class="small text-muted mt-1" id="visa-status" data-i18n="unlinked_status">Chưa liên kết</div>
                                </div>
                                <button class="btn btn-outline-primary btn-sm" onclick="promptLinkAccount('visa')" id="visa-btn" data-i18n="link_account_btn">Liên kết</button>
                            </div>
                        </div>
                    </div>
'''
if 'id="momo-link-container"' not in content:
    # Inject it after the transaction history in wallet tab
    # or before it. Let's find "Lịch sử nạp tiền" and inject before it.
    content = content.replace('<h5 class="fw-bold mb-3"><i class="fas fa-history text-muted me-2"></i>', wallet_html_replacement + '\n<h5 class="fw-bold mb-3"><i class="fas fa-history text-muted me-2"></i><span data-i18n="wallet_history">')

# Inject JavaScript logic for Linked Accounts fetching
js_logic = '''
        function loadLinkedAccounts() {
            fetch('/Account/GetLinkedAccounts')
                .then(r => r.json())
                .then(d => {
                    if (d.success) {
                        if (d.momo) {
                            document.getElementById('momo-status').innerHTML = <span class="text-success"><i class="fas fa-check-circle me-1"></i> ()</span>;
                            document.getElementById('momo-btn').style.display = 'none';
                        }
                        if (d.zalo) {
                            document.getElementById('zalo-status').innerHTML = <span class="text-success"><i class="fas fa-check-circle me-1"></i> ()</span>;
                            document.getElementById('zalo-btn').style.display = 'none';
                        }
                        if (d.visa) {
                            document.getElementById('visa-status').innerHTML = <span class="text-success"><i class="fas fa-check-circle me-1"></i> ()</span>;
                            document.getElementById('visa-btn').style.display = 'none';
                        }
                    }
                });
        }

        function promptLinkAccount(method) {
            let info = prompt(t('link_account_desc'));
            if (info) {
                fetch('/Account/LinkAccount', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    body: method=&accountInfo=
                }).then(r => r.json()).then(d => {
                    if (d.success) {
                        alert(t('link_account_success'));
                        loadLinkedAccounts();
                    } else {
                        alert(d.message || t('error_generic'));
                    }
                });
            }
        }
'''
if 'function promptLinkAccount(' not in content:
    content = content.replace('// Khởi tạo các hàm bản đồ khi trang tải', js_logic + '\n// Khởi tạo các hàm bản đồ khi trang tải')
    content = content.replace('loadWalletBalance();', 'loadWalletBalance();\n            loadLinkedAccounts();')

with open('C:/Users/micrsh/source/repos/SaigonBus/SaigonBus/Views/Home/UserDashboard.cshtml', 'w', encoding='utf-8') as f:
    f.write(content)
print("Updated UserDashboard.cshtml")
