using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTProcessConsole.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTProcessConsole
{
    class TokenGenerator : ITokenGenerator
    {
        public readonly IConfiguration _configuration;
        public readonly IConsoleWriter _consoleWriter;
        public TokenGenerator(IConfiguration configuration, IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
            _configuration = configuration;
        }

        public void GenerateToken(UserContext userContext)
        {
            string keyString = string.Empty;
            List<Claim> claims = new();
            var token = new JwtSecurityToken();

            if (userContext.jwtdetails != null)
            {
                keyString = userContext.jwtdetails.SecurityKey;

                var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(userContext.jwtdetails.SecurityKey));
                var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);


                var jwtDetails = userContext.jwtdetails;

                foreach (var dict in userContext.jwtdetails.claims)
                {
                    claims.Add(new Claim(dict.Key, dict.Value));
                }

                token = new JwtSecurityToken(jwtDetails.Issuer,
                jwtDetails.Audience,
                claims,
                expires: DateTime.Now.AddMinutes((double)jwtDetails.ExpiryInMinutes),
                signingCredentials: credentials
                );
            }
            else
            {
                keyString = _configuration["Jwt:SecretKey"]!;

                var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
                var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                claims.Add(new Claim(JwtRegisteredClaimNames.Name, userContext.users.Username));
                claims.Add(new Claim("Role", userContext.users.Role));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));



                token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:Expiry"])),
                        signingCredentials: credentials
                        );

            }

            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token).ToString();

            _consoleWriter.PrintMessage(Message.GeneratedToken(generatedToken));
        }
    }
}