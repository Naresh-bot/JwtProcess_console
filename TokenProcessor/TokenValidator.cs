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
        public readonly IConfiguration _configuration;
        public TokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenWrapper ValidateToken(string token, string customKey)
        { 
            var handler = new JwtSecurityTokenHandler();
            TokenWrapper tokenWrapper = new TokenWrapper();

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
                tokenWrapper.JwtToken = jwtToken;
            }
            catch (SecurityTokenExpiredException ex)
            {
                tokenWrapper.ErrorMessage = ex.Message; 
            }
            catch (SecurityTokenException ex)
            {
                tokenWrapper.ErrorMessage = ex.Message; 
            } 
            return tokenWrapper;

        }


    }

}