using System.Net;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly ILogger<LoginController> _logger;


    public LoginController(ILoginService loginService, ILogger<LoginController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {

        var response = await _loginService.Login(req);
        return Ok(response);

    }
}

