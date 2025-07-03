using System.IdentityModel.Tokens.Jwt;
using System.Text;
using JWTProcessConsole.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTProcessConsole
{
    class TokenParser : ITokenParser
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
                var jwtSecurityToken = handler.ReadJwtToken(jwtToken); 
                _consoleWriter.PrintParsedToken(jwtSecurityToken);
               
            }
            catch (Exception ex)
            {
                _consoleWriter.PrintMessage(Message.ErrorMessage(ex.Message));
            }
        }
    }
}