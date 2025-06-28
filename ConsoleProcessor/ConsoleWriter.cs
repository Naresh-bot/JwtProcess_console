using System.Linq;
namespace JWTProcessConsole
{
    class ConsoleWriter : IConsoleWriter
    {
        public void Heading()
        {
            ClearConsole();
            System.Console.WriteLine("============================");
            System.Console.WriteLine("      JWT PROCESSOR         ");
            System.Console.WriteLine("============================");
        }
        #region  optionSet
        public void TokenHandlingOptions() //TokenHandlingOptions
        {
            Heading();
            System.Console.WriteLine("1.Generate JWT token");
            System.Console.WriteLine("2.Validate JWT token");
            System.Console.WriteLine("3.Read JWT\n\n");
        }
        public void UserInfoOptions() //UserInfoOptions
        {
            Heading();
            System.Console.WriteLine("1.Existing User");
            System.Console.WriteLine("2.Register New User");
            System.Console.WriteLine("3.Anonymous User\n\n");
        }
        public void SecurityKeyOptions() //SecurityKeyOptions
        {
            Heading();
            System.Console.WriteLine("1.Use System security key");
            System.Console.WriteLine("2.Use custom security key\n\n");
        }
        #endregion


        public string? GetUserInput()
        {
            //TokenHandlingOptions();
            System.Console.WriteLine("Enter the Option:");

            string? input = Console.ReadLine();
            return input;

        }

        public string? GetSecretKey()
        {
            System.Console.WriteLine("Enter the custom security key:");

            string? input = Console.ReadLine();
            return input;

        }

        public void PrintInvalidInput()
        {
            //Console.Clear();
            Console.WriteLine($"Invalid Input - {App.trial} tries left..!");
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

        public string GetUserId()
        {
            // Users user = new Users();
            // bool retry = false;
            Console.WriteLine("Enter user id: ");
            return Console.ReadLine();
        }

        public string GetTokenInput()
        {
            Console.WriteLine("Enter the jwt token: \n");
            return Console.ReadLine();
        }

        public string GetContinueInput()
        {
            Console.WriteLine("Do you wish to continue: ");
            return Console.ReadLine();
        }

        public void InvalidUser()
        {
            Console.WriteLine("User Does not exist");
            Console.WriteLine($"Invalid Input - {App.trial} tries left");
        }

        public Users RegisterUser()
        {

            Users user = new Users();
            Console.WriteLine("Enter user details for Registration:");
            Console.WriteLine("User Name:\t");
            user.Username = Console.ReadLine();
            Console.WriteLine("User Role:\t");
            user.Role = Console.ReadLine();
            Console.WriteLine("Password:\t");
            user.Password = Console.ReadLine();

            return user;

        }

        public void RegistrationSuccess(int userId)
        {
            System.Console.WriteLine("User Registered successfully.");
            System.Console.WriteLine($"Your userId is {userId}");
        }

        public UserContext GetAnonymousUserDetails(bool istokenValidation = false)
        {
            UserContext context = new();
            Users user = new();
            JwtDetails jwtDetails = new();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            if (!istokenValidation)
            {
                Console.WriteLine("Enter User Name:");
                user.Username = Console.ReadLine();

                Console.WriteLine("Enter user role");
                user.Role = Console.ReadLine();

                Console.WriteLine("Enter security key:");
                jwtDetails.SecurityKey = Console.ReadLine();

                System.Console.WriteLine("Enter expiry time in minutes:");
                jwtDetails.ExpiryInMinutes = Convert.ToInt32(Console.ReadLine());
            }

            System.Console.WriteLine("Enter token Issuer:");
            jwtDetails.Issuer = Console.ReadLine();

            System.Console.WriteLine("Enter token Audience:");
            jwtDetails.Audience = Console.ReadLine();

            if (!istokenValidation)
            {
                System.Console.WriteLine("Enter no of claims:");
                int claimCount = Convert.ToInt32(Console.ReadLine());

                for (int i = 0; i < claimCount; i++)
                {
                    System.Console.WriteLine($"Enter claim type {i + 1}:");
                    var key = Console.ReadLine();

                    System.Console.WriteLine($"Enter claim Value {i + 1}:");
                    var value = Console.ReadLine();

                    keyValuePairs.Add(key, value);
                }
            }

            jwtDetails.claims = keyValuePairs;
            context.users = user;
            context.jwtdetails = jwtDetails;

            return context;

        }



    }
}