# Ludik — Gamified web platform for secondary education

> Full-stack project that turns student participation and achievements into **medals, coins, rankings and rewards**, with a **progress bar** and an **equivalence table (grading GPS)**, to give **immediate feedback** and boost motivation. **ASP.NET Core 8 (Clean Architecture)** backend, **React** frontend, self-hosted with **Docker Compose on Coolify**.

**Live demo:** https://ludik.lucasgiusiano.uy · **API docs (Swagger):** https://ludik-api.lucasgiusiano.uy/swagger

---

## Problem & value proposition

In secondary school, feedback arrives late and students can't see their progress day to day, which lowers engagement. **Ludik** provides continuous, visible and actionable feedback (progress, goals, achievements and rewards) that **reduces teacher workload** and **improves student motivation**.

---

## Architecture (Clean Architecture + SPA)

- **Frontend**: React (SPA), consumes the REST API and manages state on the client.
- **API**: ASP.NET Core 8, REST controllers; **CQRS with MediatR** for use cases.
- **Domain / Application**: `LogicaNegocio` (entities, rules) and `LogicaAplicacion` (use cases, DTOs).
- **Infrastructure / Data**: `AccesoDatos` with **EF Core 8** on **SQL Server**.
- **Jobs**: **Hangfire** for background tasks (e.g., weekly "Kudos" reset).
- **Auth**: **ASP.NET Identity + JWT**.
- **File storage**: Azure Blob Storage API (SAS URLs). In this deployment it is served by **Azurite**, Microsoft's open-source emulator, so no Azure account is needed.
- **DevOps**: Swagger/OpenAPI, Docker, Docker Compose, Coolify.

---

## Key features

- **Medals** (teacher-awarded and peer-awarded, with validation), **coins** and **rewards** (shop).
- **Rankings** by medal type; **milestones** and **boosters** (temporary multipliers).
- **Personal goals** per subject + **progress bar** with next steps.
- **Equivalence table (GPS)** that translates medals into an objective **grade**.
- **Group/subject management** (codes/QR), profiles and avatars, password recovery.

---

## Main technologies

- **Backend**: ASP.NET Core 8, CQRS (MediatR), EF Core 8, Identity + JWT, Hangfire, Swagger.
- **Frontend**: React (SPA, Create React App), Redux Toolkit, React Query, Axios.
- **Database**: SQL Server.
- **Storage**: Azure Blob Storage API (Azurite in self-hosted deployments).
- **Infrastructure**: Docker, Docker Compose, Coolify, nginx.
- **Testing**: xUnit + Moq.

---

## Repository structure

~~~plaintext
Ludik/
├── LudikAPI/                    # .NET 8 backend solution (Clean Architecture)
│   ├── AccesoDatos/             # EF Core, migrations, repositories
│   ├── LogicaNegocio/           # Domain entities and rules
│   ├── LogicaAplicacion/        # Use cases (CQRS/MediatR), DTOs
│   ├── WebApi/                  # ASP.NET Core API, controllers, Identity/JWT, Swagger
│   ├── PruebasUnitarias/        # Unit tests (xUnit + Moq)
│   ├── PruebasIntegracion/      # Integration tests
│   └── Dockerfile               # API image (multi-stage build)
│
├── ludikclient/                 # React frontend (SPA)
│   ├── src/
│   │   ├── components/          # Reusable components
│   │   ├── features/            # Modules (groups, medals, rewards, etc.)
│   │   ├── hooks/               # Hooks (React Query)
│   │   ├── lib/                 # Axios setup and utilities
│   │   └── pages/               # Main pages
│   ├── Dockerfile               # Client image (build + nginx)
│   ├── nginx.conf               # SPA fallback and caching
│   └── package.json
│
├── docs/                        # Diagrams and technical documentation
├── docker-compose.yml           # Full stack: sqlserver, azurite, api, client
└── README.md                    # This document
~~~

**Exposed controllers (examples)**:
`Login`, `Profesor`, `Estudiante`, `Grupo`, `Medalla`, `AsignacionMedallas`, `TablaEquivalencia`, `TablaClasificacion`, `Tienda`, `Recompensa`, `BarraProgreso`, `Avatar`, `Kudo`, `RecuperarContrasena`.

---

## Getting started (local)

### Requirements

- **.NET SDK 8**
- **Node.js ≥ 20** / **npm ≥ 10**
- **SQL Server** (local or container)
- **Azurite** or an Azure Blob Storage account (optional, needed for images)

### Backend (API)

Configure `LudikAPI/WebApi/appsettings.json` (or environment variables):

- `ConnectionStrings:DefaultConnection` → SQL Server connection string.
- `JwtSettings` → key, issuer and audience for JWT.
- `ConnectionStrings:AzureStorage` and `StorageContainerName` → blob storage (in Development, `StorageConnection` is used, pointing to Azurite).

Run the server (in Development, image seeding runs on startup):

~~~bash
cd LudikAPI/WebApi
dotnet build
dotnet run
~~~

The API applies pending EF Core migrations on startup. Swagger is available at `/swagger`.

### Frontend (SPA)

The API base URL is read from the `REACT_APP_API_URL` environment variable (default: `https://localhost:7215`, see `ludikclient/src/app/url.js`).

~~~bash
cd ludikclient
npm ci
npm start
~~~

Production build:

~~~bash
REACT_APP_API_URL=https://your-api-url npm run build
~~~

### Tests

~~~bash
cd LudikAPI
dotnet test
~~~

---

## Self-hosted deployment (Docker Compose / Coolify)

The whole stack runs from `docker-compose.yml`:

| Service | Image | Internal port | Purpose |
|---|---|---|---|
| `sqlserver` | `mssql/server:2022` | 1433 | Database |
| `azurite` | `azure-storage/azurite` | 10000 | Blob storage (profile and avatar images) |
| `api` | `LudikAPI/Dockerfile` | 8080 | ASP.NET Core API |
| `client` | `ludikclient/Dockerfile` | 80 | React SPA served by nginx |

### Environment variables

| Variable | Description |
|---|---|
| `MSSQL_SA_PASSWORD` | SQL Server `sa` password (min. 8 chars, upper, lower, digit) |
| `JWT_KEY` | JWT signing key (e.g., `openssl rand -base64 48`) |
| `BLOB_ACCOUNT` | Azurite storage account name |
| `BLOB_KEY` | Azurite account key (e.g., `openssl rand -base64 64`) |
| `BLOB_PUBLIC_URL` | Public URL of the blob endpoint, e.g. `https://ludik-blob.example.com` |
| `REACT_APP_API_URL` | Public URL of the API, baked into the client at build time |

### Deploying on Coolify

1. Create a new resource of type **Docker Compose** from this repository (branch `main`, file `/docker-compose.yml`).
2. Set the environment variables above.
3. Assign one domain per service (the port after the domain is the container's internal port):
   - `client` → `https://ludik.example.com:80`
   - `api` → `https://ludik-api.example.com:8080`
   - `azurite` → `https://ludik-blob.example.com:10000`
4. Point the three DNS records to the server and deploy.

**Notes**
- Migrations and the avatar image seed (`SeedArchivos=true`) run automatically when the API starts.
- The blob endpoint must be publicly reachable: the API returns time-limited SAS URLs (1 hour) that the browser opens directly.
- `REACT_APP_API_URL` is compiled into the client, so changing it requires a full redeploy.
- SQL Server needs at least ~2 GB of RAM.

---

## Highlighted modules (business relevance)

- **Grading GPS (equivalence table)**: defines grade steps based on increasing medal combinations; feeds the **progress bar** and makes evaluation **objective and transparent**.
- **Internal economy**: medals → coins → **rewards shop** (configurable benefits).
- **Rankings** and **Kudos** (resettable) for short, healthy competition cycles.
- **Avatars** with attributes (images managed in blob storage).

> Motivation is not decorative: **UX** is a functional requirement so that rewards feel worthwhile to the student.

---

## Why it matters for a recruiter

- **Real, user-oriented product** (teachers and students) that solves an educational problem with **immediate feedback**.
- **Maintainable, scalable architecture** (Clean Architecture + CQRS + background jobs).
- **Modern engineering practices**: automated tests, API documentation (Swagger), containerization and self-hosted deployment.

---

## Roadmap (suggested)

- MFA and periodic JWT rotation.
- Metrics/telemetry and a teacher analytics dashboard.
- Internationalization (i18n) and accessibility (WCAG).
- Offline mode for classrooms with limited connectivity.

---

## Credits

Academic project (Universidad ORT Uruguay) developed by **Renato Ríos**, **Lucas Giusiano** and **Manuel Martínez**.

> For the full details of requirements, design decisions and project results, see the technical document included in the repository.
