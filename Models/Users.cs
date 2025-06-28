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
    class UserContext
    {
        public Users users { get; set; }
        public JwtDetails jwtdetails { get; set; }

        // public UserContext()
        // {
        //     users = new List<Users>{
        //         new Users(){ UserId = 1,Username = "Mukesh", Password = "1", Role = "Admin"},
        //         new Users(){ UserId = 2,Username = "Lokesh", Password = "admin@123", Role = "Admin"},
        //         new Users(){ UserId = 3,Username = "Madhan", Password = "admin_admin", Role = "User"}
        //     };
        // }
    }
}