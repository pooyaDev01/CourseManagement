# CourseManagement

A small ASP.NET Core Web API project built to practice backend architecture, automated testing, and real-world Git workflow.

## Architecture

The project is organized into four main layers:

- **CourseManagement.Api** — HTTP API, Controllers, routing, dependency injection.
- **CourseManagement.Application** — DTOs, service contracts, application/business logic.
- **CourseManagement.Entities** — Domain entities such as `Course` and `Lesson`.
- **CourseManagement.Infrastructure** — EF Core, `DbContext`, repositories, and database-related implementations.

The project follows a simplified **Clean Architecture** approach with Separation of Concerns and Dependency Inversion.

## Domain

### Course

A course contains:

- `Id`
- `Title`
- `Description`
- `Price`

### Lesson

A lesson contains:

- `Id`
- `Title`
- `DurationInMinutes`
- `CourseId`

`Lesson.CourseId` represents the relationship between a Lesson and its Course.

## API

The main Course endpoints follow this structure:

```text
GET    /api/course
GET    /api/course/{id}
POST   /api/course
PUT    /api/course/{id}
DELETE /api/course/{id}
```

The same approach is used for Lesson endpoints.

## Testing

The project uses:

- **xUnit** for test execution
- **Moq** for mocking dependencies in unit tests
- **FluentAssertions** for readable assertions
- **AutoFixture** for test data generation
- **ASP.NET Core WebApplicationFactory** for integration testing
- **EF Core In-Memory Database** for isolated integration-test environments
- **ITestOutputHelper** for test output/debug logging

### Test levels

#### Service Tests

Service dependencies are mocked so that the application/business logic can be tested independently.

#### Controller Tests

Controller behavior and HTTP results are tested independently of the real database and infrastructure.

#### Integration Tests

Integration tests execute the real application pipeline:

```text
HttpClient
    ↓
Controller
    ↓
Service
    ↓
Repository
    ↓
EF Core
    ↓
In-Memory Database
```

Each integration test uses an isolated In-Memory database so tests do not depend on data created by other tests.

## Current Testing Progress

### Course

- Service Tests — completed
- Controller Tests — completed
- Integration Tests — completed

### Lesson

- Service logic — completed
- Controller Tests — completed
- Integration Tests — completed

For Lesson creation, the important business rule is that the referenced `CourseId` must belong to an existing Course.

Expected flow:

```text
Add Lesson
    ↓
Check CourseId
    ↓
Course exists?
   /       \
 No         Yes
 ↓           ↓
null       Create Lesson
 ↓           ↓
404        Success
```

The Service handles the business decision, while the Controller converts a `null` result into an HTTP `404 Not Found`. This keeps HTTP concerns out of the Application layer.

## Integration Test Goals

The main Course and Lesson integration scenarios include:

- Get all
- Get by ID
- Create
- Update
- Delete
- Not-found scenarios
- Lesson creation with a valid `CourseId`
- Lesson creation with a non-existent `CourseId`

## Development Workflow

Git workflow is practiced using feature branches and Pull Requests.

Typical flow:

```text
main
  ↓
feature/test/...
  ↓
Commit
  ↓
Push
  ↓
Pull Request
  ↓
Review / Merge
```

## Learning Goals

This project is primarily intended to consolidate practical knowledge of:

- ASP.NET Core Web API
- Clean Architecture concepts
- Separation of Concerns
- Dependency Injection
- Repository pattern
- EF Core
- xUnit
- Moq
- Unit Testing
- Integration Testing
- Test isolation
- Git branching
- Pull Requests
- TDD-oriented development

