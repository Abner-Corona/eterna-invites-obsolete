# Technical Context - EternaInvites Backend

## Technology Stack

- **Framework**: ASP.NET Core 8.0 (with .NET 9.0 compatibility)
- **Database**: MySQL with Entity Framework Core
- **ORM**: Entity Framework Core + Dapper (hybrid approach)
- **Authentication**: JWT Bearer tokens
- **Logging**: Serilog with structured logging
- **API Documentation**: Swagger/OpenAPI
- **Containerization**: Docker support included

## Project Structure

```
EternaInvites.Back/
├── Database/           # Entity Framework context, migrations, table definitions
├── Domain/            # Business logic, exceptions, helpers
├── Repository/        # Data access layer using Dapper + EF Core
├── Service/           # Business service layer
├── Server/            # ASP.NET Core API controllers and configuration
└── memory-bank/       # Project documentation and context
```

## Database Schema

### Core Tables:

- **Tab_Usuarios**: Admin users with authentication
- **Tab_Clientes**: Client/event information with unique URLs
- **Tab_Plantillas**: HTML templates with component definitions

### Key Relationships:

- Clients reference Templates via `UuiPlantilla` (UUID foreign key)
- All tables inherit from `_BaseTable` (Id, Activo, FechaCreacion, FechaModificacion)

## Development Setup

### Prerequisites:

- .NET 8.0 SDK or higher
- MySQL Server
- Visual Studio Code or Visual Studio

### Configuration:

- Connection string in `appsettings.json` or `appsettings.Development.json`
- JWT configuration for authentication
- CORS configured for frontend integration

### Running the Application:

```powershell
# Using dotnet CLI
Set-Location "c:\Users\coron\OneDrive\Documentos\Git\EternaInvites\EternaInvites.Back"
dotnet run --project Server

# Using VS Code task (watchTaskName available)
dotnet watch run --project Server/Server.csproj

# Using Docker
docker build -t eternainvites-backend -f Server/Dockerfile .
docker run -p 8080:8080 eternainvites-backend
```

### Build Status:

- ✅ Builds successfully in ~12.8 seconds
- ✅ All dependencies resolved correctly
- ✅ No compilation errors or warnings
- ✅ Ready for production deployment

## Technical Constraints

- **Database**: MySQL required for JSON column support in templates
- **Authentication**: JWT tokens with configurable expiration
- **CORS**: Configured for localhost:9000 (frontend development)
- **Entity Framework**: Code-first approach with migrations applied
- **API Versioning**: Routes follow /api/[controller] pattern
- **Environment**: Windows development environment with PowerShell

## Current Implementation Status

### Repository Layer ✅

- Base repository pattern fully implemented
- All repositories inherit from `_BaseRepository<T>`
- Dapper and Entity Framework hybrid approach working
- Tab_ClientesRepository, Tab_UsuariosRepository, Tab_PlantillasRepository functional

### Service Layer ✅

- All business logic properly separated
- Model/Entity mapping implemented
- ClientesService with invitation retrieval logic
- LoginService with JWT authentication
- PlantillasService for template management

### API Layer ✅

- RESTful endpoints following standard conventions
- Public invitation access without authentication
- Protected admin endpoints with JWT
- Global exception handling middleware
- Swagger documentation available

## Technical Constraints

- **Database**: MySQL required for JSON column support in templates
- **Authentication**: JWT tokens with configurable expiration
- **CORS**: Configured for localhost:9000 (frontend development)
- **Entity Framework**: Code-first approach with migrations
- **API Versioning**: Routes follow /api/[controller] pattern

## Dependencies

### Key NuGet Packages:

- Microsoft.EntityFrameworkCore.Tools
- Pomelo.EntityFrameworkCore.MySql
- Dapper
- Dapper.Contrib
- Microsoft.AspNetCore.Authentication.JwtBearer
- Serilog.AspNetCore
- CryptSharp.Core (for password hashing)

## Security Considerations

- Passwords hashed using Blowfish algorithm
- JWT tokens for API authentication
- CORS policy restricts origins
- Exception handling middleware for secure error responses
- Database connection strings in configuration files

## Performance Notes

- Dapper used for high-performance queries
- Entity Framework for complex operations and migrations
- Repository pattern for data access abstraction
- Async/await pattern throughout for scalability
