let map;
let userMarker;
let stationMarkers = [];
let routingPolyline;

function initMap() {
    map = L.map('main-map').setView([10.8231, 106.6297], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors'
    }).addTo(map);

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(pos => {
            const lat = pos.coords.latitude;
            const lon = pos.coords.longitude;
            map.setView([lat, lon], 14);

            const userIcon = L.divIcon({
                html: '<i class="fas fa-street-view fa-3x text-danger" style="filter: drop-shadow(0 4px 4px rgba(0,0,0,0.4));"></i>',
                className: '', iconSize: [30, 30], iconAnchor: [15, 30]
            });

            userMarker = L.marker([lat, lon], { icon: userIcon }).addTo(map)
                .bindPopup("<b class='text-danger'>Bạn đang ở đây</b>").openPopup();
        });
    }
}

function renderStationsOnMap(stations) {
    stationMarkers.forEach(m => map.removeLayer(m));
    stationMarkers = [];

    stations.forEach(s => {
        let sLat = s.lat || (10.8231 + (Math.random() - 0.5) * 0.05);
        let sLng = s.lng || (106.6297 + (Math.random() - 0.5) * 0.05);
        s.lat = sLat; s.lng = sLng;

        const stationIcon = L.divIcon({
            html: `<div class="station-marker-label"><i class="fas fa-charging-station"></i> ${s.currentCount}/${s.capacity}</div>`,
            className: '', iconSize: [60, 30], iconAnchor: [30, 30]
        });

        let marker = L.marker([sLat, sLng], { icon: stationIcon }).addTo(map);

        let popupContent = `
                <div style="min-width: 220px;">
                    <h6 class="fw-bold mb-1" style="color:var(--sg-primary)">${s.name}</h6>
                    <p class="small text-muted mb-2 border-bottom pb-2">Khoảng cách: ${s.dist}</p>
                    <div style="max-height: 150px; overflow-y: auto;">
            `;

        s.vehicles.forEach(v => {
            let isElectric = (v.type === 'ebike' || v.type === 'scooter');
            let vIcon = v.type === 'bike' ? 'fa-bicycle text-success' : v.type === 'ebike' ? 'fa-bolt text-warning' : 'fa-motorcycle text-primary';

            let batHtml = '';
            if (isElectric && v.bat) {
                let batColor = v.bat > 50 ? 'bg-success' : v.bat > 20 ? 'bg-warning' : 'bg-danger';
                batHtml = `
                        <div class="progress mt-1" style="height: 6px;">
                            <div class="progress-bar ${batColor}" style="width: ${v.bat}%"></div>
                        </div>
                        <span class="small fw-bold" style="font-size: 10px;">${v.bat}% Pin</span>
                    `;
            } else {
                batHtml = `<span class="small text-muted" style="font-size: 10px;">Xe đạp cơ</span>`;
            }

            popupContent += `
                    <div class="d-flex align-items-center mb-2 pb-2 border-bottom">
                        <i class="fas ${vIcon} fs-5 me-2 w-25 text-center"></i>
                        <div class="flex-grow-1">
                            <div class="fw-bold" style="font-size: 12px;">${v.name}</div>
                            ${batHtml}
                        </div>
                    </div>`;
        });

        popupContent += `
                    </div>
                    <button class="btn btn-sm w-100 text-white mt-2" style="background:var(--sg-primary);" onclick="viewStationVehicles(${s.id})">
                        Chọn xe tại trạm này
                    </button>
                </div>`;

        marker.bindPopup(popupContent);
        stationMarkers.push(marker);
    });
}


function showRouteBetweenStations(startId, endId) {
    let startSt = stationsData.find(x => x.id == startId);
    let endSt = stationsData.find(x => x.id == endId);

    if (!startSt || !endSt) return;

    if (routingPolyline) map.removeLayer(routingPolyline);

    routingPolyline = L.polyline([
        [startSt.lat, startSt.lng],
        [endSt.lat, endSt.lng]
    ], {
        color: '#ef4444',
        weight: 4,
        dashArray: '10, 10'
    }).addTo(map);

    map.fitBounds(routingPolyline.getBounds(), { padding: [50, 50] });

    let distKm = (map.distance([startSt.lat, startSt.lng], [endSt.lat, endSt.lng]) / 1000).toFixed(1);
    routingPolyline.bindPopup(`<b>Chuyến đi dự kiến</b><br>Khoảng cách: ~${distKm} km`).openPopup();
}