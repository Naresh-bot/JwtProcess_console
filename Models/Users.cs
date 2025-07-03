using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace JWTProcessConsole
{
    public class Users
    {    [Key]
        public int UserId { get; set; }  
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime Expirydate { get; set; }
        public string Password { get; set; }
    }
    
    public class JwtDetails
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
        public int ExpiryInMinutes { get; set; }
        public Dictionary<string, string> claims { get; set; }

    }
    public class UserContext
    {
        public Users users { get; set; }
        public JwtDetails jwtdetails { get; set; } 
    }
}