using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Tables;
using Domain.Exceptions;
using Repository;

namespace Service;

/// <summary>
/// Servicio base que proporciona operaciones comunes de negocio para diferentes tipos de entidades
/// </summary>
/// <typeparam name="T">Tipo de entidad que deriva de _BaseTable</typeparam>
/// <typeparam name="TRepository">Tipo de repositorio que deriva de _BaseRepository</typeparam>
public class _BaseService<T, TRepository>
        where T : _BaseTable
        where TRepository : _BaseRepository<T>
{
    protected readonly TRepository _repository;

    /// <summary>
    /// Constructor del servicio base
    /// </summary>
    /// <param name="repository">Repositorio para acceso a datos</param>
    public _BaseService(TRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository), "El repositorio no puede ser nulo");
    }

    #region methods

    /// <summary>
    /// Crea una nueva entidad
    /// </summary>
    /// <param name="entity">Entidad a crear</param>
    /// <returns>Entidad creada con su ID asignado</returns>
    public virtual async Task<T> CreateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula");

        // Aquí podrías añadir validaciones adicionales antes de crear
        return await _repository.AddAsync(entity);
    }

    /// <summary>
    /// Elimina suavemente una entidad por su ID
    /// </summary>
    /// <param name="id">ID de la entidad a eliminar</param>
    /// <returns>True si se eliminó correctamente, False si no se encontró</returns>
    public virtual async Task<bool> DeleteAsync(ulong id)
    {
        return await _repository.DeleteAsync(id);
    }

    /// <summary>
    /// Busca entidades que coincidan con los criterios especificados
    /// </summary>
    /// <param name="criteria">Objeto con propiedades que representan columnas y valores para filtrar</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Colección de entidades que cumplen los criterios</returns>
    public virtual async Task<IEnumerable<T>> FindByCriteriaAsync(object criteria, bool? activo = true)
    {
        return await _repository.GetByColumnsAsync(criteria, activo);
    }

    /// <summary>
    /// Busca la primera entidad que coincida con los criterios especificados
    /// </summary>
    /// <param name="criteria">Objeto con propiedades que representan columnas y valores para filtrar</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Primera entidad que cumple los criterios o null si no hay coincidencias</returns>
    public virtual async Task<T?> FindFirstByCriteriaAsync(object criteria, bool? activo = true)
    {
        return await _repository.GetFirstByColumnsAsync(criteria, activo);
    }

    /// <summary>
    /// Obtiene todas las entidades
    /// </summary>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Colección de entidades</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync(bool? activo = true)
    {
        return await _repository.GetAllAsync(activo);
    }

    /// <summary>
    /// Obtiene una entidad por su ID
    /// </summary>
    /// <param name="id">ID de la entidad</param>
    /// <param name="activo">Filtrar por estado activo</param>
    /// <returns>Entidad solicitada o null si no existe</returns>
    public virtual async Task<T?> GetByIdAsync(ulong id, bool? activo = true)
    {
        return await _repository.GetByIdAsync(id, activo);
    }

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    /// <param name="entity">Entidad a actualizar</param>
    /// <returns>Entidad actualizada</returns>
    public virtual async Task<T> UpdateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula");

        // Verificar que la entidad existe antes de actualizarla
        var existingEntity = await _repository.GetByIdAsync(entity.Id);
        if (existingEntity == null)
        {
            throw new ExceptionHelper(
                $"No se encontró la entidad {typeof(T).Name} con ID {entity.Id} para actualizar",
                System.Net.HttpStatusCode.NotFound
            );
        }

        // Aquí podrías añadir validaciones adicionales antes de actualizar
        return await _repository.UpdateAsync(entity);
    }

    #endregion 
}