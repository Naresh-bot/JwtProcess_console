namespace JWTProcessConsole.Models
{
    public static class Message
    {
        public const string TokenIsValid = "Token is valid.";
        public const string EnterOption = "Enter the Option :";
        public const string GetSecretKey = "Enter the custom security key:";
        public const string GetTokenInput = "Enter the jwt token:";
        public const string GetContinueInput = "Do you wish to continue (Yes/No): ";
        public static string RegistrationSuccess(int userId) => $"User registration successful. UserId: {userId}";
        public static string GeneratedToken(string token) => $"Your generated token is in next line\n\"token\":\"{token}\"";
        public static string PrintInvalidInput(int tries)
        {

            return $"Invalid Input - {tries} tries left..!";
        }
        public static string InvalidUser(int tries)
        {

            return $"User credentials incorrect - {tries} tries left..!";
        }

        public static string ErrorMessage(string message)
        {
            return $"Error message : {message}";
        }



    }
}