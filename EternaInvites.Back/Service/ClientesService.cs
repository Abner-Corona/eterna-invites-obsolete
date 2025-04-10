using System.Net;
using Database.Tables;
using Domain.Exceptions;
using Repository;

namespace Service;

public interface IClientesService : _IBaseService<ClientesModel, Tab_Clientes>
{
    Task<ObtenerInivitacionResponse> ObtenerInivitacion(string url);
}

public class ClientesModel
{
    #region properties
    public string Apellido { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string? Correo { get; set; } = string.Empty;
    public string? Descripcion { get; set; } = string.Empty;
    public string? Direccion { get; set; } = string.Empty;
    public DateTime FechaEvento { get; set; } = DateTime.Now;
    public int? Id { get; set; } = null;
    public string Nombre { get; set; } = string.Empty;
    public string? NombreEvento { get; set; } = string.Empty;
    public string? Telefono { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public Guid? Uui { get; set; } = Guid.NewGuid();
    #endregion 
}

public class ClientesService : _BaseService<ClientesModel, Tab_Clientes>, IClientesService
{

    private readonly ITab_ClientesRepository _repositoryClientes;
    private readonly ITab_PlantillasRepository _repositoryPlantillas;

    public ClientesService(ITab_ClientesRepository repository, ITab_PlantillasRepository repositoryPlantillas) : base(repository)
    {
        _repositoryClientes = repository ?? throw new ArgumentNullException(nameof(repository));
        _repositoryPlantillas = repositoryPlantillas ?? throw new ArgumentNullException(nameof(repositoryPlantillas));
    }

    // Implement service methods here

    public async Task<ObtenerInivitacionResponse> ObtenerInivitacion(string url)
    {
        var cliente = await _repositoryClientes.GetFirstByColumnsAsync(new { url });
        if (cliente == null)
            throw new ExceptionHelper($"Cliente with URL {url} not found.", HttpStatusCode.NotFound);

        var plantilla = await _repositoryPlantillas.GetFirstByColumnsAsync(new { Uui = cliente.UuiPlantilla });
        return new ObtenerInivitacionResponse
        {
            Html = plantilla?.Html ?? string.Empty,
        };

    }
}

public class ObtenerInivitacionResponse
{
    public string Html { get; set; } = string.Empty;
}