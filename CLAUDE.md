# CLAUDE.md — Seem Project Guide

## Project Overview

Seem is a task and project management platform with a .NET 8 backend API and an Angular 21 frontend SPA. It includes project/task management with kanban boards, a knowledge base, automation/recurring tasks, and analytics dashboards.

## Repository Structure

```
Seem/
├── src/
│   ├── Seem.Domain/           # Domain layer — entities, value objects, enums, events, interfaces
│   ├── Seem.Application/      # Application layer — CQRS commands/queries, DTOs, validators, behaviors
│   ├── Seem.Infrastructure/   # Infrastructure layer — EF Core, PostgreSQL, persistence, services
│   └── Seem.Api/              # Presentation layer — ASP.NET Core Web API, controllers, middleware
├── tests/
│   ├── Seem.Domain.Tests/     # Domain unit tests (xUnit)
│   ├── Seem.Application.Tests/# Application layer tests (xUnit)
│   └── Seem.Api.Tests/        # API integration tests (xUnit)
├── client/                    # Angular 21 SPA frontend
│   └── src/app/
│       ├── core/              # Shared services (ApiService) and models
│       ├── features/          # Feature modules (tasks, projects, analytics, etc.)
│       └── layout/            # Shell layout (header, sidebar)
├── docker-compose.yml         # Docker setup (API + PostgreSQL 17)
├── Seem.sln                   # .NET solution file
└── .config/dotnet-tools.json  # dotnet-ef tool manifest
```

## Architecture

**Clean Architecture** with **CQRS** (MediatR) pattern. Dependencies flow inward:

```
Api → Application → Domain
Infrastructure → Application → Domain
```

- **Domain**: Entities with private constructors + static factory methods, value objects, domain events, enums
- **Application**: MediatR commands/queries with FluentValidation, pipeline behaviors (validation, logging)
- **Infrastructure**: EF Core DbContext (PostgreSQL/Npgsql), entity configurations, interceptors, migrations
- **Api**: REST controllers, global exception handling middleware, Swagger/OpenAPI

## Quick Commands

### Backend (.NET)

```bash
# Build the entire solution
dotnet build Seem.sln

# Run all tests
dotnet test Seem.sln

# Run tests for a specific project
dotnet test tests/Seem.Domain.Tests
dotnet test tests/Seem.Application.Tests
dotnet test tests/Seem.Api.Tests

# Run the API (from repo root)
dotnet run --project src/Seem.Api

# EF Core migrations (from repo root)
dotnet ef migrations add <Name> --project src/Seem.Infrastructure --startup-project src/Seem.Api
dotnet ef database update --project src/Seem.Infrastructure --startup-project src/Seem.Api

# Docker (API + PostgreSQL)
docker-compose up
```

### Frontend (Angular)

```bash
cd client

# Install dependencies
npm install

# Development server (localhost:4200)
npm start

# Production build
npm run build

# Run tests (Vitest)
npm test
```

## Tech Stack

### Backend
- **.NET 8.0** / C# with nullable reference types and implicit usings
- **Entity Framework Core 8.0.11** with **Npgsql** (PostgreSQL 17)
- **MediatR 14** for CQRS command/query dispatching
- **FluentValidation 12** for request validation
- **AutoMapper 16** for object mapping
- **Serilog** for structured logging
- **Quartz.NET** for background job scheduling (configured, not yet active)
- **Swashbuckle** for Swagger/OpenAPI docs
- **xUnit** with **coverlet** for testing and code coverage

### Frontend
- **Angular 21.2** with standalone components and lazy-loaded routes
- **Angular Material 21.2** (Material 3) with SCSS theming
- **RxJS 7.8** for reactive patterns
- **TypeScript 5.9** in strict mode
- **Vitest 4** for unit testing
- **Prettier** for code formatting

### Infrastructure
- **PostgreSQL 17** (Alpine) via Docker
- Multi-stage **Dockerfile** for API
- **docker-compose.yml** orchestrates API + database

## Key Patterns and Conventions

### Domain Entities
- Inherit from `AuditableEntity` (Id, CreatedAt, UpdatedAt, DeletedAt, IsDeleted) or `BaseEntity` (Id, DomainEvents)
- Use **private constructors** with **static `Create()` factory methods** for invariant enforcement
- Collections exposed as `IReadOnlyCollection<T>` backed by private `List<T>` fields
- Domain events dispatched via `AddDomainEvent()` on the base entity
- Business rule violations throw `DomainException` or specific exceptions like `InvalidTaskTransitionException`

### CQRS Commands/Queries
- Commands: `Features/{Feature}/Commands/{Name}/{Name}Command.cs` — implements `IRequest<TResponse>`
- Handlers: `Features/{Feature}/Commands/{Name}/{Name}CommandHandler.cs` — implements `IRequestHandler<TCommand, TResponse>`
- Validators: `Features/{Feature}/Commands/{Name}/{Name}CommandValidator.cs` — extends `AbstractValidator<TCommand>`
- DTOs: `Features/{Feature}/DTOs/{Name}Dto.cs` — immutable records with `init` properties

### API Controllers
- Route: `api/[controller]` with `[ApiController]` attribute
- Inject `IMediator` for CQRS or `IApplicationDbContext` for direct queries
- Return wrapped responses: `{ data: ... }` for consistency with frontend `ApiResponse<T>`
- Use `CancellationToken` on all async endpoints

### Error Handling
- Global `ExceptionHandlingMiddleware` maps exceptions to HTTP status codes:
  - `ValidationException` → 400 Bad Request
  - `DomainException` → 422 Unprocessable Entity
  - `KeyNotFoundException` → 404 Not Found
  - Unhandled → 500 Internal Server Error
- Response format: `{ errors: string[], statusCode: int }`

### Database / EF Core
- `SeemDbContext` implements both `IApplicationDbContext` and `IUnitOfWork`
- Entity configurations in `Infrastructure/Persistence/Configurations/` using Fluent API
- `AuditableEntityInterceptor` auto-sets `CreatedAt`/`UpdatedAt` timestamps on save
- Soft deletes via `IsDeleted` flag with query filters
- PostgreSQL JSONB columns for flexible data (Metadata, Preferences, TriggerConditions)
- Migrations stored in `Infrastructure/Migrations/`

### Frontend Angular
- **Standalone components** (no NgModules) — all components use `standalone: true`
- **Lazy-loaded routes** via dynamic `import()` in `app.routes.ts`
- **Feature-based folder structure**: `features/{name}/pages/{page-name}/`
- **Core services** in `core/services/` with `providedIn: 'root'`
- **Models/interfaces** in `core/models/` matching backend DTOs
- `ApiService` wraps `HttpClient` and unwraps `ApiResponse<T>.data`
- SCSS with Angular Material 3 theming (Azure primary, custom palettes)

## Code Style

### C# (.editorconfig enforced)
- 4-space indentation
- File-scoped namespaces (`namespace Foo;`)
- `var` for built-in types and when type is apparent
- System usings sorted first
- New line before all opening braces
- Using directives outside namespace

### TypeScript/Angular
- 2-space indentation
- Strict mode (`strict: true` in tsconfig)
- Single quotes (Prettier)
- 100 character print width (Prettier)
- SCSS for component styles

### General
- UTF-8 encoding, LF line endings
- Trim trailing whitespace (except Markdown)
- Insert final newline

## Domain Model Summary

### Core Entities
- **Project** — aggregate root with boards and tasks; has Key (1-5 chars), auto-generates TaskKeys (e.g., `PROJ-1`)
- **TaskItem** — aggregate root with status transitions, subtasks, tags, comments, dependencies, time tracking
- **Board** / **BoardColumn** — kanban boards with columns, WIP limits, status mapping
- **Article** / **Space** — knowledge base with article revisions and hierarchy
- **AutomationRule** / **RecurringTask** / **Reminder** — automation subsystem

### Key Enums
- `TaskItemStatus`: Todo, InProgress, InReview, Blocked, Done, Cancelled
- `Priority`: Lowest, Low, Medium, High, Critical

### Value Objects
- `TaskKey` — record wrapping `"{PROJECT_KEY}-{number}"` format
- `Color` — validated hex color (`#RRGGBB`)
- `DateRange` — start/end date pair

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/health` | Health check |
| GET | `/api/projects` | List all projects with task counts |
| GET | `/api/projects/{id}` | Get project with boards and columns |
| POST | `/api/tasks` | Create a new task (via MediatR) |
| GET | `/api/tasks/{id}` | Get task by ID (not yet implemented) |

## Configuration

- **Connection string**: `appsettings.json` → `ConnectionStrings:DefaultConnection` (PostgreSQL)
- **CORS**: Allows `http://localhost:4200` (Angular dev server)
- **Swagger UI**: Available in Development environment at `/swagger`
- **Auto-migration**: EF migrations applied automatically on startup in Development mode
- **Docker**: API on port 5000, PostgreSQL on port 5432 (credentials: `seem`/`seem_dev_password`)

## Testing

- **Backend**: xUnit with coverlet for coverage. Run `dotnet test Seem.sln` from the repo root.
- **Frontend**: Vitest with Angular TestBed. Run `npm test` from `client/`.
- Test projects mirror the source structure: `Seem.Domain.Tests`, `Seem.Application.Tests`, `Seem.Api.Tests`
- Global usings in test projects: `global using Xunit;`

## Important Notes

- The project uses **file-scoped namespaces** throughout all C# files
- Entities use **private setters** — all mutations go through domain methods
- TaskItem status changes go through `ValidateTransition()` which enforces a state machine
- The `IApplicationDbContext` abstraction is the primary data access interface (no generic repositories implemented yet)
- Quartz.NET is included as a dependency but background jobs are not yet configured
- JWT authentication package is referenced but auth is not yet implemented
- Frontend feature pages are scaffolded but mostly contain placeholder content
