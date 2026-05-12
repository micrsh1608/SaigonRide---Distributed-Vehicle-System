let currentLang = localStorage.getItem("saigonride_lang") || "vi";

const stationNameMap = {
    'Trạm Metro Bến Thành (Quận 1)': 'Ben Thanh Metro Station (District 1)',
    'Trạm Ký túc xá TDTU (Quận 7)': 'TDTU Dormitory Station (District 7)',
    'Trạm Vincom Thảo Điền (TP. Thủ Đức)': 'Vincom Thao Dien Station (Thu Duc City)',
    'Trạm Phố đi bộ Nguyễn Huệ (Quận 1)': 'Nguyen Hue Walking Street Station (District 1)',
    'Trạm Hồ Con Rùa (Quận 3)': 'Turtle Lake Station (District 3)',
    'Trạm Bưu điện Trung tâm (Quận 1)': 'Central Post Office Station (District 1)',
    'Trạm Công viên Lê Văn Tám (Quận 1)': 'Le Van Tam Park Station (District 1)',
    'Trạm Crescent Mall (Quận 7)': 'Crescent Mall Station (District 7)',
    'Trạm SC VivoCity (Quận 7)': 'SC VivoCity Station (District 7)',
    'Trạm Landmark 81 (Bình Thạnh)': 'Landmark 81 Station (Binh Thanh)',
    'Trạm Làng Đại học (TP. Thủ Đức)': 'University Village Station (Thu Duc City)',
    'Trạm Bến xe Miền Đông mới (TP. Thủ Đức)': 'New Eastern Bus Terminal Station (Thu Duc City)',
    'Trạm Sân bay Tân Sơn Nhất (Tân Bình)': 'Tan Son Nhat Airport Station (Tan Binh)',
    'Trạm Chợ Lớn (Quận 5)': 'Cho Lon Station (District 5)',
    'Metro Bến Thành ': 'Ben Thanh Metro',
    'Metro Bến Thành': 'Ben Thanh Metro',
};

function translateStationName(name) {
    const lang = (typeof currentLang !== 'undefined') ? currentLang : 'vi';
    if (lang === 'en') {
        if (stationNameMap[name]) return stationNameMap[name];
        if (stationNameMap[name.trim()]) return stationNameMap[name.trim()];
    }
    return name;
}

const translations = {

    vi: {
        nav_home: "Trang chủ",
        nav_about: "Về chúng tôi",
        nav_support: "Hỗ trợ",

        menu_myaccount: "Tài khoản của tôi",
        menu_profile: "Hồ sơ cá nhân",
        menu_topup: "Nạp tiền ví",
        menu_language: "Ngôn ngữ",
        menu_logout: "Đăng xuất an toàn",

        hero_service: "Dịch vụ 24/7",
        hero_location: "Đang ở TP.HCM",
        hero_title: "Trải nghiệm di chuyển xanh",
        hero_subtitle: "Mở khóa xe đạp, xe điện và scooter nhanh chóng với mã QR chỉ trong 5 giây.",

        btn_rent: "Thuê xe ngay",
        btn_group_rent: "Thuê xe nhóm",
        btn_find_station: "Tìm trạm & Xe",
        btn_topup_more: "Nạp thêm tiền",
        btn_checkout: "Trả xe",

        section_stations: "Trạm đỗ & Xe gần bạn",
        station_near_you: "Gần bạn",
        station_capacity: "Sức chứa",
        station_full_pct: "đầy",
        station_not_found: "Không tìm thấy trạm đỗ.",
        station_view_detail: "Xem chi tiết xe đang đỗ",

        station_empty_title: "Trạm tạm hết xe",
        station_empty_desc: "Hiện chưa có xe khả dụng tại trạm này. Vui lòng thử trạm lân cận hoặc quay lại sau.",
        station_find_other: "Tìm trạm khác",

        vtype_bike: "Xe đạp",
        vtype_ebike: "Xe điện",
        vtype_scooter: "Scooter",
        vtype_manual_note: "Xe đạp cơ — không cần sạc",

        bat_good: "Tốt",
        bat_mid: "Trung bình",
        bat_low: "Yếu",
        btn_rent_vehicle: "Thuê",
        btn_reserve: "Giữ chỗ",

        wallet_balance: "SỐ DƯ KHẢ DỤNG",

        status_title: "Trạng thái thuê xe",
        status_no_trip: "Chưa có chuyến đi",
        status_scan_qr: "Quét mã QR tại trạm để bắt đầu.",

        map_you_are_here: "Bạn đang ở đây",
        map_vehicles: "xe",
        map_choose_vehicle: "Chọn xe tại trạm này",
        map_no_vehicle: "Không có xe khả dụng",
        map_route_label: "Chuyến đi dự kiến",
        map_distance: "Khoảng cách",

        notif_discount: "Ưu đãi 15% — trạm gần hết xe",
        notif_navigate: "Dẫn đường",
        notif_empty: "Hiện chưa có ưu đãi",

        weather_rain_alert: "Trời đang mưa — hãy cân nhắc trước khi thuê xe!",

        dist_label: "Khoảng cách",
        per_minute: "đ/phút",
        loading: "Đang tải...",
        error_generic: "Đã có lỗi xảy ra. Vui lòng thử lại.",

        nav_tab_home: "Trang chủ",
        nav_tab_history: "Lịch sử",
        nav_tab_wallet: "Ví tiền",
        nav_tab_account: "Tài khoản",
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


        admin_nav_home: "Trang chủ",
        admin_nav_about: "Về chúng tôi",
        admin_nav_contact: "Liên hệ hỗ trợ",
        admin_switch_account: "Chuyển tài khoản",
        admin_section_title: "Phân hệ Quản lý cốt lõi",

        admin_vehicle_title: "Quản lý Phương tiện",
        admin_vehicle_desc: "Quản lý danh sách xe đạp thường, xe đạp điện và scooter. Theo dõi tình trạng bảo trì.",
        admin_station_title: "Quản lý Trạm đỗ",
        admin_station_desc: "Giám sát sức chứa và kiểm soát tình trạng kho bãi (trạm đầy, trạm cạn).",
        admin_rental_title: "Giao dịch Thuê xe",
        admin_rental_desc: "Quản lý quy trình nhận/trả xe của cá nhân. Theo dõi doanh thu và lịch sử di chuyển.",
        admin_customer_title: "Quản lý Khách hàng",
        admin_customer_desc: "Quản lý tài khoản, lịch sử nạp tiền và điểm thưởng của khách hàng địa phương/du khách.",
        admin_group_title: "Giao dịch Thuê Nhóm",
        admin_group_desc: "Theo dõi và xử lý các cuốc xe hội nhóm (Một tài khoản đại diện thuê nhiều xe cùng lúc).",
        admin_support_title: "Yêu cầu Hỗ trợ",
        admin_support_desc: "Xử lý các yêu cầu từ khách hàng, phản hồi lỗi xe hỏng, khiếu nại cước phí và sự cố.",
        admin_weather_title: "Thời tiết & GPS",
        admin_weather_desc: "Xem thời tiết realtime theo vị trí GPS. Dự báo theo giờ, dự báo 7 ngày và chỉ số chất lượng không khí AQI.",
        admin_btn_access: "Truy cập phân hệ",
        admin_btn_weather: "Xem thời tiết",
        admin_footer_text: "Đồ án Phát triển phần mềm MVC - Nhóm SaigonRide",
        search_placeholder: "Tìm kiếm trạm đỗ (VD: Bến Thành)...",
    },

    en: {
        /* Navigation */
        nav_home: "Home",
        nav_about: "About Us",
        nav_support: "Support",

        /* User menu */
        menu_myaccount: "My Account",
        menu_profile: "My Profile",
        menu_topup: "Top Up Wallet",
        menu_language: "Language",
        menu_logout: "Safe Logout",

        /* Hero section */
        hero_service: "24/7 Service",
        hero_location: "In Ho Chi Minh City",
        hero_title: "Experience Green Mobility",
        hero_subtitle: "Unlock bicycles, e-bikes and scooters with QR code in just 5 seconds.",

        /* Buttons */
        btn_rent: "Rent Now",
        btn_group_rent: "Group Rental",
        btn_find_station: "Find Stations",
        btn_topup_more: "Add Funds",
        btn_checkout: "Return Vehicle",

        /* Stations section */
        section_stations: "Nearby Stations & Vehicles",
        station_near_you: "Near you",
        station_capacity: "Capacity",
        station_full_pct: "full",
        station_not_found: "No stations found.",
        station_view_detail: "View parked vehicles",

        /* Station detail modal */
        station_empty_title: "No vehicles available",
        station_empty_desc: "No vehicles are currently available at this station. Try a nearby station or check back later.",
        station_find_other: "Find another station",

        /* Vehicle types */
        vtype_bike: "Bicycle",
        vtype_ebike: "E-Bike",
        vtype_scooter: "Scooter",
        vtype_manual_note: "Manual bike — no charging needed",

        /* Battery */
        bat_good: "Good",
        bat_mid: "Fair",
        bat_low: "Low",

        /* Vehicle actions */
        btn_rent_vehicle: "Rent",
        btn_reserve: "Reserve",

        /* Wallet */
        wallet_balance: "AVAILABLE BALANCE",

        /* Rental status */
        status_title: "Rental Status",
        status_no_trip: "No active trip",
        status_scan_qr: "Scan QR code at a station to get started.",

        /* Map popup */
        map_you_are_here: "You are here",
        map_vehicles: "vehicles",
        map_choose_vehicle: "Choose a vehicle at this station",
        map_no_vehicle: "No vehicles available",
        map_route_label: "Estimated route",
        map_distance: "Distance",

        /* Notifications / incentives */
        notif_discount: "15% off — station running low",
        notif_navigate: "Navigate",
        notif_empty: "No promotions available",

        /* Weather */
        weather_rain_alert: "It's raining — consider your trip before renting!",

        /* Misc */
        dist_label: "Distance",
        per_minute: "/min",
        loading: "Loading...",
        error_generic: "Something went wrong. Please try again.",

        nav_tab_home: "Home",
        nav_tab_history: "History",
        nav_tab_wallet: "Wallet",
        nav_tab_account: "Account",
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


        /* ── Admin Dashboard ── */
        admin_nav_home: "Home",
        admin_nav_about: "About Us",
        admin_nav_contact: "Contact Support",
        admin_switch_account: "Switch Account",
        admin_section_title: "Core Management Modules",

        admin_vehicle_title: "Vehicle Management",
        admin_vehicle_desc: "Manage the list of regular bikes, e-bikes and scooters. Track maintenance status.",
        admin_station_title: "Station Management",
        admin_station_desc: "Monitor capacity and control station status (full or empty).",
        admin_rental_title: "Rental Transactions",
        admin_rental_desc: "Manage individual vehicle pickup/return process. Track revenue and travel history.",
        admin_customer_title: "Customer Management",
        admin_customer_desc: "Manage accounts, top-up history and reward points for local customers and tourists.",
        admin_group_title: "Group Rental Transactions",
        admin_group_desc: "Track and process group rentals (one account renting multiple vehicles at once).",
        admin_support_title: "Support Requests",
        admin_support_desc: "Handle customer requests, broken vehicle reports, fee complaints and incidents.",
        admin_weather_title: "Weather & GPS",
        admin_weather_desc: "View real-time weather by GPS location. Hourly forecast, 7-day forecast and AQI index.",
        admin_btn_access: "Access Module",
        admin_btn_weather: "View Weather",
        admin_footer_text: "MVC Software Development Project - SaigonRide Team",
        search_placeholder: "Search stations (e.g. Ben Thanh)...",
    }
};

function t(key) {
    const lang = (typeof currentLang !== 'undefined') ? currentLang : 'vi';
    const dict = translations[lang] || translations.vi;
    return dict[key] || key;
}

function renderStationsList() {
    const filter = document.getElementById('station-search').value.toLowerCase().trim();
    const listObj = document.getElementById('stations-render-area');
    listObj.innerHTML = '';

    let filtered = stationsData.filter(s => s.name.toLowerCase().includes(filter));

    if (filtered.length === 0) {
        listObj.innerHTML = `
            <div class="p-5 text-center text-muted border rounded-4 bg-white">
                <i class="fas fa-search-minus fs-1 mb-3 opacity-25"></i><br>
                ${t('station_not_found')}
            </div>`;
        return;
    }

    if (filter.length > 0 && filtered.length > 0 && typeof map !== 'undefined') {
        const target = filtered[0];
        if (target.lat && target.lng) {
            focusOnMap(target.lat, target.lng);
            stationMarkers.forEach(marker => {
                const ll = marker.getLatLng();
                if (Math.abs(ll.lat - target.lat) < 0.0001 &&
                    Math.abs(ll.lng - target.lng) < 0.0001) {
                    setTimeout(() => marker.openPopup(), 400);
                }
            });
        }
    } else if (filter.length === 0 && typeof focusOnMap === 'function') {
        focusOnMap(10.7769, 106.7009);
        if (window.stationMarkers) stationMarkers.forEach((_, idx) => _resetMarker(idx));
    }

    filtered.forEach(s => {
        let counts = { bike: 0, ebike: 0, scooter: 0 };
        s.vehicles.forEach(v => counts[v.type] = (counts[v.type] || 0) + 1);

        const fillPct = s.capacity > 0 ? Math.round((s.currentCount / s.capacity) * 100) : 0;
        const fillColor = fillPct >= 50 ? 'bg-success' : fillPct >= 20 ? 'bg-warning' : 'bg-danger';
        const isMatch = filter && s.name.toLowerCase().includes(filter);
        const displayName = translateStationName(s.name);

        listObj.insertAdjacentHTML('beforeend', `
            <div class="station-card-list${isMatch ? ' station-card-highlight' : ''}">
                <div class="station-header d-flex justify-content-between align-items-start">
                    <div>
                        <h5 class="fw-bold m-0 text-primary">${_highlightText(displayName, filter)}</h5>
                        <span class="text-danger fw-bold small">
                            <i class="fas fa-location-arrow"></i> ${s.dist || t('station_near_you')}
                        </span>
                    </div>
                    <div class="text-end">
                        <div class="fw-bold text-dark" style="font-size:0.85rem;">
                            ${t('station_capacity')}: ${s.currentCount}/${s.capacity}
                        </div>
                        <div class="progress mt-1" style="height:6px;width:80px;margin-left:auto;">
                            <div class="progress-bar ${fillColor}" style="width:${fillPct}%"></div>
                        </div>
                        <div style="font-size:0.7rem;color:#6b7280;margin-top:2px;">
                            ${fillPct}% ${t('station_full_pct')}
                        </div>
                    </div>
                </div>
                <div class="vehicle-type-grid">
                    <div class="vehicle-type-item ${counts.bike > 0 ? 'has-bike' : ''}">
                        <i class="fas fa-bicycle text-success"></i>
                        <div class="v-count">${counts.bike}</div>
                    </div>
                    <div class="vehicle-type-item ${counts.ebike > 0 ? 'has-bike' : ''}">
                        <i class="fas fa-bolt text-warning"></i>
                        <div class="v-count">${counts.ebike}</div>
                    </div>
                    <div class="vehicle-type-item ${counts.scooter > 0 ? 'has-bike' : ''}">
                        <i class="fas fa-motorcycle text-primary"></i>
                        <div class="v-count">${counts.scooter}</div>
                    </div>
                </div>
                <button class="btn w-100 fw-bold py-2"
                        style="background:#e0f2f1;color:var(--sg-primary);border-radius:14px;"
                        onclick="viewStationVehicles(${s.id})">
                    <i class="fas fa-search me-2"></i>${t('station_view_detail')}
                </button>
            </div>`);
    });
}

function viewStationVehicles(stationId) {
    currentStartStationId = stationId;
    const st = stationsData.find(x => x.id === stationId);
    document.getElementById('sd-name').innerText = translateStationName(st.name);
    document.getElementById('sd-dist').innerText = st.dist || t('station_near_you');

    const vList = document.getElementById('sd-vehicle-list');
    vList.innerHTML = '';

    if (!st.vehicles || st.vehicles.length === 0) {
        vList.innerHTML = `
            <div style="display:flex;flex-direction:column;align-items:center;
                        justify-content:center;padding:36px 20px;gap:12px;text-align:center;">
                <div style="width:72px;height:72px;background:#fef2f2;border-radius:50%;
                            display:flex;align-items:center;justify-content:center;">
                    <i class="fas fa-bicycle" style="font-size:28px;color:#fca5a5;"></i>
                </div>
                <div style="font-weight:800;font-size:1.05rem;color:#374151;">
                    ${t('station_empty_title')}
                </div>
                <div style="font-size:0.85rem;color:#9ca3af;max-width:220px;line-height:1.5;">
                    ${t('station_empty_desc')}
                </div>
                <button class="btn btn-outline-secondary btn-sm rounded-pill mt-2"
                        onclick="closeModal('station-detail-modal')">
                    <i class="fas fa-search me-1"></i>${t('station_find_other')}
                </button>
            </div>`;
        openModal('station-detail-modal');
        return;
    }

    st.vehicles.forEach(v => {
        const icon = v.type === 'bike' ? 'fa-bicycle' : v.type === 'ebike' ? 'fa-bolt' : 'fa-motorcycle';
        const colorClass = v.type === 'bike' ? 'text-success' : v.type === 'ebike' ? 'text-warning' : 'text-primary';
        const typeLabel = t('vtype_' + v.type) || v.type;
        const actualPrice = v.type === 'ebike' ? 1500 : v.type === 'bike' ? 500 : 1000;

        let batHtml = '';
        if (v.bat != null) {
            const batPct = Math.max(0, Math.min(100, v.bat));
            const batColor = batPct > 50 ? '#10b981' : batPct > 20 ? '#f59e0b' : '#ef4444';
            const batIcon = batPct > 50 ? 'fa-battery-three-quarters'
                : batPct > 20 ? 'fa-battery-half'
                    : 'fa-battery-quarter';
            const batLabel = batPct > 50 ? t('bat_good') : batPct > 20 ? t('bat_mid') : t('bat_low');
            batHtml = `
                <div style="margin-top:6px;">
                    <div style="display:flex;align-items:center;gap:6px;margin-bottom:3px;">
                        <i class="fas ${batIcon}" style="color:${batColor};font-size:13px;"></i>
                        <span style="font-size:11px;font-weight:700;color:${batColor};">
                            ${batPct}% — ${batLabel}
                        </span>
                    </div>
                    <div style="background:#e2e8f0;border-radius:999px;height:5px;overflow:hidden;width:110px;">
                        <div style="background:${batColor};width:${batPct}%;height:100%;
                                    transition:width .4s ease;border-radius:999px;"></div>
                    </div>
                </div>`;
        } else {
            batHtml = `
                <div style="margin-top:6px;">
                    <span style="font-size:11px;color:#94a3b8;font-style:italic;">
                        <i class="fas fa-leaf me-1 text-success"></i>${t('vtype_manual_note')}
                    </span>
                </div>`;
        }

        vList.innerHTML += `
            <div class="single-bike-item">
                <div class="d-flex align-items-center gap-2" style="flex:1;min-width:0;">
                    <div class="bike-img"><i class="fas ${icon} ${colorClass}"></i></div>
                    <div style="min-width:0;">
                        <div style="display:flex;align-items:center;gap:6px;flex-wrap:wrap;">
                            <span class="fw-bold" style="font-size:1rem;">${v.name}</span>
                            <span class="text-muted" style="font-size:0.78rem;">(${v.id})</span>
                            <span class="badge"
                                  style="background:#f0fdf4;color:#16a34a;font-size:0.7rem;border:1px solid #bbf7d0;">
                                ${typeLabel}
                            </span>
                        </div>
                        ${batHtml}
                        <div class="mt-2">
                            <span class="badge bg-white text-dark border">
                                <i class="fas fa-tag text-danger me-1"></i>${v.price}${t('per_minute')}
                            </span>
                        </div>
                    </div>
                </div>
                <div class="d-flex flex-column gap-2" style="flex-shrink:0;">
                    <button class="btn btn-primary fw-bold px-3 rounded-pill btn-sm"
                            onclick="selectVehicleToRent('${v.name}', ${actualPrice})">
                        <i class="fas fa-key me-1"></i>${t('btn_rent_vehicle')}
                    </button>
                    <button class="btn btn-outline-warning fw-bold px-3 rounded-pill btn-sm"
                            onclick="reserveVehicle(${v.id}, '${v.name}')">
                        <i class="fas fa-clock me-1"></i>${t('btn_reserve')}
                    </button>
                </div>
            </div>`;
    });

    openModal('station-detail-modal');
}

function updateIncentiveUI(data) {
    const listContainer = document.getElementById("notification-list");
    const badge = document.getElementById("promo-badge");
    if (!listContainer || !badge) return;

    const promoStations = data.filter(s => {
        if (!s.Capacity || s.Capacity <= 0) return false;
        return (s.CurrentBikes / s.Capacity) < 0.2;
    });

    badge.innerText = promoStations.length;
    badge.style.display = promoStations.length > 0 ? "block" : "none";

    if (promoStations.length > 0) {
        listContainer.innerHTML = promoStations.map(s => `
            <div class="card mb-3 border-warning" style="background:#fffbeb;">
                <div class="card-body p-3">
                    <h6>${translateStationName(s.StationName || s.name || '')}</h6>
                    <span class="badge bg-warning text-dark">
                        ${t('notif_discount')}
                    </span>
                    <button class="btn btn-sm btn-warning w-100 mt-2"
                            onclick="focusOnMap(${s.Latitude}, ${s.Longitude})">
                        <i class="fas fa-location-arrow me-1"></i>${t('notif_navigate')}
                    </button>
                </div>
            </div>`).join('');
    } else {
        listContainer.innerHTML = `
            <div class="text-center py-3 text-muted">
                <i class="fas fa-tag me-1 opacity-50"></i>${t('notif_empty')}
            </div>`;
    }
}

function _highlightText(text, keyword) {
    if (!keyword) return text;
    const re = new RegExp(`(${keyword.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')})`, 'gi');
    return text.replace(re, '<mark style="background:#fef08a;padding:0 2px;border-radius:3px;">$1</mark>');
}

function _highlightMarker(idx) {
    const el = stationMarkers[idx]?.getElement();
    if (!el) return;
    el.style.transition = 'transform .25s ease';
    el.style.transform = 'scale(1.35)';
    el.style.zIndex = '1000';
    el.style.filter = 'drop-shadow(0 0 8px #f59e0b)';
}

function _resetMarker(idx) {
    const el = stationMarkers[idx]?.getElement();
    if (!el) return;
    el.style.transform = 'scale(1)';
    el.style.zIndex = '';
    el.style.filter = '';
}

function applyLanguage(lang) {
    currentLang = lang;
    localStorage.setItem("saigonride_lang", lang);
    const searchEl = document.getElementById('station-search');
    if (searchEl) searchEl.placeholder = dict['search_placeholder'] || '';

    const dict = translations[lang] || translations.vi;
    document.querySelectorAll("[data-i18n]").forEach(el => {
        const key = el.getAttribute("data-i18n");
        if (dict[key]) el.textContent = dict[key];
    });

    updateLangButtons(lang);
    document.documentElement.lang = lang === "en" ? "en" : "vi";

    if (typeof renderStationsList === 'function') renderStationsList();
    if (typeof populateGroupStations === 'function') populateGroupStations();
    if (typeof updateIncentiveUI === 'function' && typeof stationsData !== 'undefined') {
        updateIncentiveUI(stationsData);
    }
    if (typeof renderStationsOnMap === 'function' && typeof stationsData !== 'undefined') {
        renderStationsOnMap(stationsData);
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const saved = localStorage.getItem('saigonride_lang') || 'vi';
    currentLang = saved;
    const dict = translations[saved] || translations.vi;
    document.querySelectorAll('[data-i18n]').forEach(el => {
        const key = el.getAttribute('data-i18n');
        if (dict[key]) el.textContent = dict[key];
    });
    document.documentElement.lang = saved === 'en' ? 'en' : 'vi';
    if (typeof updateLangButtons === 'function') updateLangButtons(saved);
});
