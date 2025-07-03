namespace  JWTProcessConsole.Handler
{
    public interface IUserHandler
    {
        public int RegisterUser(Users users);
        public Users ValidateUser(Users users);
    }
}