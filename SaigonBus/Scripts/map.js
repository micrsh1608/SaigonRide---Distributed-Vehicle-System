let map;
let userMarker;
let stationMarkers = [];
let routingPolyline;

function _mt(key) {
    if (typeof t === 'function') return t(key);
    const fallback = {
        map_you_are_here: 'Bạn đang ở đây',
        map_no_vehicle: 'Không có xe khả dụng',
        map_choose_vehicle: 'Chọn xe tại trạm này',
        map_route_label: 'Chuyến đi dự kiến',
        map_distance: 'Khoảng cách',
        map_vehicles: 'xe',
        bat_good: 'Tốt',
        bat_mid: 'Trung bình',
        bat_low: 'Yếu',
        vtype_manual_note: 'Xe đạp cơ',
        per_minute: 'đ/p',
    };
    return fallback[key] || key;
}

function initMap() {
    map = L.map('main-map').setView([10.8231, 106.6297], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://openstreetmap.org">OpenStreetMap</a> contributors'
    }).addTo(map);

    setTimeout(function () { map.invalidateSize(); }, 200);

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (pos) {
            const lat = pos.coords.latitude;
            const lon = pos.coords.longitude;
            map.setView([lat, lon], 14);

            const userIcon = L.divIcon({
                html: '<i class="fas fa-street-view fa-3x text-danger" style="filter:drop-shadow(0 4px 4px rgba(0,0,0,0.4));"></i>',
                className: '',
                iconSize: [30, 30],
                iconAnchor: [15, 30]
            });

            userMarker = L.marker([lat, lon], { icon: userIcon })
                .addTo(map)
                .bindPopup('<b style="color:#ef4444">' + _mt('map_you_are_here') + '</b>')
                .openPopup();
        });
    }
}

const HCMC_KNOWN_COORDS = {
    'ben thanh': [10.7729, 106.6980],
    'metro ben thanh': [10.7729, 106.6980],
    'ho con rua': [10.7800, 106.6990],
    'nha tho duc ba': [10.7798, 106.6991],
    'tan son nhat': [10.8185, 106.6587],
    'thu duc': [10.8498, 106.7716],
    'go vap': [10.8355, 106.6815],
    'binh thanh': [10.8009, 106.7100],
    'phu nhuan': [10.7998, 106.6820],
    'quan 1': [10.7769, 106.7009],
    'quan 3': [10.7864, 106.6849],
    'quan 7': [10.7379, 106.7218],
    'quan 2': [10.7880, 106.7417],
    'quan 4': [10.7584, 106.7044],
    'quan 5': [10.7532, 106.6665],
    'quan 6': [10.7464, 106.6335],
    'quan 8': [10.7238, 106.6282],
    'quan 9': [10.8417, 106.7817],
    'quan 10': [10.7731, 106.6681],
    'quan 11': [10.7626, 106.6516],
    'quan 12': [10.8633, 106.6576],
    'tan binh': [10.8036, 106.6507],
    'tan phu': [10.7897, 106.6279],
    'binh tan': [10.7645, 106.6093],
    'hoc mon': [10.8895, 106.5967],
    'cu chi': [11.0144, 106.4723],
    'nha be': [10.6989, 106.7367],
    'binh chanh': [10.7003, 106.5817],
    'can gio': [10.4147, 106.9536],
    'vincom': [10.7803, 106.6996],
    'landmark': [10.7951, 106.7213],
    'vivo city': [10.7281, 106.7178],
    'crescent': [10.7334, 106.7208],
    'suoi tien': [10.8432, 106.7984],
    'dam sen': [10.7638, 106.6326],
    'zoo': [10.7882, 106.6959],
    'bitexco': [10.7713, 106.7039],
    'cho lon': [10.7528, 106.6612],
};

const GRID_OFFSETS = [
    [0.02, 0.02], [-0.02, 0.02], [0.02, -0.02], [-0.02, -0.02],
    [0.04, 0.00], [-0.04, 0.00], [0.00, 0.04], [0.00, -0.04],
    [0.03, 0.03], [-0.03, -0.03], [0.05, -0.01], [-0.01, 0.05],
    [0.06, 0.00], [-0.06, 0.00], [0.00, 0.06], [0.00, -0.06],
    [0.035, 0.05], [-0.05, 0.035], [0.05, -0.035], [-0.035, -0.05],
];
let _fallbackIdx = 0;

function _removeDiacritics(str) {
    return str.normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase();
}

function _resolveCoords(station) {
    if (station.lat && station.lng) return [station.lat, station.lng];

    const nameNorm = _removeDiacritics(station.name || '');
    for (const keyword in HCMC_KNOWN_COORDS) {
        if (nameNorm.includes(_removeDiacritics(keyword))) return HCMC_KNOWN_COORDS[keyword];
    }

    const offset = GRID_OFFSETS[_fallbackIdx % GRID_OFFSETS.length];
    _fallbackIdx++;
    return [10.8231 + offset[0], 106.6297 + offset[1]];
}

function _buildBatteryHtml(v) {
    if (v.bat != null) {
        const batPct = Math.max(0, Math.min(100, v.bat));
        const batColor = batPct > 50 ? '#10b981' : batPct > 20 ? '#f59e0b' : '#ef4444';
        const batLabel = batPct > 50 ? _mt('bat_good') : batPct > 20 ? _mt('bat_mid') : _mt('bat_low');
        return (
            '<div style="background:#f1f5f9;border-radius:999px;height:5px;margin-top:4px;overflow:hidden;">' +
            '<div style="background:' + batColor + ';width:' + batPct + '%;height:100%;border-radius:999px;"></div>' +
            '</div>' +
            '<span style="font-size:10px;color:' + batColor + ';font-weight:700;">' +
            batPct + '% — ' + batLabel +
            '</span>'
        );
    }
    return '<span style="font-size:10px;color:#94a3b8;">' + _mt('vtype_manual_note') + '</span>';
}

function renderStationsOnMap(stations) {
    stationMarkers.forEach(function (m) { map.removeLayer(m); });
    stationMarkers = [];
    _fallbackIdx = 0;

    stations.forEach(function (s) {
        const coords = _resolveCoords(s);
        const sLat = coords[0];
        const sLng = coords[1];
        s.lat = sLat;
        s.lng = sLng;

        const ratio = s.capacity > 0 ? s.currentCount / s.capacity : 0;
        const markerColor = ratio >= 0.5 ? '#10b981' : ratio >= 0.2 ? '#f59e0b' : '#ef4444';
        const markerBg = ratio >= 0.5 ? '#ecfdf5' : ratio >= 0.2 ? '#fffbeb' : '#fef2f2';

        const stationIcon = L.divIcon({
            html:
                '<div style="background:' + markerBg + ';border:2px solid ' + markerColor +
                ';border-radius:12px;padding:5px 9px;font-size:11px;font-weight:800;color:' + markerColor +
                ';white-space:nowrap;box-shadow:0 3px 10px rgba(0,0,0,0.18);display:flex;' +
                'align-items:center;gap:5px;cursor:pointer;font-family:Segoe UI,sans-serif;">' +
                '<i class="fas fa-charging-station" style="font-size:13px;"></i>' +
                '<span>' + s.currentCount + '/' + s.capacity + '</span>' +
                '</div>',
            className: '',
            iconSize: [84, 32],
            iconAnchor: [42, 32]
        });

        const marker = L.marker([sLat, sLng], { icon: stationIcon }).addTo(map);

        let vehiclesHtml = '';
        if (!s.vehicles || s.vehicles.length === 0) {
            vehiclesHtml =
                '<p style="color:#94a3b8;font-size:12px;text-align:center;padding:10px 0;">' +
                '<i class="fas fa-exclamation-circle"></i> ' + _mt('map_no_vehicle') +
                '</p>';
        } else {
            s.vehicles.forEach(function (v) {
                const vIcon = v.type === 'bike' ? 'fa-bicycle' : v.type === 'ebike' ? 'fa-bolt' : 'fa-motorcycle';
                const vColor = v.type === 'bike' ? '#10b981' : v.type === 'ebike' ? '#f59e0b' : '#3b82f6';
                const batHtml = _buildBatteryHtml(v);

                vehiclesHtml +=
                    '<div style="display:flex;align-items:center;margin-bottom:8px;' +
                    'padding-bottom:8px;border-bottom:1px solid #f8fafc;">' +
                    '<i class="fas ' + vIcon + '" style="color:' + vColor + ';font-size:18px;' +
                    'width:28px;text-align:center;flex-shrink:0;"></i>' +
                    '<div style="flex:1;margin-left:8px;min-width:0;">' +
                    '<div style="font-weight:700;font-size:12px;">' + v.name + '</div>' +
                    batHtml +
                    '</div>' +
                    '<span style="font-size:10px;color:#ef4444;font-weight:800;flex-shrink:0;margin-left:6px;">' +
                    v.price + _mt('per_minute') +
                    '</span>' +
                    '</div>';
            });
        }

        const distText = s.dist || '';
        const popupContent =
            '<div style="min-width:230px;font-family:Segoe UI,sans-serif;">' +
            '<h6 style="font-weight:800;margin:0 0 4px;color:#00897b;font-size:14px;">' + s.name + '</h6>' +
            '<p style="font-size:12px;color:#64748b;margin:0 0 10px;padding-bottom:8px;border-bottom:1px solid #f1f5f9;">' +
            '<i class="fas fa-location-arrow" style="color:#ef4444;"></i> ' + distText +
            ' &bull; <span style="color:' + markerColor + ';font-weight:700;">' +
            s.currentCount + '/' + s.capacity + ' ' + _mt('map_vehicles') +
            '</span></p>' +
            '<div style="max-height:160px;overflow-y:auto;padding-right:4px;">' + vehiclesHtml + '</div>' +
            '<button onclick="viewStationVehicles(' + s.id + ')" ' +
            'style="margin-top:10px;width:100%;padding:10px;border:none;border-radius:12px;' +
            'background:#00897b;color:#fff;font-weight:800;font-size:13px;cursor:pointer;' +
            'box-shadow:0 4px 10px rgba(0,137,123,0.2);">' +
            '<i class="fas fa-bicycle" style="margin-right:6px;"></i>' +
            _mt('map_choose_vehicle') +
            '</button></div>';

        marker.bindPopup(popupContent, { maxWidth: 260 });
        stationMarkers.push(marker);
    });
}

function focusOnMap(lat, lng) {
    if (!map) return;
    map.setView([lat, lng], 16, { animate: true });
    map.invalidateSize();
}

function showRouteBetweenStations(startId, endId) {
    const startSt = stationsData.find(function (x) { return x.id == startId; });
    const endSt = stationsData.find(function (x) { return x.id == endId; });
    if (!startSt || !endSt) return;

    if (routingPolyline) map.removeLayer(routingPolyline);

    routingPolyline = L.polyline(
        [[startSt.lat, startSt.lng], [endSt.lat, endSt.lng]],
        { color: '#ef4444', weight: 4, dashArray: '10, 10' }
    ).addTo(map);

    map.fitBounds(routingPolyline.getBounds(), { padding: [50, 50] });

    const distKm = (map.distance(
        [startSt.lat, startSt.lng],
        [endSt.lat, endSt.lng]
    ) / 1000).toFixed(1);

    routingPolyline
        .bindPopup('<b>' + _mt('map_route_label') + '</b><br>' + _mt('map_distance') + ': ~' + distKm + ' km')
        .openPopup();
}

window.addEventListener('resize', function () {
    if (map) map.invalidateSize();
});