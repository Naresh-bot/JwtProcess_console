
namespace JWTProcessConsole
{
    interface ITokenValidator
    {
        public void ValidateToken(string token,int keytypeId);
    }
}           