# ShredStoreRevamp

[🇺🇸 English](./README.md)

Uma aplicação web de e-commerce full-stack para uma loja de guitarras, construída com ASP.NET Core. O projeto segue uma arquitetura em camadas bem definida, separando API, apresentação, lógica de aplicação, contratos e acesso a dados em projetos distintos.

---

## 🏗️ Arquitetura

A solução é organizada nos seguintes projetos:

| Projeto | Responsabilidade |
|---|---|
| `ShredStore` | ASP.NET Core Web API — endpoints, controllers, autenticação JWT |
| `ShredStorePresentation` | ASP.NET Core MVC — Razor Views, Bootstrap, frontend JS |
| `Application` | Lógica de negócio — serviços, repositórios, modelos de domínio |
| `Contracts` | DTOs e interfaces compartilhados |
| `DatabaseAccess` | Queries SQL via Dapper |
| `DatabaseConnection` | Gerenciamento de conexão com SQL Server |
| `ShredStoreTests` | Testes de integração |
| `ShredStoreApiTests` | Testes de API |

---

## 🛠️ Stack Tecnológica

- **Backend**: ASP.NET Core Web API (.NET 7+)
- **Frontend**: ASP.NET Core MVC com Razor Views, Bootstrap, JavaScript
- **Banco de Dados**: SQL Server + Dapper (SQL puro, sem ORM)
- **Autenticação**: JWT Bearer Tokens
- **Rate Limiting**: AspNetCoreRateLimit
- **Testes**: xUnit, TestContainers (MsSQL), Moq, FluentAssertions, Bogus, Coverlet
- **Infraestrutura**: Docker + docker-compose

---

## 🔐 Autenticação

A autenticação é feita via **JWT Bearer Tokens**. Endpoints protegidos exigem um token válido no header `Authorization`.

---

## 🗄️ Acesso a Dados

Todas as interações com o banco de dados utilizam **Dapper** com queries SQL puras — sem ORM (Entity Framework). O projeto `DatabaseAccess` contém toda a lógica de queries, e `DatabaseConnection` gerencia a conexão com o SQL Server.

---

## 🧪 Testes

Os testes estão divididos em dois projetos:

- **`ShredStoreTests`** — Testes de integração usando TestContainers (sobe um container real de MsSQL), Moq para mocks, FluentAssertions para asserções legíveis e Bogus para geração de dados falsos.
- **`ShredStoreApiTests`** — Testes de API.

A cobertura de código é rastreada via **Coverlet**.

Para rodar os testes:

```bash
dotnet test
```

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET 7+ SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- SQL Server (ou use Docker)

### Executando com Docker

```bash
docker-compose up --build
```

### Executando Localmente

1. Clone o repositório:

```bash
git clone https://github.com/YouSuckAtCoding/ShredStoreRevamp.git
cd ShredStoreRevamp
```

2. Atualize a connection string no `appsettings.json`.

3. Execute a API:

```bash
dotnet run --project ShredStore
```

4. Execute o frontend MVC:

```bash
dotnet run --project ShredStorePresentation
```

---

## 📁 Estrutura do Projeto

```
ShredStoreRevamp/
├── ShredStore/                  # Web API
├── ShredStorePresentation/      # Frontend MVC (Razor Views)
├── Application/                 # Lógica de negócio, serviços, modelos
├── Contracts/                   # DTOs e interfaces
├── DatabaseAccess/              # Queries Dapper
├── DatabaseConnection/          # Conexão SQL Server
├── ShredStoreTests/             # Testes de integração
├── ShredStoreApiTests/          # Testes de API
├── docker-compose.yml
└── README.md
```

---

## 📄 Licença

Este projeto é para fins educacionais/portfólio.
