using Database;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Repository;
using Service;
using System.Data;

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

                    //dbContext.Database.Migrate();
                }
                // MigrarStores(dbContext);
            }
        }
    }

    public static void AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ITab_UsuariosRepository, Tab_UsuariosRepository>();
    }

    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ILoginService, LoginService>();

    }
}
