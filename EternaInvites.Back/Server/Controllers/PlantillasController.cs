using System.Net;
using Database.Tables;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlantillasController : _BaseController<PlantillasModel, Tab_Plantillas>
{


    public PlantillasController(IPlantillasService service) : base(service)
    {
    }



}

