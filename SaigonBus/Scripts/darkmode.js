const DARK_KEY = "saigonbus_darkMode"; 

function toggleDarkMode() {
    document.body.classList.toggle("dark-mode");
    localStorage.setItem(DARK_KEY, document.body.classList.contains("dark-mode"));
    updateDarkModeButton();
}

function updateDarkModeButton() {
    const btn = document.getElementById("darkModeToggle");
    if (!btn) return;
    const isDark = document.body.classList.contains("dark-mode");
    // Đổi icon/text nếu cần
    btn.innerHTML = isDark
        ? '<i class="fa fa-sun"></i>'
        : '<i class="fa fa-moon"></i>';
}

// Áp dụng ngay khi trang load — tránh flash trắng
(function () {
    if (localStorage.getItem(DARK_KEY) === "true") {
        document.body.classList.add("dark-mode");
    }
})();

// Gắn sự kiện sau khi DOM sẵn sàng
document.addEventListener("DOMContentLoaded", function () {
    const btn = document.getElementById("darkModeToggle");
    if (btn) {
        btn.addEventListener("click", function (e) {
            e.preventDefault();
            toggleDarkMode();
        });
    }
    updateDarkModeButton();
});