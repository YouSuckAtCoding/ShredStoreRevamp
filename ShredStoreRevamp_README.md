# ShredStoreRevamp

[🇧🇷 Português](./README.pt-BR.md)

A full-stack e-commerce web application for a guitar shop, built with ASP.NET Core. The project follows a clean layered architecture separating API, presentation, application logic, contracts, and data access into distinct projects.

---

## 🏗️ Architecture

The solution is organized into the following projects:

| Project | Responsibility |
|---|---|
| `ShredStore` | ASP.NET Core Web API — endpoints, controllers, JWT auth |
| `ShredStorePresentation` | ASP.NET Core MVC — Razor Views, Bootstrap, JS frontend |
| `Application` | Business logic — services, repositories, domain models |
| `Contracts` | Shared DTOs and interfaces |
| `DatabaseAccess` | Raw SQL queries via Dapper |
| `DatabaseConnection` | SQL Server connection management |
| `ShredStoreTests` | Integration tests |
| `ShredStoreApiTests` | API-level tests |

---

## 🛠️ Tech Stack

- **Backend**: ASP.NET Core Web API (.NET 7+)
- **Frontend**: ASP.NET Core MVC with Razor Views, Bootstrap, JavaScript
- **Database**: SQL Server + Dapper (raw SQL, no ORM)
- **Authentication**: JWT Bearer Tokens
- **Rate Limiting**: AspNetCoreRateLimit
- **Testing**: xUnit, TestContainers (MsSQL), Moq, FluentAssertions, Bogus, Coverlet
- **Infrastructure**: Docker + docker-compose

---

## 🔐 Authentication

Authentication is handled via **JWT Bearer Tokens**. Protected endpoints require a valid token in the `Authorization` header.

---

## 🗄️ Data Access

All database interactions use **Dapper** with raw SQL queries — no ORM (Entity Framework) is used. The `DatabaseAccess` project contains all query logic, and `DatabaseConnection` manages the SQL Server connection.

---

## 🧪 Testing

Tests are split across two projects:

- **`ShredStoreTests`** — Integration tests using TestContainers (spins up a real MsSQL container), Moq for mocking, FluentAssertions for readable assertions, and Bogus for fake data generation.
- **`ShredStoreApiTests`** — API-level tests.

Code coverage is tracked via **Coverlet**.

To run tests:

```bash
dotnet test
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 7+ SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- SQL Server (or use Docker)

### Running with Docker

```bash
docker-compose up --build
```

### Running Locally

1. Clone the repository:

```bash
git clone https://github.com/YouSuckAtCoding/ShredStoreRevamp.git
cd ShredStoreRevamp
```

2. Update the connection string in `appsettings.json`.

3. Run the API:

```bash
dotnet run --project ShredStore
```

4. Run the MVC frontend:

```bash
dotnet run --project ShredStorePresentation
```

---

## 📁 Project Structure

```
ShredStoreRevamp/
├── ShredStore/                  # Web API
├── ShredStorePresentation/      # MVC Frontend (Razor Views)
├── Application/                 # Business logic, services, models
├── Contracts/                   # DTOs and interfaces
├── DatabaseAccess/              # Dapper queries
├── DatabaseConnection/          # SQL Server connection
├── ShredStoreTests/             # Integration tests
├── ShredStoreApiTests/          # API tests
├── docker-compose.yml
└── README.md
```

---

## 📄 License

This project is for educational/portfolio purposes.
