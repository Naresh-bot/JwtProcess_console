using System.IdentityModel.Tokens.Jwt;  
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTProcessConsole
{
    class TokenParser:ITokenParser
    {
        public readonly IConsoleWriter _consoleWriter;
        public readonly IConfiguration _configuration;
        public TokenParser(IConsoleWriter consoleWriter, IConfiguration configuration)
        {
            _consoleWriter = consoleWriter;
            _configuration = configuration;
        }
        public void ParseToken(string jwtToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var validationParams = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]))

                };

                SecurityToken validatedToken;
                var principal = handler.ValidateToken(jwtToken, validationParams, out validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    Console.WriteLine("Token Header:");
                    foreach (var header in jwtSecurityToken.Header)
                    {
                        Console.WriteLine($"{header.Key}: {header.Value}");
                    }

                    Console.WriteLine("\nToken Payload (Claims):");
                    foreach (var claim in jwtSecurityToken.Claims)
                    {
                        Console.WriteLine($"{claim.Type}: {claim.Value}");
                    }

                    Console.WriteLine($"\nToken Expiration: {jwtSecurityToken.ValidTo}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decoding JWT: {ex.Message}");
            }
        }
    }
}