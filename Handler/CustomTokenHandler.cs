
using JWTProcessConsole.Models;
using Microsoft.Extensions.Configuration;

namespace JWTProcessConsole
{
    class CustomTokenHandler : ICustomTokenHandler
    {
        public readonly ITokenGenerator _tokenGenerator;
        public readonly ITokenValidator _tokenValidator;
        public readonly ITokenParser _tokenParser;
        public readonly IConsoleWriter _consoleWriter;
        public readonly IConfiguration _configuration;

        public CustomTokenHandler(IConsoleWriter consoleWriter,
            ITokenGenerator tokenGenerator, ITokenValidator tokenValidator,
            ITokenParser tokenParser, IConfiguration configuration)
        {
            _consoleWriter = consoleWriter;
            _tokenGenerator = tokenGenerator;
            _tokenValidator = tokenValidator;
            _tokenParser = tokenParser;
            _configuration = configuration;
        }

        public void GenerateToken(UserContext userContext)
        {
            string generatedToken = _tokenGenerator.GenerateToken(userContext);
            _consoleWriter.PrintMessage(Message.GeneratedToken(generatedToken));
        }
        public void ParseToken(string token)
        {
            var TokenWrapper = _tokenParser.ParseToken(token);
            if (TokenWrapper.ErrorMessage != null)
            {
                _consoleWriter.PrintMessage(Message.ErrorMessage(TokenWrapper.ErrorMessage));
            }
            else
            {
                _consoleWriter.PrintParsedToken(TokenWrapper.JwtToken!);
            } 
        }
        public void ValidateToken(string token, int keytypeId)
        {
            string customKey;
            TokenWrapper tokenWrapper = new();
            if (keytypeId == 2)
            {
                customKey = _consoleWriter.GetUserInput(Message.GetSecretKey)!;
            }
            else
            {
                customKey = _configuration["Jwt:SecretKey"]!;
            }

            tokenWrapper = _tokenValidator.ValidateToken(token, customKey);
            
            if (tokenWrapper.ErrorMessage != string.Empty)
            {
                _consoleWriter.PrintMessage(Message.ErrorMessage(tokenWrapper.ErrorMessage!));
            }
            else
            {
                _consoleWriter.PrintTokenValidationResult(tokenWrapper.JwtToken!); 
            }
        }

        
    }

}