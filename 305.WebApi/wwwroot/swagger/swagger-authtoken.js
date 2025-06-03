﻿let accessToken = null;
let refreshToken = null;
let tokenExpiresAt = 0;

async function getToken() {
    const formData = new URLSearchParams();
    formData.append("user_name_or_email", "info@305.com");
    formData.append("password", "QAZqaz!@#123");

    const response = await fetch("/api/auth/username", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        body: formData.toString()
    });

    if (!response.ok) {
        console.error("Login failed:", await response.text());
        return;
    }

    const data = await response.json();
    accessToken = data.data.access_token;
    refreshToken = data.data.refreshToken;
    tokenExpiresAt = Date.now() + (data.data.expireIn * 60 * 1000); // expireIn به دقیقه است
    console.log(accessToken);
    ui.preauthorizeApiKey("Bearer", accessToken); // تأیید توکن برای استفاده در API
}

async function refreshAccessToken() {
    const formData = new URLSearchParams();
    formData.append("token", accessToken);
    formData.append("refresh_token", refreshToken);

    const response = await fetch("/api/auth/refresh-token", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        body: formData.toString()
    });

    if (!response.ok) {
        console.error("Refresh token failed:", await response.text());
        return;
    }

    const data = await response.json();
    accessToken = data.data.acces_token;
    refreshToken = data.data.refreshToken;
    tokenExpiresAt = Date.now() + (15 * 60 * 1000); // فرض: توکن جدید هم 15 دقیقه اعتبار دارد

    ui.preauthorizeApiKey("Bearer", accessToken); // تأیید توکن جدید برای استفاده در API
}

async function tokenManager() {
    await getToken(); // دریافت اولیه توکن

    setInterval(async () => {
        if (Date.now() >= tokenExpiresAt - 10000) { // 10 ثانیه قبل از انقضا
            await refreshAccessToken();
        }
    }, 60000); // هر دقیقه بررسی انقضا
}

// اجرای توابع پس از بارگذاری کامل صفحه
window.addEventListener("load", tokenManager);