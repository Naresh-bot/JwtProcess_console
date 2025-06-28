

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTProcessConsole
{
    class TokenGenerator : ITokenGenerator
    {
        public readonly IConfiguration Configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void GenerateToken(Users user, string secretkey)
        {
            string keyString = string.Empty;
            if (secretkey != null)
                keyString = secretkey;
            else
                keyString = Configuration["Jwt:SecretKey"];

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Name,user.Username),
                new Claim("Role",user.Role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
            Configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(Configuration["Jwt:Expiry"])),
            signingCredentials: credentials
            );

            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token).ToString();

            System.Console.WriteLine("your generated token is in next line\n");
            System.Console.WriteLine(generatedToken);
            System.Console.WriteLine("\n");
        }

        public void GenerateAnonymousUserToken(UserContext userContext)
        {  
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(userContext.jwtdetails.SecurityKey));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            
            List<Claim> claims = new();
            var jwtDetails = userContext.jwtdetails;

            foreach (var dict in userContext.jwtdetails.claims)
            {
                claims.Add(new Claim(dict.Key, dict.Value));
            } 

            var token = new JwtSecurityToken(jwtDetails.Issuer,
            jwtDetails.Audience,
            claims,
            expires: DateTime.Now.AddMinutes((double)jwtDetails.ExpiryInMinutes),
            signingCredentials: credentials
            );

            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token).ToString();

            System.Console.WriteLine("your generated token is in next line\n");
            System.Console.WriteLine(generatedToken);
        }
    }
}