
using Microsoft.EntityFrameworkCore;

namespace JWTProcessConsole
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        
        public DbSet<Users> Users { get; set; }
    }    
}