using System.IdentityModel.Tokens.Jwt;
using JWTProcessConsole.Models;
namespace JWTProcessConsole
{
    public class ConsoleWriter : IConsoleWriter
    {
        public void Heading()
        {
            ClearConsole();
            System.Console.WriteLine(new string('=', 32));
            System.Console.WriteLine($"||{new String(' ', 7)}JWT PROCESSOR {new String(' ', 7)}||");
            System.Console.WriteLine(new string('=', 32));
        }

        #region  optionSet
        public void TokenHandlingOptions()
        {
            Heading();
            System.Console.WriteLine("1.Generate JWT token");
            System.Console.WriteLine("2.Validate JWT token");
            System.Console.WriteLine("3.Read JWT\n\n");
        }
        public void UserInfoOptions()
        {
            Heading();
            System.Console.WriteLine("1.Existing User");
            System.Console.WriteLine("2.Register New User");
            System.Console.WriteLine("3.Anonymous User\n\n");
        }
        public void SecurityKeyOptions()
        {
            Heading();
            System.Console.WriteLine("1.Use System security key");
            System.Console.WriteLine("2.Use custom security key\n\n");
        }
        #endregion

        public string? GetUserInput(string message)
        {
            PrintMessageInLine(message);
            string? input = Console.ReadLine();
            return input;

        }

        public void StopApp()
        {
            Console.WriteLine("Application is closing");
            Environment.Exit(0);
        }

        public void ClearConsole()
        {
            Console.Clear();
        }

        public Users GetUserCredentials()
        {
            Users user = new Users();
            Console.Write("Enter username:");
            user.Username = Console.ReadLine()!;

            Console.Write("\nEnter password:");
            user.Password = Console.ReadLine()!;

            return user;
        }

        public Users RegisterUser()
        {

            Users user = new Users();
            Console.WriteLine("Enter user details for Registration:");
            Console.WriteLine("User Name:\t");

            user.Username = Console.ReadLine()!;
            Console.WriteLine("User Role:\t");

            user.Role = Console.ReadLine()!;
            Console.WriteLine("Password:\t");

            user.Password = Console.ReadLine()!;

            return user;

        }
        public UserContext GetAnonymousUserDetails()
        {
            UserContext context = new();
            Users user = new();
            JwtDetails jwtDetails = new();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();


            System.Console.Write("Enter token Issuer:");
            jwtDetails.Issuer = Console.ReadLine()!;

            System.Console.Write("Enter token Audience:");
            jwtDetails.Audience = Console.ReadLine()!;

            Console.Write("Enter security key:");
            jwtDetails.SecurityKey = Console.ReadLine()!;

            System.Console.Write("Enter expiry time in minutes:");
            jwtDetails.ExpiryInMinutes = Convert.ToInt32(Console.ReadLine());

            System.Console.Write("Enter no of claims:");
            int claimCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < claimCount; i++)
            {
                System.Console.Write($"Enter claim type {i + 1}:");
                var key = Console.ReadLine();

                System.Console.Write($"Enter claim Value {i + 1}:");
                var value = Console.ReadLine();

                keyValuePairs.Add(key!, value!);
            }

            jwtDetails.claims = keyValuePairs;
            context.users = user;
            context.jwtdetails = jwtDetails;

            return context;

        }
        public void PrintParsedToken(JwtSecurityToken token)
        {
            Console.WriteLine("Token Header:\n{");
            foreach (var header in token.Header)
            {
                Console.WriteLine($"\"{header.Key}\": \"{header.Value}\"");
            }

            Console.WriteLine("}\nToken Payload (Claims):\n{");
            foreach (var claim in token.Claims)
            {
                Console.WriteLine($"\"{claim.Type}\": \"{claim.Value}\"");
            }

            Console.WriteLine("}"+ $"\nToken Expiration: {token.ValidTo}");
            Console.WriteLine($"Token valid till: {Convert.ToInt32((token.ValidTo.ToLocalTime() - DateTime.Now.ToLocalTime()).TotalMinutes)} minutes");

        }

        public void PrintTokenValidationResult(JwtSecurityToken jwtToken)
        {
            Console.WriteLine("Token is valid.");

            if (jwtToken != null)
            {
                DateTime expiry = jwtToken.ValidTo; // UTC time 
                Console.WriteLine($"Token expires at (Local Time): {expiry.ToLocalTime()}");
                Console.WriteLine($"Token valid till: {Convert.ToInt32((expiry.ToLocalTime() - DateTime.Now.ToLocalTime()).TotalMinutes)} minutes");
            }

        }

        public void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }
        public void PrintMessageInLine(string message)
        {
            Console.Write(string.Format(message));
        }
    }
}