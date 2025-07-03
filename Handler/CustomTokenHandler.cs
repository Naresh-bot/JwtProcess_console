
namespace JWTProcessConsole
{
    class CustomTokenHandler : ICustomTokenHandler
    {
        public readonly ITokenGenerator _tokenGenerator;
        public readonly ITokenValidator _tokenValidator;
        public readonly ITokenParser _tokenParser;
        public readonly IConsoleWriter _consoleWriter;

        public CustomTokenHandler(IConsoleWriter consoleWriter,
            ITokenGenerator tokenGenerator, ITokenValidator tokenValidator,
            ITokenParser tokenParser)
        {
            _consoleWriter = consoleWriter;
            _tokenGenerator = tokenGenerator;
            _tokenValidator = tokenValidator;
            _tokenParser = tokenParser;
        }

        public void GenerateToken(UserContext userContext)
        {
            _tokenGenerator.GenerateToken(userContext);
        } 
        public void ParseToken(string token)
        {
            _tokenParser.ParseToken(token);
        }
        public void ValidateToken(string token, int keytypeId)
        {
            _tokenValidator.ValidateToken(token, keytypeId);
        }

        
    }

}