
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

            Console.WriteLine("Is the user Anonymous? (Yes/No)");
            var input = Console.ReadLine();
            string[] affirm = ["yes", "y"];

            bool isAnonymousUser = affirm.Contains(input.ToLower());

            if (keytypeId == 2)
            {
                customKey = _consoleWriter.GetSecretKey();
            }
            else
            {
                customKey = _configuration["Jwt:SecretKey"];
            }

            var userContext = _consoleWriter.GetAnonymousUserDetails(true);


            var handler = new JwtSecurityTokenHandler();

            var validationParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(customKey))

            };


            if (isAnonymousUser)
            {
                validationParams.ValidIssuer = userContext.jwtdetails.Issuer;
                validationParams.ValidAudience = userContext.jwtdetails.Audience;
            }
            else
            {
                validationParams.ValidIssuer = _configuration["Jwt:Issuer"];
                validationParams.ValidAudience = _configuration["Jwt:Audience"];
            }



            try
            {
                // Validate token
                ClaimsPrincipal principal = handler.ValidateToken(token, validationParams, out SecurityToken validatedToken);

                Console.WriteLine("Token is valid.");

                // Cast to JwtSecurityToken to access claims like expiration
                var jwtToken = validatedToken as JwtSecurityToken;
                //var jwtToken = (JwtSecurityToken)validatedToken;

                if (jwtToken != null)
                {
                    DateTime expiry = jwtToken.ValidTo; // UTC time 
                    Console.WriteLine($"Token expires at (Local Time): {expiry.ToLocalTime()}");
                }
            }
            catch (SecurityTokenExpiredException)
            {
                Console.WriteLine("Token has expired.");
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine($"Token is invalid: {ex.Message}");
            }


        }


    }

}