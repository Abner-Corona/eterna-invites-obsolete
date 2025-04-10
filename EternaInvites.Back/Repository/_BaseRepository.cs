using Dapper;
using Dapper.Contrib.Extensions;
using Database.Tables;
using Domain.Exceptions;
using System.Data;
using System.Net;
using System.Reflection;

namespace Repository;

/// <summary>
/// Interfaz base que define operaciones comunes de acceso a datos para diferentes tipos de entidades
/// </summary>
/// <typeparam name="T">Tipo de entidad que deriva de _BaseTable</typeparam>
public interface _IBaseRepository<T> where T : _BaseTable
{
    /// <summary>
    /// Agrega una nueva entidad a la base de datos
    /// </summary>
    /// <param name="item">Entidad a agregar</param>
    /// <returns>Entidad agregada con ID generado</returns>
    Task<T> AddAsync(T item);

    /// <summary>
    /// Elimina suavemente una entidad configurando su estado activo a falso
    /// </summary>
    /// <param name="id">ID de la entidad a eliminar</param>
    /// <returns>Verdadero si la operación fue exitosa, falso en caso contrario</returns>
    Task<bool> DeleteAsync(ulong id);

    /// <summary>
    /// Recupera todas las entidades del tipo T
    /// </summary>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Colección de entidades</returns>
    Task<IEnumerable<T>> GetAllAsync(bool? activo = true);

    /// <summary>
    /// Recupera una entidad por su ID
    /// </summary>
    /// <param name="id">ID de la entidad</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Entidad o null si no se encuentra</returns>
    Task<T?> GetByIdAsync(ulong id, bool? activo = true);

    /// <summary>
    /// Obtiene entidades que coinciden con las propiedades del objeto criterio
    /// </summary>
    /// <param name="criteria">Objeto anónimo con propiedades que representan columnas y valores para filtrar</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Colección de entidades que coinciden con los criterios</returns>
    Task<IEnumerable<T>> GetByColumnsAsync(object criteria, bool? activo = true);

    /// <summary>
    /// Obtiene la primera entidad que coincide con las propiedades del objeto criterio
    /// </summary>
    /// <param name="criteria">Objeto anónimo con propiedades que representan columnas y valores para filtrar</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Primera entidad que coincide con los criterios o null si no se encuentra ninguna</returns>
    Task<T?> GetFirstByColumnsAsync(object criteria, bool? activo = true);

    /// <summary>
    /// Actualiza una entidad existente en la base de datos
    /// </summary>
    /// <param name="item">Entidad a actualizar</param>
    /// <returns>Entidad actualizada</returns>
    Task<T> UpdateAsync(T item);
}
/// <summary>
/// Repositorio base que proporciona operaciones comunes de acceso a datos para diferentes tipos de entidades
/// </summary>
/// <typeparam name="T">Tipo de entidad que deriva de _BaseTable</typeparam>
public class _BaseRepository<T> : _IBaseRepository<T> where T : _BaseTable
{
    protected readonly IDbConnection _conn;
    protected readonly string _tableName;

    public _BaseRepository(IDbConnection dbConnection)
    {
        _conn = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection), "La conexión a la base de datos no puede ser nula");

        // Obtiene el nombre de la tabla desde TableAttribute si está presente, de lo contrario usa el nombre de la clase
        var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
        _tableName = tableAttr?.Name ?? typeof(T).Name;
    }

    #region methods

    /// <summary>
    /// Agrega una nueva entidad a la base de datos
    /// </summary>
    /// <param name="item">Entidad a agregar</param>
    /// <returns>Entidad agregada con ID generado</returns>
    public virtual async Task<T> AddAsync(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        item.FechaCreacion = DateTime.Now;
        item.FechaModificacion = DateTime.Now;

        var id = await _conn.InsertAsync<T>(item);
        item.Id = (ulong)id;
        return item;
    }

    /// <summary>
    /// Elimina suavemente una entidad configurando su estado activo a falso
    /// </summary>
    /// <param name="id">ID de la entidad a eliminar</param>
    /// <returns>Verdadero si la operación fue exitosa, falso en caso contrario</returns>
    public virtual async Task<bool> DeleteAsync(ulong id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return false;

        entity.Activo = false;
        entity.FechaModificacion = DateTime.Now;

        return await _conn.UpdateAsync(entity);
    }

    /// <summary>
    /// Recupera todas las entidades del tipo T
    /// </summary>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Colección de entidades</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync(bool? activo = true)
    {
        IEnumerable<T> list;

        if (activo.HasValue)
        {
            var query = $"SELECT * FROM {_tableName} WHERE activo = @activo";
            list = await _conn.QueryAsync<T>(query, new { activo });
        }
        else
        {
            list = await _conn.GetAllAsync<T>();
        }

        return list;
    }
    /// <summary>
    /// Recupera una entidad por su ID
    /// </summary>
    /// <param name="id">ID de la entidad</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Entidad o null si no se encuentra</returns>
    public virtual async Task<T?> GetByIdAsync(ulong id, bool? activo = true)
    {
        var entity = await _conn.GetAsync<T>(id);
        if (entity == null) return null;
        if (activo != null && entity.Activo != activo) return null;
        return entity;
    }
    /// <summary>
    /// Obtiene entidades que coinciden con las propiedades del objeto criterio
    /// </summary>
    /// <param name="criteria">Objeto anónimo con propiedades que representan columnas y valores para filtrar</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Colección de entidades que coinciden con los criterios</returns>
    public virtual async Task<IEnumerable<T>> GetByColumnsAsync(object criteria, bool? activo = true)
    {
        if (criteria == null)
            return await GetAllAsync(activo);

        var parameters = new DynamicParameters();
        var whereConditions = new List<string>();

        // Extraer propiedades del objeto criteria
        var properties = criteria.GetType().GetProperties();

        if (!properties.Any())
            return await GetAllAsync(activo);

        // Agregar condiciones por cada propiedad
        foreach (var prop in properties)
        {
            var columnName = prop.Name;
            var value = prop.GetValue(criteria);

            if (value != null)
            {
                whereConditions.Add($"{columnName} = @{columnName}");
                parameters.Add($"@{columnName}", value);
            }
            else
            {
                whereConditions.Add($"{columnName} IS NULL");
            }
        }

        // Agregar condición activo si se especifica
        if (activo.HasValue)
        {
            whereConditions.Add("activo = @activo");
            parameters.Add("@activo", activo.Value);
        }

        var whereClause = whereConditions.Any() ? $"WHERE {string.Join(" AND ", whereConditions)}" : string.Empty;
        var query = $"SELECT * FROM {_tableName} {whereClause}";

        return await _conn.QueryAsync<T>(query, parameters);
    }

    /// <summary>
    /// Obtiene la primera entidad que coincide con las propiedades del objeto criterio
    /// </summary>
    /// <param name="criteria">Objeto anónimo con propiedades que representan columnas y valores para filtrar</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Primera entidad que coincide con los criterios o null si no se encuentra ninguna</returns>
    public virtual async Task<T?> GetFirstByColumnsAsync(object criteria, bool? activo = true)
    {
        var results = await GetByColumnsAsync(criteria, activo);
        return results.FirstOrDefault();
    }

    /// <summary>
    /// Actualiza una entidad existente en la base de datos
    /// </summary>
    /// <param name="item">Entidad a actualizar</param>
    /// <returns>Entidad actualizada</returns>
    public virtual async Task<T> UpdateAsync(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        item.FechaModificacion = DateTime.Now;

        var success = await _conn.UpdateAsync(item);
        if (!success)
        {
            throw new ExceptionHelper(
                $"No se pudo actualizar {typeof(T).Name} con ID {item.Id}",
                HttpStatusCode.NotFound
            );
        }

        return item;
    }

    #endregion 
}