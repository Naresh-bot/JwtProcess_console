using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTProcessConsole.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTProcessConsole
{
    class TokenValidator : ITokenValidator
    {

        public readonly IConsoleWriter _consoleWriter;
        public readonly IConfiguration _configuration;
        public TokenValidator(IConsoleWriter consoleWriter, IConfiguration configuration)
        {
            _consoleWriter = consoleWriter;
            _configuration = configuration;
        }

        public void ValidateToken(string token, int keytypeId)
        {
            string customKey;

            if (keytypeId == 2)
            {
                customKey = _consoleWriter.GetUserInput(Message.GetSecretKey)!;
            }
            else
            {
                customKey = _configuration["Jwt:SecretKey"]!;
            } 
            
            var handler = new JwtSecurityTokenHandler();

            var validationParams = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(customKey))

            };

            try
            { 
                ClaimsPrincipal principal = handler.ValidateToken(token, validationParams, out SecurityToken validatedToken);
 
                var jwtToken = validatedToken as JwtSecurityToken; 
                _consoleWriter.PrintTokenValidationResult(jwtToken!);
            }
            catch (SecurityTokenExpiredException ex)
            {
                _consoleWriter.PrintMessage(Message.ErrorMessage(ex.Message));
            }
            catch (SecurityTokenException ex)
            {
                _consoleWriter.PrintMessage(Message.ErrorMessage(ex.Message)); 

            }


        }


    }

}