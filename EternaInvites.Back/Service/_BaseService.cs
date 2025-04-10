using Database.Tables;
using Domain.Exceptions;
using Mapster;
using Repository;

namespace Service;

/// <summary>
/// Interfaz base que define operaciones comunes de servicio para diferentes tipos de entidades
/// </summary>
/// <typeparam name="TModel">Tipo de modelo de negocio</typeparam>
/// <typeparam name="TTable">Tipo de entidad que deriva de _BaseTable</typeparam>
public interface _IBaseService<TModel, TTable>
    where TModel : class
    where TTable : _BaseTable
{
    /// <summary>
    /// Crea una nueva entidad
    /// </summary>
    /// <param name="request">Modelo para crear la entidad</param>
    /// <returns>Modelo creado con su ID asignado</returns>
    Task<TModel> CreateAsync(TModel request);
    /// <summary>
    /// Elimina suavemente una entidad por su ID
    /// </summary>
    /// <param name="id">ID de la entidad a eliminar</param>
    /// <returns>True si se eliminó correctamente, False si no se encontró</returns>
    Task<bool> DeleteAsync(ulong id);
    /// <summary>
    /// Busca entidades que coincidan con los criterios especificados
    /// </summary>
    /// <param name="criteria">Objeto con propiedades que representan columnas y valores para filtrar</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Colección de entidades que cumplen los criterios</returns>
    Task<IEnumerable<TModel>> FindByCriteriaAsync(object criteria, bool? activo = true);
    /// <summary>
    /// Busca la primera entidad que coincida con los criterios especificados
    /// </summary>
    /// <param name="criteria">Objeto con propiedades que representan columnas y valores para filtrar</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Primera entidad que cumple los criterios o null si no hay coincidencias</returns>
    Task<TModel?> FindFirstByCriteriaAsync(object criteria, bool? activo = true);
    /// <summary>
    /// Obtiene todas las entidades
    /// </summary>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Colección de entidades</returns>
    Task<IEnumerable<TModel>> GetAllAsync(bool? activo = true);
    /// <summary>
    /// Obtiene una entidad por su ID
    /// </summary>
    /// <param name="id">ID de la entidad</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Entidad solicitada o null si no existe</returns>
    Task<TModel?> GetByIdAsync(ulong id, bool? activo = true);
    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    /// <param name="req">Modelo con los datos actualizados</param>
    /// <returns>Modelo actualizado</returns>
    Task<TModel> UpdateAsync(TModel req);
}

/// <summary>
/// Servicio base que proporciona operaciones comunes de negocio para diferentes tipos de entidades
/// </summary>
/// <typeparam name="T">Tipo de entidad que deriva de _BaseTable</typeparam>
public class _BaseService<TModel, TTable> : _IBaseService<TModel, TTable>
    where TModel : class
    where TTable : _BaseTable
{
    protected readonly _IBaseRepository<TTable> _repositoryBase;

    /// <summary>
    /// Constructor del servicio base
    /// </summary>
    /// <param name="repository">Repositorio para acceso a datos</param>
    public _BaseService(_IBaseRepository<TTable> repository)
    {
        _repositoryBase = repository ?? throw new ArgumentNullException(nameof(repository), "El repositorio no puede ser nulo");
    }

    #region methods

    /// <summary>
    /// Crea una nueva entidad
    /// </summary>
    /// <param name="entity">Entidad a crear</param>
    /// <returns>Entidad creada con su ID asignado</returns>
    public virtual async Task<TModel> CreateAsync(TModel request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request), "La entidad no puede ser nula");
        var entity = request.Adapt<TTable>();
        var res = await _repositoryBase.AddAsync(entity!);

        // Aquí podrías añadir validaciones adicionales antes de crear
        return res.Adapt<TModel>();
    }

    /// <summary>
    /// Elimina suavemente una entidad por su ID
    /// </summary>
    /// <param name="id">ID de la entidad a eliminar</param>
    /// <returns>True si se eliminó correctamente, False si no se encontró</returns>
    public virtual async Task<bool> DeleteAsync(ulong id)
    {
        return await _repositoryBase.DeleteAsync(id);
    }

    /// <summary>
    /// Busca entidades que coincidan con los criterios especificados
    /// </summary>
    /// <param name="criteria">Objeto con propiedades que representan columnas y valores para filtrar</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Colección de entidades que cumplen los criterios</returns>
    public virtual async Task<IEnumerable<TModel>> FindByCriteriaAsync(object criteria, bool? activo = true)
    {
        var res = await _repositoryBase.GetByColumnsAsync(criteria, activo);
        return res?.Adapt<IEnumerable<TModel>>() ?? new List<TModel>();
    }

    /// <summary>
    /// Busca la primera entidad que coincida con los criterios especificados
    /// </summary>
    /// <param name="criteria">Objeto con propiedades que representan columnas y valores para filtrar</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Primera entidad que cumple los criterios o null si no hay coincidencias</returns>
    public virtual async Task<TModel?> FindFirstByCriteriaAsync(object criteria, bool? activo = true)
    {
        var res = await _repositoryBase.GetFirstByColumnsAsync(criteria, activo);
        return res?.Adapt<TModel>();
    }

    /// <summary>
    /// Obtiene todas las entidades
    /// </summary>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Colección de entidades</returns>
    public virtual async Task<IEnumerable<TModel>> GetAllAsync(bool? activo = true)
    {
        var res = await _repositoryBase.GetAllAsync(activo);
        return res?.Adapt<IEnumerable<TModel>>() ?? new List<TModel>();
    }

    /// <summary>
    /// Obtiene una entidad por su ID
    /// </summary>
    /// <param name="id">ID de la entidad</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Entidad solicitada o null si no existe</returns>
    public virtual async Task<TModel?> GetByIdAsync(ulong id, bool? activo = true)
    {
        var res = await _repositoryBase.GetByIdAsync(id, activo);
        return res?.Adapt<TModel>();
    }

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    /// <param name="entity">Entidad a actualizar</param>
    /// <returns>Entidad actualizada</returns>
    public virtual async Task<TModel> UpdateAsync(TModel req)
    {
        if (req == null) throw new ArgumentNullException(nameof(req), "La entidad no puede ser nula");
        var entity = req.Adapt<TTable>();
        // Verificar que la entidad existe antes de actualizarla
        var existingEntity = await _repositoryBase.GetByIdAsync(entity.Id);
        if (existingEntity == null)
        {
            throw new ExceptionHelper(
                $"No se encontró la entidad {typeof(TTable).Name} con ID {entity.Id} para actualizar",
                System.Net.HttpStatusCode.NotFound
            );
        }
        var res = await _repositoryBase.UpdateAsync(entity!);
        // Aquí podrías añadir validaciones adicionales antes de actualizar
        return res.Adapt<TModel>();
    }

    #endregion 
}