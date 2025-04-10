using System.Net;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;


    public LoginController(ILoginService loginService)
    {
        _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
    }
    [HttpPost()]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {

        var response = await _loginService.Login(req);
        return Ok(response);

    }


}

