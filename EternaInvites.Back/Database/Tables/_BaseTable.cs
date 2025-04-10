using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Tables;

public class _BaseTable
{
    /// <summary>
    /// Indica si esta tabla esta activa o no
    /// </summary>
    [Column("Activo", TypeName = "tinyint(1)"), DefaultValue(true)]
    public bool? Activo { get; set; } = true;

    /// <summary>
    /// Fecha de creaci�n de la tabla
    /// </summary>
    [
        Column("FechaCreacion", TypeName = "datetime(6)"),
        DatabaseGenerated(DatabaseGeneratedOption.Identity)
    ]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha de modificaci�n de la tabla
    /// </summary>
    [
        Column("FechaModificacion", TypeName = "datetime(6)"),
        DatabaseGenerated(DatabaseGeneratedOption.Computed)
    ]
    public DateTime FechaModificacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Identificador unico de la tabla
    /// </summary>
    [
        Column("Id", TypeName = "bigint(20) unsigned"),
        Key,
        DatabaseGenerated(DatabaseGeneratedOption.Identity)
    ]
    public ulong Id { get; set; }

    [Column("Uui", TypeName = "char(36)")]
    public Guid Uui { get; set; } = Guid.NewGuid();
}
