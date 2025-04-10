using Dapp = Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Tables;

[Table("Tab_Clientes")]
[Dapp.Table("Tab_Clientes")]
public class Tab_Clientes : _BaseTable
{
    #region properties
    public string Apellido { get; set; } = string.Empty;
    public string? ApellidoPaterno { get; set; } = string.Empty;
    public string? Correo { get; set; } = string.Empty;
    public string? Descripcion { get; set; } = string.Empty;
    public string? Direccion { get; set; } = string.Empty;
    public DateTime FechaEvento { get; set; } = DateTime.Now;
    public string Nombre { get; set; } = string.Empty;
    public string? NombreEvento { get; set; } = string.Empty;

    [NotMapped]
    [Dapp.Write(false)]
    public Tab_Plantillas? Plantilla { get; set; }

    public string? Telefono { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public Guid? UuiPlantilla { get; set; }
    #endregion 
}

public class Tab_ClientesConfiguration : IEntityTypeConfiguration<Tab_Clientes>
{
    public void Configure(EntityTypeBuilder<Tab_Clientes> builder)
    {
        builder.Property(e => e.Activo).HasDefaultValue(true);
        builder.HasIndex(e => e.Url).IsUnique();
    }
}