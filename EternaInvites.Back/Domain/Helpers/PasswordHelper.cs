using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Helpers;

public static class PasswordHelper
{
    public static void CreatePasswordHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt
    )
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public static string GenerateToken(
        IEnumerable<Claim> claims,
        string secret,
        string issuer,
        int expire
    )
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256Signature
        );
        var tokenDescriptor = new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.Now.AddDays(expire),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public static bool VerifyPasswordHash(
        string password,
        byte[] passwordHashAlmacenado,
        byte[] passwordSalt
    )
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var passwordHashNuevo = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return new ReadOnlySpan<byte>(passwordHashAlmacenado).SequenceEqual(
            new ReadOnlySpan<byte>(passwordHashNuevo)
        );
    }
}
