# System Patterns and Architecture - EternaInvites Backend

## Architecture Overview

The system follows a clean architecture pattern with clear separation of concerns:

```
API Layer (Controllers) → Service Layer → Repository Layer → Database Layer
```

## Core Design Patterns

### 1. Repository Pattern

- **Base Repository**: `_BaseRepository<T>` provides common CRUD operations
- **Specific Repositories**: Inherit from base and add entity-specific methods
- **Interface Segregation**: Each repository has its own interface (e.g., `ITab_ClientesRepository`)

**Example Implementation**:

```csharp
public interface ITab_ClientesRepository : _IBaseRepository<Tab_Clientes>
{
    Task<Tab_Clientes?> GetByUrlAsync(string url, bool? activo = true);
}

public class Tab_ClientesRepository : _BaseRepository<Tab_Clientes>, ITab_ClientesRepository
{
    // Implementation inherits base functionality and adds specific methods
}
```

### 2. Service Layer Pattern

- **Base Service**: `_BaseService<TModel, TEntity>` handles mapping between models and entities
- **Specific Services**: Business logic for each domain entity
- **Interface-Based**: All services implement interfaces for dependency injection

### 3. Controller Pattern

- **Base Controller**: `_BaseController<TModel, TEntity>` provides standard CRUD endpoints
- **Specific Controllers**: Add domain-specific endpoints
- **RESTful Design**: Follows REST conventions with additional action methods

## Data Access Strategy

### Hybrid ORM Approach

- **Entity Framework Core**: For complex operations, migrations, and relationships
- **Dapper**: For high-performance queries and custom SQL operations
- **Dapper.Contrib**: For simple CRUD operations with attributes

### Base Table Pattern

All entities inherit from `_BaseTable`:

```csharp
public abstract class _BaseTable
{
    public int Id { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public DateTime FechaModificacion { get; set; } = DateTime.Now;
    public Guid Uui { get; set; } = Guid.NewGuid();
}
```

## Key Architectural Decisions

### 1. UUID Usage

- Primary keys are integers for performance
- UUIDs (`Uui`) for external references and security
- Templates referenced by UUID to avoid exposing internal IDs

### 2. JSON Column Strategy

Templates store components as JSON for flexibility:

```csharp
[Column("ComponentesJson", TypeName = "json")]
public string? ComponentesJson { get; set; }

[NotMapped]
public List<ComponenteHTML>? Componentes
{
    get => JsonSerializer.Deserialize<List<ComponenteHTML>>(ComponentesJson);
    set => ComponentesJson = JsonSerializer.Serialize(value);
}
```

### 3. Soft Delete Pattern

- `Activo` field for soft deletes
- Repository methods filter by `Activo` by default
- Data preservation for audit and recovery

### 4. Exception Handling Strategy

- Custom `ExceptionHelper` for HTTP status codes
- Global exception middleware for consistent error responses
- Validation at service layer with meaningful error messages

## Security Architecture

### Authentication Flow

1. User credentials validated against `Tab_Usuarios`
2. JWT token generated with user claims
3. Token required for protected endpoints
4. Public endpoints (invitation access) bypass authentication

### Authorization Levels

- **Public**: Invitation retrieval by URL
- **Authenticated**: All administrative operations
- **Future**: Role-based access control

## Data Flow Patterns

### 1. Client Creation Flow

```
Controller → Service (validation) → Repository (uniqueness check) → Database
```

### 2. Invitation Retrieval Flow

```
Public Controller → Service → Repository (by URL) → Template Repository → Response
```

### 3. Template Management Flow

```
Authenticated Controller → Service (component validation) → Repository → Database
```

## Component Relationships

### Core Entities

- **Tab_Usuarios**: System administrators
- **Tab_Clientes**: Event clients with unique URLs
- **Tab_Plantillas**: HTML templates with component definitions

### Relationships

- Client → Template (via UuiPlantilla)
- Template → Components (JSON serialization)
- All entities → BaseTable (inheritance)

## Extension Points

- **New Entity Types**: Inherit from `_BaseTable` and create repository/service
- **Custom Validation**: Add to service layer
- **Additional Endpoints**: Extend specific controllers
- **Custom Queries**: Add to repository interfaces
