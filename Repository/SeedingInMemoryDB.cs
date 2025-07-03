namespace JWTProcessConsole.Repository
{
    public class SeedingInMemoryDB
    {
        public readonly AppDbContext _dbContext;
        public SeedingInMemoryDB(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (!_dbContext.Users.Any())
            {
                _dbContext.Users.AddRange(new List<Users>
                {
                    new Users { UserId = 1, Username = "Mukesh", Password = "1", Role = "Admin" },
                    new Users { UserId = 2, Username = "Lokesh", Password = "admin@123", Role = "Admin" },
                    new Users { UserId = 3, Username = "Madhan", Password = "admin_admin", Role = "User" }
                });
                _dbContext.SaveChanges();
            }
        }
    }
}