using Database.Tables;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Database;

public class _Context : DbContext

{
    /// <summary>
    /// Inicializa una nueva instancia de la clase _Context.
    /// </summary>
    /// <param name="options">Las opciones de DbContext que se van a usar.</param>
    public _Context(DbContextOptions<_Context> opciones)
        : base(opciones)
    {
        // Asegura la existencia de la base de datos.
        Database.EnsureCreated();
    }

    /// <summary>
    /// Configura las opciones de DbContext.
    /// </summary>
    /// <param name="optionsBuilder">El creador de opciones para el DbContext.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    /// <summary>
    /// M�todo que se ejecuta cuando se est� modelando el contexto de la base de datos.
    /// La personalizaci�n del modelo se puede hacer aqu� utilizando Fluent API.
    /// </summary>
    /// <param name="modelBuilder">El constructor de modelo para el DbContext.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        Assembly assemblyWithConfigurations = GetType().Assembly; //get whatever assembly you want
        modelBuilder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);
    }
}
// Add-Migration creacion_bd2 -Project Database -Context _Context -OutputDir Migrations -verbose
// dotnet ef migrations add creacion_bd2 -p Database -s Server -c _Context --output-dir Migrations --verbose
// update-database -Context _Context -Project Database -verbose
// dotnet ef database update -p Database -s Server -c _Context --verbose