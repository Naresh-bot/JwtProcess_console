using JWTProcessConsole.Handler;
using JWTProcessConsole.Models;
namespace JWTProcessConsole
{
    class ConsoleOptionsHandler : IConsoleOptionsHandler
    {

        public int currentOption { get; set; }
        public int trial { get; set; } = 3;
        public bool retry { get; set; }
        public readonly IConsoleWriter _consoleWriter;
        public readonly ICustomTokenHandler _tokenHandler;
        public readonly IUserHandler _userHandler;

        public ConsoleOptionsHandler(IConsoleWriter consoleWriter, IUserHandler userHandler,
        ICustomTokenHandler tokenHandler)
        {
            _consoleWriter = consoleWriter;
            _tokenHandler = tokenHandler;
            _userHandler = userHandler;
        }

        public void GetPrimaryInput()
        {
            String? input = null;
            do
            {
                _consoleWriter.TokenHandlingOptions();
                if (retry)
                    _consoleWriter.PrintMessage(Message.PrintInvalidInput(trial));

                input = _consoleWriter.GetUserInput(Message.EnterOption)!;
                currentOption = ValidateCurrentOption(3, input);
                if (trial == 0)
                {
                    _consoleWriter.StopApp();
                }
            }
            while (retry);
            HandlePrimaryOptions(currentOption);

        }

        public void HandlePrimaryOptions(int currentOption)
        {
            string token;
            string userInput;
            int userOption;
            switch (currentOption)
            {
                case 1:
                    //token generation
                    do
                    {
                        _consoleWriter.UserInfoOptions();
                        if (retry)
                            _consoleWriter.PrintMessage(Message.InvalidUser(trial));
                        userInput = _consoleWriter.GetUserInput(Message.EnterOption)!;
                        userOption = ValidateCurrentOption(3, userInput);
                    } while (retry);
                    HandleUserOptions(userOption);
                    break;
                case 2:
                    //call validate token
                    token = _consoleWriter.GetUserInput(Message.GetTokenInput)!;
                    do
                    {
                        _consoleWriter.SecurityKeyOptions();
                        if (retry)
                            _consoleWriter.PrintMessage(Message.PrintInvalidInput(trial));

                        userInput = _consoleWriter.GetUserInput(Message.EnterOption)!;
                        userOption = ValidateCurrentOption(2, userInput);
                    } while (retry);
                    _tokenHandler.ValidateToken(token, userOption);
                    GetContinueInput();

                    break;
                case 3:
                    //read jwt
                    token = _consoleWriter.GetUserInput(Message.GetTokenInput)!;
                    _tokenHandler.ParseToken(token);
                    GetContinueInput();
                    break;

            }
        }

        public void HandleUserOptions(int option)
        {
            Users users = new();
            UserContext userContext = new();
            switch (option)
            {
                case 1:
                    //existing user
                    do
                    {
                        _consoleWriter.UserInfoOptions();
                        if (retry)
                            _consoleWriter.PrintMessage(Message.PrintInvalidInput(trial));

                        var Input = _consoleWriter.GetUserCredentials();
                        users = _userHandler.ValidateUser(Input);
                        retry = users.UserId == 0 ? true : false;  
                    } while (retry);
                    userContext.users = users;
                    _tokenHandler.GenerateToken(userContext);
                    GetContinueInput();
                    break;

                case 2:
                    //register user
                    users = _consoleWriter.RegisterUser();
                    int userId = _userHandler.RegisterUser(users);
                    _consoleWriter.PrintMessage(Message.RegistrationSuccess(userId));
                    GetContinueInput();
                    break;

                case 3:
                    //anonymous user
                    userContext = _consoleWriter.GetAnonymousUserDetails();
                    _tokenHandler.GenerateToken(userContext);
                    GetContinueInput();
                    break;
            }
        }



        public void GetContinueInput()
        {
            var input = _consoleWriter.GetUserInput(Message.GetContinueInput);
            string[] affirm = ["yes", "y"];

            bool cont = affirm.Contains(input!.ToLower());

            if (cont)
            {
                _consoleWriter.ClearConsole();
                GetPrimaryInput();
            }
        }

        public int ValidateCurrentOption(int maxLimit, string? input)
        {
            int trial = 3;
            int option = 0;
            if (!Int32.TryParse(input, out option))
            {
                trial--;
                retry = true;

            }
            else if (option > 0 && option <= maxLimit)
            {
                trial = 3;
                retry = false;

            }
            else
            {
                trial--;
                retry = true;
            }

            return option;

        }
    }
}