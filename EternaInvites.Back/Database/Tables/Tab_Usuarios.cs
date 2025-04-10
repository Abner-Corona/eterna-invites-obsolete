using CryptSharp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using Dapp = Dapper.Contrib.Extensions;

namespace Database.Tables;

[Table("Tab_Usuarios")]
[Dapp.Table("Tab_Usuarios")]
public class Tab_Usuarios : _BaseTable
{
    public string Contrasena { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
}

public class Tab_UsuariosConfiguration : IEntityTypeConfiguration<Tab_Usuarios>
{
    public void Configure(EntityTypeBuilder<Tab_Usuarios> builder)
    {
        builder.Property(e => e.Activo).HasDefaultValue(true);
        builder.HasData(
            new Tab_Usuarios
            {
                Id = 1,
                Nombre = "ADMINISTRADOR",
                Usuario = "admin",
                Contrasena = Crypter.Blowfish.Crypt("12345678"),
            }
        );
    }
}
