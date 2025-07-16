namespace JWTProcessConsole
{
    interface ITokenGenerator
    {
        public string GenerateToken(UserContext userContext); 
    }
    
}

