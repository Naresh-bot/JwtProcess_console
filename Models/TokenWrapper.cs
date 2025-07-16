using System.IdentityModel.Tokens.Jwt;

namespace JWTProcessConsole.Models;

public class TokenWrapper
{
    public JwtSecurityToken? JwtToken { get; set; }
    public string? ErrorMessage { get; set; } = string.Empty;
}