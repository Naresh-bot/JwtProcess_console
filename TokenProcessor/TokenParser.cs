using System.IdentityModel.Tokens.Jwt;
using System.Text;
using JWTProcessConsole.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTProcessConsole
{
    class TokenParser : ITokenParser
    {
        public readonly IConfiguration _configuration;
        public TokenParser(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenWrapper ParseToken(string jwtToken)
        {
            TokenWrapper tokenWrapper = new TokenWrapper();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(jwtToken);
                tokenWrapper.JwtToken = jwtSecurityToken;

            }
            catch (Exception ex)
            {
                tokenWrapper.ErrorMessage = ex.Message; 
            }
            return tokenWrapper;
        }
    }
}