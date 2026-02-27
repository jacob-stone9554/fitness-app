# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All commands run from `backend/fitness.api/` unless noted otherwise.

**Run the API (development):**
```sh
dotnet run --project fitness.api/fitness.api.csproj
```
Runs on `http://localhost:5040` and `https://localhost:7049`. Swagger UI available at `/swagger`.

**Build:**
```sh
dotnet build
```

**EF Core migrations** (run from `fitness.api/` inner project directory):
```sh
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

**Docker (full stack — run from repo root `fitness-app/`):**
```sh
docker compose up --build
```
This spins up PostgreSQL on port 5432 and the API on port 8080.

## Configuration

Sensitive values are not in `appsettings.json`. For local development, use .NET User Secrets (configured with `UserSecretsId: c1177c21-7993-423a-b626-3331bb2dbfcf`):

```sh
dotnet user-secrets set "Jwt:Key" "<your-secret-key>"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=fitness;Username=fitness;Password=REDACTED"
```

In production/Docker, these are set via environment variables (e.g., `ConnectionStrings__DefaultConnection`).

## Architecture

The project uses a **feature-slice** layout inside `fitness.api/`:

```
Features/
  Auth/
    AuthController.cs       — POST /auth/register, POST /auth/login
    Services/TokenService.cs — JWT creation (2-hour expiry, HS256)
    Dtos/                   — RegisterRequest, LoginRequest, AuthResponse
  Users/
    UsersController.cs      — GET /users/me (authorized), GET /users/me-debug
Data/
  AppDbContext.cs           — EF Core context extending IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
  Entities/
    AppUser.cs              — Extends IdentityUser<Guid>
    UserProfile.cs          — 1-to-1 with AppUser; holds DisplayName, Units (imperial|metric)
    Workouts/
      WorkoutSession.cs     — Top-level workout record (UserId, StartedAt, EndedAt, Notes)
      WorkoutExercise.cs    — Ordered exercise within a session (Name, Order, Notes)
      WorkoutSet.cs         — Individual set (Reps, Weight, RPE, Duration, Distance, RestSeconds, IsWarmup)
Infrastructure/
  Errors/
    ErrorHandlingMiddleware.cs  — Global exception → ProblemDetails (RFC 7807)
    NotFoundException.cs        — Maps to 404
    ConflictException.cs        — Maps to 409
    AppValidationException.cs   — Maps to 400 with field-level errors dict
  Middleware/
    CorrelationIdMiddleware.cs  — Reads/generates X-Correlation-Id, injects into Serilog context
```

### Data model hierarchy

```
AppUser
  └── UserProfile (1:1)
  └── WorkoutSession (1:many)
        └── WorkoutExercise (1:many, ordered by Order)
              └── WorkoutSet (1:many, ordered by SetNumber)
```

All relationships cascade-delete. All primary keys are `Guid`.

### Middleware pipeline order (Program.cs)

1. `CorrelationIdMiddleware` — sets `X-Correlation-Id` and `TraceIdentifier`
2. `ErrorHandlingMiddleware` — catches all exceptions, returns ProblemDetails JSON
3. `SerilogRequestLogging` — structured HTTP request logs with TraceId and UserId
4. CORS (`"frontend"` policy — allows localhost:5173 and GitHub Pages)
5. Authentication / Authorization

### Authentication

JWT Bearer, HMAC-SHA256, 2-hour tokens. Claims include `sub` (user GUID), `email`, and `ClaimTypes.NameIdentifier`. User ID is extracted from `ClaimTypes.NameIdentifier` in controllers.

### Error handling pattern

Throw typed exceptions from anywhere in the call stack; `ErrorHandlingMiddleware` converts them to ProblemDetails responses. Do **not** return error status codes directly from services — throw `NotFoundException`, `ConflictException`, or `AppValidationException` instead.

### Health & diagnostics

- `GET /health` — NpgSql health check, returns 200 or 503
- `GET /ping` — liveness check
