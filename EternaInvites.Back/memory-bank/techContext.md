# Technical Context - EternaInvites Backend

## Technology Stack

- **Framework**: ASP.NET Core 8.0 (with .NET 9.0 compatibility)
- **Database**: MySQL 8.0 with Entity Framework Core
- **ORM**: Entity Framework Core + Dapper (hybrid approach)
- **Authentication**: JWT Bearer tokens
- **Logging**: Serilog with structured logging (console + MySQL)
- **API Documentation**: Swagger/OpenAPI with JWT support
- **Reverse Proxy**: Nginx 1.25 Alpine with rate limiting and security
- **Containerization**: Complete Docker infrastructure with health checks

## Infrastructure Architecture

### **Multi-Container Setup**
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Nginx Proxy    │    │  ASP.NET Core   │    │  MySQL Database │
│  (Port 80/443)  │───▶│  (Port 8080)    │───▶│  (Port 3306)    │
│                 │    │                 │    │                 │
│ • Rate Limiting │    │ • JWT Auth      │    │ • EF Core       │
│ • Security      │    │ • Swagger API   │    │ • Migrations    │
│ • Error Pages   │    │ • Health Check  │    │ • Health Check  │
│ • Compression   │    │ • Logging       │    │ • Optimized     │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### **Container Specifications**

#### **MySQL Container (Containerfile.mysql)**
- **Base**: mysql:8.0-oracle
- **Performance**: 200 max connections, 256M buffer pool, optimized InnoDB settings
- **Character Set**: UTF8MB4 with Unicode collation
- **Security**: Remote access configured, monitoring user created
- **Health Check**: mysqladmin ping validation
- **Initialization**: Automatic user creation and privilege setup

#### **ASP.NET Core Container (Containerfile.dotnet)**
- **Base**: mcr.microsoft.com/dotnet/aspnet:8.0-alpine
- **Build**: Multi-stage optimized build process
- **Runtime**: Globalization enabled, proper timezone handling
- **Health Check**: Built-in health endpoint monitoring
- **Logging**: Dedicated logs directory with proper permissions

#### **Nginx Container (Containerfile.nginx)**
- **Base**: nginx:1.25-alpine
- **Features**: Reverse proxy, rate limiting, security headers
- **Performance**: Gzip compression, caching, optimized workers
- **Security**: XSS protection, content-type sniffing prevention
- **Error Handling**: Custom 404/502 pages
- **Health Check**: Dedicated health endpoint

## Project Structure

```
EternaInvites.Back/
├── Database/           # Entity Framework context, migrations, table definitions
├── Domain/            # Business logic, exceptions, helpers
├── Repository/        # Data access layer using Dapper + EF Core
├── Service/           # Business service layer
├── Server/            # ASP.NET Core API controllers and configuration
│   ├── .env          # Environment variables
│   ├── Controllers/  # API endpoints
│   ├── Extensions/   # Service configuration
│   └── Middlewares/  # Exception handling
├── nginx/             # Nginx configuration files
│   ├── nginx.conf    # Main Nginx configuration
│   └── default.conf  # Server block configuration
├── Containerfile.*   # Docker container definitions
├── Compose.yml       # Docker Compose orchestration
└── memory-bank/      # Project documentation and context
```

## Database Schema

### Core Tables:

- **Tab_Usuarios**: Admin users with JWT authentication
- **Tab_Clientes**: Client/event information with unique URLs
- **Tab_Plantillas**: HTML templates with component definitions

### Key Relationships:

- Clients reference Templates via `UuiPlantilla` (UUID foreign key)
- All tables inherit from `_BaseTable` (Id, Activo, FechaCreacion, FechaModificacion)

### Performance Optimizations:

- Unique constraints on client URLs for fast lookup
- Proper indexing on foreign keys
- Soft delete pattern with `Activo` flag
- UTF8MB4 character set for full Unicode support

## Security Implementation

### **Multi-Layer Security**

#### **Nginx Layer**
- **Rate Limiting**: 10 req/s for API, 5 req/min for login
- **Security Headers**: XSS protection, content-type sniffing prevention
- **Request Filtering**: Block hidden files and backup files
- **Custom Error Pages**: No information disclosure

#### **Application Layer**
- **JWT Authentication**: Bearer token validation (ready, currently disabled)
- **CORS Configuration**: Controlled cross-origin access
- **Global Exception Handling**: Consistent error responses
- **Connection Security**: Password masking in logs

#### **Database Layer**
- **Parameterized Queries**: Protection against SQL injection
- **User Privileges**: Limited database user permissions
- **Connection Encryption**: SSL mode configurable

## Development Setup

### Prerequisites:

- .NET 8.0 SDK or higher
- Docker Desktop with Compose
- Visual Studio Code or Visual Studio

### Quick Start:

```bash
# Start all services
docker-compose up -d --build

# View service logs
docker-compose logs -f

# Check health status
curl http://localhost/health
```

### Configuration:

#### **Environment Variables (.env)**
```env
# Database Configuration
MYSQL_SERVER=localhost
MYSQL_DATABASE=eternainvites
MYSQL_USER=eternauser
MYSQL_PASSWORD=eterna123

# Connection Strings (using variables)
CONNECTIONSTRING_MYSQL=Server=${MYSQL_SERVER};Port=${MYSQL_PORT};Database=${MYSQL_DATABASE};...

# JWT Configuration (ready for activation)
JWT_KEY=your-secret-key
JWT_ISSUER=your-issuer
JWT_AUDIENCE=your-audience
```

#### **Docker Compose Services**
- **mysql**: Database service with health checks
- **api**: ASP.NET Core API with dependency on MySQL
- **nginx**: Reverse proxy with dependency on API

## Performance Considerations

### **Nginx Optimizations**
- **Gzip Compression**: Reduces bandwidth usage
- **Connection Keepalive**: Reduces connection overhead
- **Worker Processes**: Auto-scaled based on CPU cores
- **Buffer Optimization**: Proper proxy buffering settings

### **Database Optimizations**
- **Connection Pooling**: EF Core connection management
- **Query Optimization**: Proper indexing and relationships
- **Buffer Pool**: 256MB InnoDB buffer pool for caching
- **Slow Query Logging**: Performance monitoring enabled

### **Application Optimizations**
- **Structured Logging**: Efficient log processing with Serilog
- **Health Checks**: Lightweight monitoring endpoints
- **Lazy Loading**: EF Core lazy loading for related entities
- **Caching**: Proxy-level caching for public invitations

## Monitoring & Observability

### **Health Checks**
- **Database**: MySQL connectivity and query performance
- **Application**: ASP.NET Core health endpoint
- **Proxy**: Nginx health endpoint

### **Logging**
- **Structured Logs**: JSON format with Serilog
- **Multiple Sinks**: Console for development, MySQL for persistence
- **Log Levels**: Configurable verbosity for different environments
- **Exception Details**: Comprehensive error information

### **Metrics (Ready for Implementation)**
- **Request Metrics**: Response times, error rates
- **Database Metrics**: Connection counts, query performance
- **System Metrics**: CPU, memory, disk usage

## Deployment Strategies

### **Development**
- **Hot Reload**: File watching with dotnet watch
- **Direct Access**: API accessible on ports 5000/5001
- **Debug Logging**: Verbose logging enabled

### **Production**
- **Reverse Proxy**: All traffic through Nginx on port 80
- **Health Monitoring**: Automated health checks
- **Security Headers**: Full security configuration
- **Performance Optimized**: Compression and caching enabled

### **Scaling Options**
- **Horizontal Scaling**: Multiple API containers behind Nginx
- **Database Scaling**: Read replicas for MySQL
- **Load Balancing**: Nginx upstream configuration ready
- **Container Orchestration**: Ready for Kubernetes deployment

This technical foundation provides a robust, secure, and scalable platform for the EternaInvites application with enterprise-grade infrastructure capabilities.

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
