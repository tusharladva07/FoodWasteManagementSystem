# Food Waste Management System (Hospitality Operations)

[![.NET Version](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-16.2-DD0031?logo=angular)](https://angular.io/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-10.0-512BD4)](https://learn.microsoft.com/ef/core/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap)](https://getbootstrap.com/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger)](https://swagger.io/)

A specialized enterprise hospitality application designed to minimize food waste, eliminate breakfast buffet shortages, and optimize kitchen prep forecasting in hotels, resorts, and convention centers.

---

## 🍽️ Problem Statement & Domain Context

In hotel and resort food & beverage operations, the morning breakfast buffet is one of the highest cost drivers and a frequent source of guest friction:

1. **Food Waste / Over-Preparation:** Kitchens over-prepare high-cost protein and dairy items (bacon, sausages, live egg counters) to prevent empty trays during peak hours.
2. **Late Cancellations & No-Shows:** Group bookings, conference delegates, and last-minute room cancellations result in fully prepared, untouched food being discarded.
3. **Food Shortage & Guest Friction:** Under-preparing leads to empty buffet stations during peak breakfast rushes (e.g., 9:00 AM), lowering guest satisfaction (CSAT/NPS) and putting stress on line cooks.

The **Food Waste Management System** bridges this operational gap by integrating automated vision audits, cancellation ingestion, and dynamic prep forecasting into a unified kitchen management dashboard.

---

## ✨ Key Features

- **📸 AI Buffet Tray Audit (Computer Vision)**
  - Upload photos of leftover buffet trays at the close of service (10:30 AM).
  - Automatically identifies dish names (*Scrambled Eggs*, *Crispy Bacon*, *Breakfast Sausages*, *Hash Browns*, *Baked Beans*).
  - Calculates remaining volume percentage, wasted mass (kg), and financial loss ($ USD).
  - Eliminates the labor-intensive need for kitchen stewards to manually weigh food scraps.

- **📈 Morning Prep & Shortage Preventer (Dynamic Forecasting)**
  - Enter expected guest covers for the upcoming breakfast service.
  - Dynamically calculates optimal batch prep count based on historical group cancellation trends.
  - Applies an intelligent safety buffer (10%) to prevent buffet line run-outs while eliminating over-prep.
  - Displays real-time cumulative waste (kg) and lost revenue across historical services.

- **📋 Group & Booking Cancellation Ingestion**
  - Record group and conference cancellations, dropped covers, cancellation rates, and estimated cost impact.
  - Instantly feeds into the forecasting engine to refine safety stock algorithms.

- **🔄 Untouched Food Re-Routing Alerts**
  - Proactively flags untouched surplus meals caused by cancellations.
  - Triggers workflow alerts to re-route viable portions to staff cafeterias or local food redistribution partners before food spoils.

- **📊 Executive KPI Dashboard**
  - Instant visibility into Total Food Waste (kg), Cumulative Lost Revenue ($ USD), and Recommended Morning Prep Portions.

---

## 🏗️ Architecture & Technology Stack

```
                        ┌──────────────────────────────────────┐
                        │      Angular 16 Dashboard UI         │
                        │    (Bootstrap 5, Responsive Web)     │
                        └──────────────────┬───────────────────┘
                                           │  HTTP REST / Proxy (Port 4200)
                                           ▼
                        ┌──────────────────────────────────────┐
                        │       ASP.NET Core 10 Web API        │
                        │   (Controllers, Services, Repos)     │
                        └──────────┬────────────────┬──────────┘
                                   │                │
             ┌─────────────────────┴──────┐         └───────────────────────┐
             ▼                            ▼                                 ▼
   [ VisionService ]            [ Forecast Engine ]              [ Entity Framework Core ]
   (Mock / AI Analyzer)        (Dynamic Prep Algorithms)          (SQL Server / LocalDB)
```

### Technology Breakdown

| Layer | Technologies |
| :--- | :--- |
| **Frontend** | Angular 16.2, TypeScript, Bootstrap 5.3, RxJS, HTML5 / CSS3 |
| **Backend** | ASP.NET Core (.NET 10.0 Web API), C# |
| **ORM / Data** | Entity Framework Core 10, SQL Server (MSSQLLocalDB) |
| **API Documentation**| Swashbuckle / Swagger UI |
| **Containerization** | Docker, Dockerfile |

---

## 📁 Repository Structure

```text
FoodWasteManagementSystem/
├── .gitignore                          # Root Git ignore rules (.NET, Angular, IDE, OS)
├── README.md                           # Project documentation & setup guide
├── backend/
│   └── Hospitality.API/                # ASP.NET Core 10 Web API
│       ├── Controllers/                # API Controllers (Vision, Forecast, Orders)
│       ├── Data/                       # EF Core DbContext (HospitalityDbContext)
│       ├── Migrations/                 # EF Core Code-First Migrations
│       ├── Models/                     # Domain & DTO Models
│       ├── Properties/                 # launchSettings.json (Ports: 5088, 7059)
│       ├── Repositories/               # Data access repository pattern
│       ├── Services/                   # Business logic & VisionService
│       ├── Dockerfile                  # Container build definition
│       ├── Program.cs                  # ASP.NET Core entry point & DI configuration
│       └── appsettings.json            # Database connection & configuration
└── fronted/                            # Angular 16 Frontend Application
    ├── proxy.conf.json                 # Dev proxy routing /api -> http://localhost:5088
    ├── package.json                    # Angular & NPM dependencies
    ├── angular.json                    # Angular CLI project configuration
    └── src/
        ├── app/
        │   ├── components/dashboard/   # Main Operations & Audit Dashboard
        │   ├── models/                 # TypeScript interfaces
        │   ├── services/               # Angular HttpClient service (HospitalityService)
        │   └── app.module.ts           # Root module registration
        └── styles.css                  # Global styles & theme tokens
```

---

## 🚀 Getting Started

### Prerequisites

Ensure you have the following installed on your development machine:

- **[.NET 10.0 SDK](https://dotnet.microsoft.com/download)** or higher
- **[Node.js](https://nodejs.org/)** (v18.x or v20.x recommended) & **npm**
- **[Angular CLI](https://angular.io/cli)** (version 16):
  ```bash
  npm install -g @angular/cli@16
  ```
- **SQL Server**: SQL Server LocalDB (`(localdb)\MSSQLLocalDB`) or standard SQL Server instance.

---

### Step 1: Configure & Run the Backend API

1. Open a terminal and navigate to the backend project directory:
   ```bash
   cd backend/Hospitality.API
   ```

2. Review or update your database connection string in `appsettings.json` (defaults to SQL Server LocalDB):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=FoodWasteManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. Apply database migrations to create the database and tables:
   ```bash
   dotnet ef database update
   ```

4. Run the API application:
   ```bash
   dotnet run
   ```
   The backend service starts on:
   - **HTTP:** `http://localhost:5088`
   - **Swagger UI:** `http://localhost:5088/swagger`

---

### Step 2: Configure & Run the Angular Frontend

1. Open a second terminal and navigate to the frontend directory:
   ```bash
   cd fronted
   ```

2. Install all required dependencies:
   ```bash
   npm install
   ```

3. Launch the development server with the proxy configuration:
   ```bash
   npm start
   # or: ng serve --proxy-config proxy.conf.json
   ```

4. Open your browser and navigate to:
   ```text
   http://localhost:4200
   ```

---

## 📡 API Reference

### 1. Vision & Buffet Tray Audit
- **`POST /api/Vision/analyze`**
  - **Description:** Accepts a multipart form upload of a tray photo, evaluates volume remaining, and logs the waste record.
  - **Payload:** `file` (image binary, e.g., `.jpg`, `.png`).
  - **Response Sample:**
    ```json
    {
      "id": "e6a4b123-...",
      "itemName": "Scrambled Eggs",
      "trayCapacityKg": 10.00,
      "remainingPercentage": 30.00,
      "wastedWeightKg": 3.00,
      "estimatedLossUsd": 45.00,
      "timestamp": "2026-10-08T10:30:00Z"
    }
    ```

### 2. Dynamic Morning Prep Forecasting
- **`GET /api/Forecast?expectedCovers={count}`**
  - **Description:** Computes the recommended batch prep portions using historical cancellation data with a safety factor.
  - **Formula:** `Portions = Ceiling(ExpectedCovers * (1 - AvgCancellationRate) * 1.10)`
  - **Response Sample:**
    ```json
    {
      "expectedCovers": 150,
      "historicalCancellationRate": 12.50,
      "recommendedPrepCount": 145,
      "totalWasteKg": 18.50,
      "totalCumulativeDollarLoss": 195.00
    }
    ```

### 3. Booking Cancellations Ingestion
- **`POST /api/Orders/cancellations`**
  - **Description:** Ingests one or more booking cancellation records.
  - **Payload:**
    ```json
    [
      {
        "bookingName": "Tech Conference Summit Group",
        "canceledCovers": 25,
        "cancellationRate": 0.15,
        "estimatedWasteCost": 150.00
      }
    ]
    ```

---

## 📦 Committing to Git

To stage and commit your changes safely with the newly configured `.gitignore`:

```bash
# 1. Verify status (unwanted binaries, node_modules, and cache files are ignored)
git status

# 2. Stage tracked files
git add .

# 3. Create your initial commit
git commit -m "feat: initial commit for Food Waste Management System with backend API, Angular dashboard, and documentation"
```

---

## 📄 License
This project is licensed for internal enterprise hospitality operations and educational use.
