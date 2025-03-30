using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Server.Extensions;
using Serilog.Exceptions;
using Server.Middleware;
var builder = WebApplication.CreateBuilder(args);
var todos = "Todos";
var connection = builder.Configuration.GetConnectionString("MySql");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddContext(connection!);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddCors(options =>
{
    options.AddPolicy(todos, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
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


var app = builder.Build();
app.UseExceptionHandlingMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
