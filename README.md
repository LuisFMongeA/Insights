# Insights

A **microservices-based backend platform** built with **.NET 8**, designed to aggregate geolocation, weather, audit, and statistics data. Built as a portfolio and learning project to demonstrate real-world senior-level backend architecture decisions.

---

## Architecture Overview

Insights follows **Clean Architecture (Onion Architecture)** with a clear dependency rule: every dependency points inward toward the domain. The domain layer has zero external dependencies.

```
┌─────────────────────────────────────────────┐
│              Insights.Gateway               │  ← REST API + JWT Auth + RabbitMQ Publisher
├─────────────────────────────────────────────┤
│           Application Services             │  ← Handlers, Validators, DTOs
├─────────────────────────────────────────────┤
│         Insights.Infrastructure.Data        │  ← EF Core, Repositories, Migrations
├─────────────────────────────────────────────┤
│             Insights.Domain                 │  ← Entities, Value Objects, Interfaces
└─────────────────────────────────────────────┘
```

### Microservices

| Service | Responsibility |
|---|---|
| `Insights.Gateway` | Public-facing REST API. Authenticates requests via JWT RS256, validates input, publishes events to RabbitMQ |
| `Insights.AuthAPI` | Issues and validates JWT tokens using RSA key pairs |
| `Insights.Processor` | Consumes RabbitMQ events, persists audit and stats data |
| `Insights.Services.AuditAPI` | Manages audit trail data |
| `Insights.Services.StatsAPI` | Manages statistics aggregation |
| `Insights.Services.CitiesAPI` | City resolution microservice |
| `Insights.Services.CountriesAPI` | Country data microservice |
| `Insights.Services.WeatherAPI` | Weather data microservice |
| `Insights.ServiceBus` | RabbitMQ infrastructure abstraction |
| `Insights.SharedKernel` | Middlewares, extensions, shared utilities |
| `Insights.Contracts` | Shared event definitions and queue names |
| `Insights.Domain` | Entities, Value Objects, Domain Exceptions |
| `Insights.Infrastructure.Data` | EF Core, Repositories, Unit of Work, Migrations |

---

## Tech Stack

- **Runtime**: .NET 8
- **Database**: PostgreSQL (Neon cloud) via EF Core Code First
- **Messaging**: RabbitMQ (Docker)
- **Auth**: JWT RS256 (asymmetric key pair)
- **Validation**: FluentValidation + Action Filters
- **Logging**: Serilog (structured logging)
- **Testing**: xUnit + Moq + WebApplicationFactory
- **Frontend**: Angular (separate repository)

---

## Design Patterns Implemented

### Domain-Driven Design (DDD)
- **Value Objects**: `Coordinates` and `CountryCode` as C# records with private constructors
- **Rich Domain Models**: `AuditEntry.Create()` and `StatEntry.Create()` factory methods enforce business rules at construction time
- **Domain Exceptions**: Custom `DomainException` class
- **EF Core `OwnsOne`** for Value Object persistence

### Architectural Patterns
- **Repository Pattern + Unit of Work** — abstracts data access, keeps transactions explicit
- **Outbox Pattern** — `OutboxMessage` + `OutboxWorker` guarantees at-least-once message delivery even if RabbitMQ is temporarily unavailable
- **Strategy Pattern** — `ICityResolutionStrategy` with `IEnumerable<ICityResolutionStrategy>` injection; each strategy declares `CanHandle()` before `ResolveAsync()`
- **Decorator Pattern** — `CachedGeoService` wraps `GeoService` transparently via DI, adding caching without modifying the original class
- **Template Method** — `RabbitMqConsumerBase` defines the skeleton for message consumption; subclasses implement only what varies

### Event-Driven
- Gateway publishes events to RabbitMQ on each request
- Processor consumes asynchronously and persists data independently
- Outbox Pattern ensures no event is lost between the two

---

## Security (OWASP Top 10)

| Concern | Implementation |
|---|---|
| Authentication | JWT RS256 — asymmetric keys, short-lived tokens |
| Input Validation | FluentValidation + `ValidationFilter` + `[ValidateDtoAttribute]` |
| Rate Limiting | Gateway: 100 req/min — AuthAPI: 10 req/min |
| Security Headers | Custom middleware on every response |
| Timing Attacks | `CryptographicOperations.FixedTimeEquals` for token comparison |
| Server Fingerprinting | `Kestrel AddServerHeader = false` |
| Swagger Exposure | Dev environment only |
| Dependency Vulnerabilities | NuGet vulnerability scanning enabled |

---

## Testing

### Unit Tests — `Insights.Tests.Unit` (8/8 passing)
- **Subject**: `GeoService`
- **Mocked dependencies**: `ICityResolutionStrategy`, `ICountriesHttpClient`, `IWeatherHttpClient`, `IOutboxRepository`
- Tests cover strategy selection, external HTTP client calls, and outbox persistence

### Integration Tests — `Insights.Tests.Integration` (4/4 passing)
- **Subject**: `GeoController`
- Uses `WebApplicationFactory` with a custom `GatewayWebApplicationFactory`
- `TestAuthHandler` simulates JWT authentication without real tokens
- Scenarios covered:
  - `401` — no token provided
  - `400` — no parameters
  - `400` — latitude without longitude
  - `200 OK` — valid authenticated request

---

## Key Engineering Decisions

**Why `IEnumerable<ICityResolutionStrategy>` instead of a single service?**
Open/Closed Principle — adding a new city resolution strategy requires zero changes to `GeoService`. Register the new strategy in DI and it is automatically discovered.

**Why Outbox Pattern instead of direct RabbitMQ publish?**
If RabbitMQ is unavailable at publish time, a direct call fails silently and the event is lost. The Outbox writes to the database in the same transaction as the business operation, then a background worker reliably delivers it. Guarantees consistency between the database and the message broker.

**Why Decorator Pattern for caching?**
`CachedGeoService` is registered over `GeoService` in the DI container. Callers depend on `IGeoService` and are completely unaware of caching. Caching can be added, modified, or removed without touching the business logic.

**Why RS256 over HS256 for JWT?**
RS256 uses an asymmetric key pair. The private key signs tokens (held only by AuthAPI). Any other service can verify tokens using the public key without ever having access to the private key. Significantly safer in a multi-service architecture.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) — for RabbitMQ
- [PostgreSQL](https://www.postgresql.org/) or a Neon cloud connection string
- User Secrets configured for connection strings (never committed)

---

## Running Locally

**1. Start RabbitMQ:**
```bash
./rabbitmq-launch.ps1
```

**2. Configure User Secrets for each service** (connection strings, RSA keys):
```bash
dotnet user-secrets set "ConnectionStrings:Default" "your-connection-string" --project Insights.Gateway
```

**3. Apply migrations:**
```bash
dotnet ef database update --project Insights.Infrastructure.Data
```

**4. Run the services:**
```bash
dotnet run --project Insights.Gateway
dotnet run --project Insights.AuthAPI
dotnet run --project Insights.Processor
```

---

## Project Status

This is an active learning and portfolio project. Features under development:

- [ ] CQRS with MediatR + Pipeline Behaviors
- [ ] Circuit Breaker with Polly
- [ ] Health Checks endpoints
- [ ] Saga Pattern for distributed transactions
- [ ] Request/Reply pattern over RabbitMQ
- [ ] Dedicated `Insights.Application` layer (separate from Gateway)
- [ ] Domain Events replacing direct Outbox creation in services

---

## Author

**Luis Monge** — Senior .NET & Angular Developer  
[GitHub](https://github.com/LuisFMongeA)
