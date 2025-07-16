using JWTProcessConsole.Models;

namespace JWTProcessConsole
{
    interface ITokenParser
    { 
        public TokenWrapper ParseToken(string token);

    }
    
}

