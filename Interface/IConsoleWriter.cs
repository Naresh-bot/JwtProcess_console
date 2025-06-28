namespace JWTProcessConsole
{
    interface IConsoleWriter
    {
        public void PrintInvalidInput();
        // public void PrintTokenGenSuccess();

        // public int InvalidInput();
        public void StopApp();

        public string GetTokenInput();
        public string GetContinueInput();
        public string GetUserId();
        public void ClearConsole();

        public void InvalidUser();
        public void TokenHandlingOptions();
        public void UserInfoOptions();
        public void SecurityKeyOptions();
        public string? GetUserInput();
        public string? GetSecretKey();
        public Users RegisterUser();
        public void RegistrationSuccess(int userId);
        public UserContext GetAnonymousUserDetails(bool istokenValidation);
    }
    
}
