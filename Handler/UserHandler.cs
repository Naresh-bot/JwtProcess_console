namespace JWTProcessConsole.Handler
{
    public class UserHandler : IUserHandler
    {
        public readonly AppDbContext _dbContext;
        public readonly IConsoleWriter _consoleWriter;
        public UserHandler(AppDbContext dbContext, IConsoleWriter consoleWriter)
        {
            _dbContext = dbContext;
            _consoleWriter = consoleWriter;
        }

        public int RegisterUser(Users users)
        {
            var latestId = _dbContext.Users.Max(x => x.UserId);
            users.UserId = ++latestId;
            _dbContext.Users.Add(users);
            _dbContext.SaveChanges();

            return users.UserId;
        }

        public Users ValidateUser(Users Input)
        {

            int trial = 3;
            Users user = new();
            user = _dbContext.Users.FirstOrDefault(x => x.Username == Input.Username && x.Password == Input.Password) ?? new Users();
            if (user.UserId == 0)
            {
                trial--;

                if (trial == 1)
                {
                    _consoleWriter.StopApp();
                }


            }
            return user;
        }
    }
}