using Domain.Exceptions;
using Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Repository;
using System.Net;
using System.Security.Claims;

namespace Service;

public interface ILoginService
{
    Task<LoginResponse> Login(LoginRequest req);
}

public class LoginRequest
{
    public string Contrasena { get; set; }
    public string Usuario { get; set; }
}

public class LoginResponse
{
    public ulong Id { get; set; }
    public string Token { get; set; }
    public string Uuid { get; set; }
}

public class LoginService : ILoginService
{
    private readonly IConfiguration _config;
    private readonly ITab_UsuariosRepository _tab_UsuariosRepository;

    public LoginService(ITab_UsuariosRepository tab_UsuariosRepository, IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config), "La configuración no puede ser nula");
        _tab_UsuariosRepository = tab_UsuariosRepository ?? throw new ArgumentNullException(nameof(tab_UsuariosRepository), "El repositorio no puede ser nulo");
    }

    public async Task<LoginResponse> Login(LoginRequest req)
    {
        var user = await _tab_UsuariosRepository.GetByUsuarioAsync(req.Usuario!);
        if (user == null)
        {
            throw new ExceptionHelper("Usuario no encontrado", HttpStatusCode.NotFound);
        }
        if (!CryptSharp.Core.Crypter.CheckPassword(req.Contrasena!, user.Contrasena!))
        {
            throw new ExceptionHelper("Contraseña incorrecta", HttpStatusCode.NotFound);
        }
        var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString() ),
                new Claim("uuid", user.Uui.ToString()!),
            };
        return new LoginResponse
        {
            Id = user.Id,
            Uuid = user.Uui.ToString()!,
            Token = PasswordHelper.GenerateToken(claims, _config["Jwt:Key"]!, _config["Jwt:Issuer"]!, 0),
        };
    }
}