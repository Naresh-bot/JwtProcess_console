namespace JWTProcessConsole
{
    interface ITokenGenerator
    {
        public void GenerateToken(Users user,string secretkey);
        public void GenerateAnonymousUserToken(UserContext userContext);
    }
    
}

