using Database.Tables;
using System.Data;
using System.Threading.Tasks;
using Domain.Exceptions;
using System.Net;

namespace Repository;

public interface ITab_UsuariosRepository
{
    /// <summary>
    /// Obtiene un usuario por su nombre de usuario
    /// </summary>
    /// <param name="usuario">Nombre de usuario</param>
    /// <param name="activo">Indica si se debe filtrar por usuarios activos</param>
    /// <returns>Usuario encontrado o null si no existe</returns>
    Task<Tab_Usuarios?> GetByUsuarioAsync(string usuario, bool? activo = true);
}

public class Tab_UsuariosRepository : _BaseRepository<Tab_Usuarios>, ITab_UsuariosRepository
{
    public Tab_UsuariosRepository(IDbConnection dbConnection) : base(dbConnection)
    {
    }

    /// <summary>
    /// Obtiene un usuario por su nombre de usuario
    /// </summary>
    /// <param name="usuario">Nombre de usuario</param>
    /// <param name="activo">Indica si se debe filtrar por usuarios activos</param>
    /// <returns>Usuario encontrado o null si no existe</returns>
    public async Task<Tab_Usuarios?> GetByUsuarioAsync(string usuario, bool? activo = true)
    {
        if (string.IsNullOrWhiteSpace(usuario))
            throw new ArgumentNullException(nameof(usuario), "El nombre de usuario no puede estar vacío");

        var criteria = new
        {
            usuario
        };

        return await GetFirstByColumnsAsync(criteria, activo);
    }
}