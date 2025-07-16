using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace JWTProcessConsole
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime Expirydate { get; set; }
        public string Password { get; set; }
    }

}