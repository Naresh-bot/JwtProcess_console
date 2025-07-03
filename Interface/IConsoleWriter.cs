using System.IdentityModel.Tokens.Jwt;

namespace JWTProcessConsole
{
    public interface IConsoleWriter
    { 
        public void StopApp();  
        public Users GetUserCredentials();
        public void ClearConsole(); 
        public void TokenHandlingOptions();
        public void UserInfoOptions();
        public void SecurityKeyOptions();
        public string? GetUserInput(string message); 
        public Users RegisterUser(); 
        public UserContext GetAnonymousUserDetails(); 
        public void PrintParsedToken(JwtSecurityToken token);
        public void PrintTokenValidationResult(JwtSecurityToken jwtToken);
        public void PrintMessage(string message);
        public void PrintMessageInLine(string message);

    }

}
