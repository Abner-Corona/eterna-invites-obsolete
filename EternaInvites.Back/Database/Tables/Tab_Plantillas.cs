using CryptSharp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Dapp = Dapper.Contrib.Extensions;

namespace Database.Tables;

[Table("Tab_Plantillas")]
[Dapp.Table("Tab_Plantillas")]
public class Tab_Plantillas : _BaseTable
{
    public string? Nombre { get; set; } = string.Empty;

    public bool? MostrarDemo { get; set; } = false;
    public string? Descripcion { get; set; } = string.Empty;

    public string? Html { get; set; } = string.Empty;

    public string? Css { get; set; } = string.Empty;

    public string? Js { get; set; } = string.Empty;


    // JSON column to store template components and styles
    [Column("ComponentesJson", TypeName = "json")]
    public string? ComponentesJson { get; set; } = "[]";


    // Navigation property to access the JSON data in a structured way
    // This property should NOT be mapped to a database column
    [NotMapped]
    [Dapp.Write(false)]
    public List<ComponenteHTML>? Componentes
    {
        get => string.IsNullOrEmpty(ComponentesJson)
            ? new List<ComponenteHTML>()
            : JsonSerializer.Deserialize<List<ComponenteHTML>>(ComponentesJson) ?? new List<ComponenteHTML>();
        set => ComponentesJson = JsonSerializer.Serialize(value);
    }

    // Category or type of template
    public string Categoria { get; set; } = string.Empty;
}

// Class to define the structure of template components
public class ComponenteHTML
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // button, div, form, etc.
    public string Contenido { get; set; } = string.Empty;
    public Dictionary<string, string> Propiedades { get; set; } = new Dictionary<string, string>();
    public List<string> Clases { get; set; } = new List<string>();
    public List<EstiloCSS> Estilos { get; set; } = new List<EstiloCSS>();

    public List<ComponenteHTML> Hijos { get; set; } = new List<ComponenteHTML>();
    public Dictionary<string, string> Eventos { get; set; } = new Dictionary<string, string>(); // onclick, onhover, etc.
}

public class EstiloCSS
{
    public string Selector { get; set; } = string.Empty;
}

public class ConfiguracionGeneral
{
}

public class Tab_PlantillasConfiguration : IEntityTypeConfiguration<Tab_Plantillas>
{
    public void Configure(EntityTypeBuilder<Tab_Plantillas> builder)
    {
        builder.Property(e => e.Activo).HasDefaultValue(true);

        // Configure ComponentesJson as JSON column
        builder.Property(e => e.ComponentesJson).HasColumnType("json");

        // Explicitly ignore the ComponentesData property
        builder.Ignore(e => e.Componentes);
    }
}