using System.Net;
using Database.Tables;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ClientesController : _BaseController<ClientesModel, Tab_Clientes>
{
    private readonly IClientesService _serviceCliente;

    public ClientesController(IClientesService service) : base(service)
    {
        _serviceCliente = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet("[action]/{url}")]
    [AllowAnonymous]
    public async Task<ActionResult<ClientesModel>> ObtenerInivitacion(string url)
    {
        var item = await _serviceCliente.ObtenerInivitacion(url);
        if (item == null)
            return NotFound();

        return Ok(item);
    }


}

