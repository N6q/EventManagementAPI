# 🎉 EventManagementAPI

A full-featured **ASP.NET Core 8 Web API** for managing events,
attendees, and real-time weather-enhanced reports.\
Designed for scalability, clean architecture, and easy integration with
external APIs.

------------------------------------------------------------------------

## 🧭 Table of Contents

-   [🌍 Overview](#-overview)
-   [⚙️ Tech Stack](#️-tech-stack)
-   [📂 Project Structure](#-project-structure)
-   [🧩 Core Features](#-core-features)
-   [🔐 Authentication & Authorization](#-authentication--authorization)
-   [🌦️ Weather Integration](#️-weather-integration)
-   [📊 API Endpoints](#-api-endpoints)
-   [💾 Database Design](#-database-design)
-   [🚀 Getting Started](#-getting-started)
-   [⚡ Using Swagger UI](#-using-swagger-ui)
-   [🧠 Code Architecture](#-code-architecture)
-   [🪶 Logging & Monitoring](#-logging--monitoring)
-   [👥 Roles & Access Levels](#-roles--access-levels)
-   [🧪 Testing Guide](#-testing-guide)
-   [🛠️ Future Enhancements](#️-future-enhancements)
-   [📜 License](#-license)

------------------------------------------------------------------------

## 🌍 Overview

**EventManagementAPI** is a modular and extendable system that
manages: - Event creation and scheduling\
- Attendee registration and validation\
- Real-time weather information via Open-Meteo API\
- Dynamic event reports (with live forecasts, attendee counts, and
timestamps)

It's built with **clean architecture principles** (Repository +
Service + DTO layers)\
and follows **SOLID** and **best RESTful practices**.

------------------------------------------------------------------------

## ⚙️ Tech Stack

  Category                 Technology
  ------------------------ --------------------------------------
  Backend Framework        ASP.NET Core 8 (C#)
  ORM / Database           Entity Framework Core 8 + SQL Server
  Authentication           JWT Bearer Tokens
  External API             Open-Meteo Weather API
  Object Mapping           AutoMapper
  Logging                  Serilog
  Documentation            Swagger / Swashbuckle
  PDF Support (Optional)   QuestPDF
  Tools                    Visual Studio 2022 / VS Code

------------------------------------------------------------------------

## 📂 Project Structure

``` bash
EventManagementAPI/
│
├── Auth/
│   ├── AuthController.cs
│   ├── JwtTokenService.cs
│   ├── JwtSettings.cs
│
├── Controllers/
│   ├── EventController.cs
│   ├── AttendeeController.cs
│   ├── ReportController.cs
│   └── WeatherController.cs
│
├── DTOs/
│   ├── Event/
│   ├── Attendee/
│   ├── Report/
│   └── External/
│
├── Data/
│   ├── AppDbContext.cs
│   └── SeedData.cs
│
├── Services/
│   ├── Implementations/
│   └── Interfaces/
│
├── Repositories/
│   ├── Implementations/
│   └── Interfaces/
│
├── Mapping/
│   └── AutoMapperProfile.cs
│
└── Program.cs
```

------------------------------------------------------------------------

## 🧩 Core Features

✅ **Event Management** - CRUD operations for events\
- Lazy loading of attendees\
- Event filtering and pagination (upcoming, recent, etc.)

✅ **Attendee Management** - Register attendees for specific events\
- Validation: no duplicate emails per event\
- Email and event capacity checks

✅ **Weather Integration** - Fetches live temperature and forecast using
**Open-Meteo API** - Automatically included in event reports

✅ **Reports** - Auto-generates event summary reports with: - Attendee
count\
- Location-based weather\
- Timestamp of generation

✅ **Authentication** - JWT-based secure login\
- Role-based API access (Admin / Attendee)

✅ **Logging** - Centralized structured logging via **Serilog**\
- Console + File logging with daily rotation

------------------------------------------------------------------------

## 🔐 Authentication & Authorization

  ------------------------------------------------------------------------
  Role            Description                         Access
  --------------- ----------------------------------- --------------------
  🛠️ **Admin**    Full control --- manage events and  CRUD
                  attendees                           

  👤 **Attendee** Can register and view event details Read / Register

  🌍 **Public**   Can view upcoming events            Read-only
  ------------------------------------------------------------------------

**Flow:** 1. `POST /api/Auth/login` → get a JWT token\
2. Copy the token → click **Authorize** in Swagger → enter
`Bearer <your_token>`\
3. Access protected endpoints

------------------------------------------------------------------------

## 🌦️ Weather Integration

The system calls the **Open-Meteo API** to fetch real-time data:

    https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true

**Supported Cities (Oman):** - Muscat\
- Salalah\
- Sohar\
- Nizwa

Returned fields:

``` json
{
  "summary": "Current Weather",
  "temperatureC": 34.8,
  "forecastTimeUtc": "2025-10-08T14:00:00Z"
}
```

------------------------------------------------------------------------

## 📊 API Endpoints

  -------------------------------------------------------------------------------------------
  Category          Method       Endpoint                              Description
  ----------------- ------------ ------------------------------------- ----------------------
  **Auth**          `POST`       `/api/Auth/login`                     Generate JWT token

  **Events**        `GET`        `/api/Events`                         List all events

                    `GET`        `/api/Events/{id}`                    Get specific event

                    `POST`       `/api/Events`                         Create event

                    `PUT`        `/api/Events/{id}`                    Update event

                    `DELETE`     `/api/Events/{id}`                    Delete event

  **Attendees**     `POST`       `/api/Attendee`                       Register attendee

                    `GET`        `/api/Attendee/event/{eventId}`       List attendees for
                                                                       event

  **Reports**       `GET`        `/api/Report/{eventId}`               Single report (with
                                                                       weather)

                    `GET`        `/api/Report/upcoming`                Upcoming event reports

  **Weather**       `GET`        `/api/Weather/forecast?city=Muscat`   Fetch current weather
  -------------------------------------------------------------------------------------------

------------------------------------------------------------------------

## 💾 Database Design

  Table                  Description
  ---------------------- -------------------------------------------
  **Events**             Stores all event details
  **Attendees**          Links users to events they registered for
  **Users (Optional)**   Holds credentials for JWT authentication

**Relationships:** - One `Event` → Many `Attendees`\
- Each `Attendee` references one `EventId`

------------------------------------------------------------------------

## 🚀 Getting Started

### 🧱 Prerequisites

-   Visual Studio 2022 / VS Code\
-   .NET 8 SDK\
-   SQL Server LocalDB

### 🧩 Setup Instructions

``` bash
git clone https://github.com/<your-repo>/EventManagementAPI.git
cd EventManagementAPI
dotnet restore
dotnet ef database update
dotnet run
```

Then open your browser →\
👉 `https://localhost:7288/swagger`

------------------------------------------------------------------------

## ⚡ Using Swagger UI

1.  Run the project (`dotnet run` or F5)
2.  Go to `/swagger`
3.  Click **Authorize** → enter your JWT token
4.  Test any endpoint (Create Event, Register Attendee, etc.)

------------------------------------------------------------------------

## 🧠 Code Architecture

  Layer                  Responsibility
  ---------------------- -----------------------------------------
  **Controllers**        Handle HTTP requests and responses
  **Services**           Business logic
  **Repositories**       Data access logic
  **DTOs**               Data transfer objects (decouple models)
  **Mappings**           AutoMapper configuration
  **ExternalServices**   Connects to APIs like Open-Meteo

------------------------------------------------------------------------

## 🪶 Logging & Monitoring

-   Uses **Serilog** with two sinks:
    -   Console output\
    -   File logging (`Logs/log-.txt`)
-   Automatically rotates logs daily.

``` json
"Serilog": {
  "WriteTo": [
    { "Name": "Console" },
    { "Name": "File", "Args": { "path": "Logs/log-.txt", "rollingInterval": "Day" } }
  ]
}
```

------------------------------------------------------------------------

## 👥 Roles & Access Levels

  Role           Endpoints                         Access
  -------------- --------------------------------- ---------------
  **Admin**      `/api/Events`, `/api/Report`      Full CRUD
  **Attendee**   `/api/Attendee`, `/api/Weather`   Create + Read
  **Public**     `/api/Events/upcoming`            Read-only

------------------------------------------------------------------------

## 🧪 Testing Guide

✅ Unit Test Ideas: - EventService → Add / Update / Delete events\
- AttendeeService → Prevent duplicate registration\
- ReportService → Validate weather integration\
- AuthController → Token generation test

✅ Integration Test Ideas: - Full login → register → report → weather
flow

------------------------------------------------------------------------

## 🛠️ Future Enhancements

-   📧 Email notifications for attendees\
-   🧾 PDF report generation (QuestPDF integration)\
-   🕒 Event reminders via background job\
-   📱 Blazor or Next.js front-end dashboard\
-   🌍 Multi-language support (English + Arabic)



------------------------------------------------------------------------

## 🧑‍💻 Author

**Samir Al-Bulushi**\
🔗 [GitHub Profile](https://github.com/N6q)

