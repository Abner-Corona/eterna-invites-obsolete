using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Server.Extensions;
using Serilog.Exceptions;
using Server.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Load .env from project root (only if file exists)
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
    Console.WriteLine($"✅ Loaded .env file from: {envPath}");
}
else
{
    Console.WriteLine("No .env file found, using environment variables from container");
    Console.WriteLine($"   Searched path: {envPath}");
}

var todos = "Todos";
var connection = Environment.GetEnvironmentVariable("CONNECTIONSTRING_MYSQL");

if (string.IsNullOrEmpty(connection))
{
    Console.WriteLine("❌ CONNECTIONSTRING_MYSQL environment variable is not set.");
    return;
}
else
{
    Console.WriteLine($"✅ Using connection string: {connection}");
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddContext(connection!);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddCors(options =>
{
    options.AddPolicy(todos,
    builder =>
    {

        builder.WithOrigins("http://localhost:9000", "https://localhost:9000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Host.UseSerilog((context, configuration) => configuration
.MinimumLevel.Debug()
.Filter.ByExcluding((le) => le.Level == LogEventLevel.Information)
.Filter.ByExcluding((le) => le.Level == LogEventLevel.Warning)

.Enrich.WithExceptionDetails()
.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
// Filter out ASP.NET Core infrastructre logs that are Information and below
.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
.Enrich.FromLogContext()
.WriteTo.Console()
.WriteTo.MySQL(
        connection,
        tableName: "Logs"));



//builder.Services.AddJWTAuth(builder.Configuration["Jwt:Issuer"]!,builder.Configuration["Jwt:Key"]!);
var app = builder.Build();
app.UseExceptionHandlingMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(todos);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
