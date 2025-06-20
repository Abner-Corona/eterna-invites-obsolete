using Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Repository;
using Service;
using System.Data;
using System.Text;

namespace Server.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddContext(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddTransient<IDbConnection>(db => new MySqlConnection(connectionString));
        serviceCollection.AddDbContext<_Context>(options =>
        {

            options.UseLazyLoadingProxies().UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                b => b.UseMicrosoftJson(MySqlCommonJsonChangeTrackingOptions.FullHierarchyOptimizedSemantically)
            );
        });
        using (var scope = serviceCollection.BuildServiceProvider().CreateScope())
        {
            using (var dbContext = scope.ServiceProvider.GetRequiredService<_Context>())
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                {

                    dbContext.Database.MigrateAsync();
                }
                // MigrarStores(dbContext);
            }
        }
    }

    public static void AddJWTAuth(this IServiceCollection serviceCollection, string issuer, string key)

    {
        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!))
        };
    });
        serviceCollection.AddAuthorization(options =>
        {
            options.FallbackPolicy = options.DefaultPolicy;
        });
    }

    public static void AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ITab_PlantillasRepository, Tab_PlantillasRepository>();
        serviceCollection.AddTransient<ITab_ClientesRepository, Tab_ClientesRepository>();
        serviceCollection.AddTransient<ITab_UsuariosRepository, Tab_UsuariosRepository>();
    }

    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ILoginService, LoginService>();
        serviceCollection.AddTransient<IClientesService, ClientesService>();
        serviceCollection.AddTransient<IPlantillasService, PlantillasService>();

    }
}