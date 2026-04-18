# 🚚 Limak Cargo Project

## 📖 Haqqında

**Limak Cargo Project** — istifadəçilərə xaricdən sifariş vermək, kargo idarə etmək və bağlamaları izləmək imkanı yaradan tam funksional kargo tətbiqidir. Sistem həm istifadəçi paneli, həm də admin panel vasitəsilə işləyir.

İstifadəçilər qeydiyyatdan keçərək sifariş verə, xarici anbar ünvanından istifadə edə və ödənişlərini online şəkildə həyata keçirə bilirlər. Admin panel vasitəsilə isə bütün proseslərə nəzarət etmək mümkündür.

---

## ✨ Xüsusiyyətlər

### 👤 İstifadəçi Paneli

* 🔐 JWT ilə təhlükəsiz qeydiyyat və giriş
* 📦 Kargo sifarişi yaratma
* 🌍 Xarici anbar ünvanı ilə sifariş
* 💳 Kapital API ilə online ödəniş
* 🔔 App daxili və email bildirişləri
* 📊 Sifariş və bağlama statuslarının izlənməsi

### 🛠 Admin Panel

* 🧾 CRUD əməliyyatları
* 📦 Bağlamalara nəzarət və status dəyişmə
* 📈 Sifarişlərin vəziyyətinə nəzarət
* ⚙️ Sistem idarəetməsi

---

## 🏗 Arxitektura

Layihə **Onion Architecture** prinsiplərinə uyğun şəkildə qurulmuşdur:

* **Domain** – əsas biznes məntiqi
* **Application** – use-case və servis layı
* **Infrastructure** – xarici servislər və inteqrasiyalar
* **Persistence** – verilənlər bazası ilə işləmə
* **API** – təqdimat layı (Web API)

---

## 🛠 Texnologiyalar

### Backend

* ASP.NET Web API
* SQL Server
* Entity Framework

### Frontend

* ASP.NET MVC

### Digər

* JWT Authentication
* Kapital Bank API (Online Payment)
* Email Service
* Notification System

---

## ⚙️ Quraşdırma

```bash
git clone https://github.com/username/limak-cargo-project.git
cd limak-cargo-project
```

### Database

* SQL Server qurun
* Connection string-i `appsettings.json` faylında dəyişin

### Backend

```bash
dotnet restore
dotnet run
```

---

## 📂 Struktur

```
LimakCargo/
│── Domain/
│── Application/
│── Infrastructure/
│── Persistence/
│── API/
│── MVC/
│── README.md
```

---

## 🔐 Təhlükəsizlik

* JWT Authentication istifadə olunur
* Role-based access control
* Secure API endpoints
