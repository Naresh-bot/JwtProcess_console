using JWTProcessConsole.Models;

namespace JWTProcessConsole
{
    interface ITokenValidator
    {
        public TokenWrapper ValidateToken(string token,string customKey);
    }
}           