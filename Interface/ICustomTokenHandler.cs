namespace JWTProcessConsole
{
    public interface ICustomTokenHandler
    {
        public void GenerateToken(UserContext userContext); 
        public void ParseToken(string token);
        public void ValidateToken(string token, int keytypeId);

    }

}

