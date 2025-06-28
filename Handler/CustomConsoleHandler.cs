
namespace JWTProcessConsole
{
    class CustomConsoleHandler : ICustomConsoleHandler
    {

        public int currentOption { get; set; }
        public static bool retry { get; set; }
        public readonly IConsoleWriter _consoleWriter;
        public readonly AppDbContext _dbContext;
        public readonly ITokenGenerator _tokenGenerator;
        public readonly ITokenValidator _tokenValidator;
        public readonly ITokenParser _tokenParser;

        public CustomConsoleHandler(IConsoleWriter consoleWriter,
        ITokenGenerator tokenGenerator, ITokenValidator tokenValidator,
        ITokenParser tokenParser, AppDbContext dbContext)
        {
            _consoleWriter = consoleWriter;
            _tokenGenerator = tokenGenerator;
            _tokenValidator = tokenValidator;
            _tokenParser = tokenParser;
            _dbContext = dbContext;
            LoadDefaultUsers();
        }

        public void LoadDefaultUsers()
        {
            if (!_dbContext.Users.Any())
            {
                _dbContext.Users.AddRange(new List<Users>{
                    new Users(){ UserId = 1,Username = "Mukesh", Password = "1", Role = "Admin"},
                    new Users(){ UserId = 2,Username = "Lokesh", Password = "admin@123", Role = "Admin"},
                    new Users(){ UserId = 3,Username = "Madhan", Password = "admin_admin", Role = "User"}
                });
                _dbContext.SaveChanges();
            }
        }

        public void GetPrimaryInput()
        {
            String? input = null;
            do
            {
                PrintOptionset(1);
                input = _consoleWriter.GetUserInput();
                currentOption = ValidateCurrentOption(3, input);
                if (App.trial == 0)
                {
                    _consoleWriter.StopApp();
                }
            }
            while (retry);
            HandlePrimaryOptions(currentOption);

        }

        public void PrintOptionset(int optionSet)
        {
            switch (optionSet)
            {
                case 1: //token handling
                    if (retry)
                    {
                        _consoleWriter.ClearConsole();
                        _consoleWriter.TokenHandlingOptions();
                        _consoleWriter.PrintInvalidInput();
                    }
                    else
                    {
                        _consoleWriter.TokenHandlingOptions();
                    }
                    break;
                case 2: //user type
                    if (retry)
                    {
                        _consoleWriter.ClearConsole();
                        _consoleWriter.UserInfoOptions();
                        _consoleWriter.InvalidUser();
                    }
                    else
                    {
                        _consoleWriter.UserInfoOptions();
                    }
                    break;
                case 3: //security key type
                if (retry)
                    {
                        _consoleWriter.ClearConsole();
                        _consoleWriter.SecurityKeyOptions();
                        _consoleWriter.PrintInvalidInput();

                    }
                    else
                    {
                        _consoleWriter.SecurityKeyOptions();
                    }
                    break;
            }
        }

        public void HandlePrimaryOptions(int currentOption)
        {
            string token = "";
            string userInput = "";
            int userOption;
            switch (currentOption)
            {
                case 1:
                    //token generation
                    do
                    {
                        PrintOptionset(2);
                        userInput = _consoleWriter.GetUserInput();
                        userOption = ValidateCurrentOption(3, userInput);
                    } while (retry);
                    HandleUserOptions(userOption);
                    break;
                case 2:
                    //call validate token
                    token = _consoleWriter.GetTokenInput(); 
                    do
                    {
                        PrintOptionset(3);
                        userInput = _consoleWriter.GetUserInput();
                        userOption = ValidateCurrentOption(2, userInput);
                    } while (retry); 
                    _tokenValidator.ValidateToken(token,userOption);
                    GetContinueInput();

                    break;
                case 3:
                    //read jwt
                    token = _consoleWriter.GetTokenInput();
                    _tokenParser.ParseToken(token);
                    GetContinueInput();
                    break;

            }
        }

        public void HandleUserOptions(int option)
        {
            Users users = new();
            UserContext userContext = new();
            int keytypeId;
            switch (option)
            {
                case 1:
                    //existing user
                    do
                    {
                        PrintOptionset(2);
                        var Input = _consoleWriter.GetUserId();
                        users = ValidateUserId(Input);
                    } while (retry);
                    //get type of secretkey
                    string secretkey = null;
                    do
                    {
                        PrintOptionset(3);
                        var Input = _consoleWriter.GetUserInput();
                        keytypeId = ValidateCurrentOption(2, Input);
                    } while (retry);
                    if (keytypeId == 2)
                    {
                        secretkey = _consoleWriter.GetSecretKey();
                    }
                        _tokenGenerator.GenerateToken(users, secretkey);
                    GetContinueInput();
                    break;
                case 2:
                    //register user
                    users = _consoleWriter.RegisterUser();
                    var latestId = _dbContext.Users.Max(x => x.UserId);
                    users.UserId = ++latestId;
                    _dbContext.Users.Add(users);
                    _dbContext.SaveChanges();
                    _consoleWriter.RegistrationSuccess(users.UserId);
                    GetContinueInput(); 
                    break;
                case 3:
                    //anonymous user
                    userContext = _consoleWriter.GetAnonymousUserDetails(false);
                    _tokenGenerator.GenerateAnonymousUserToken(userContext);
                    GetContinueInput();
                    break;

            }
        }
 
        public Users ValidateUserId(string Input)
        {
            Users user = new();
            if (Int32.TryParse(Input, out int userId))
            {
                //Iterate through the users list
                user = _dbContext.Users.FirstOrDefault(x => x.UserId == userId) ?? new Users();
                if (user.UserId == 0)
                {
                    //_consoleWriter.InvalidUser();
                    retry = true;
                    while (retry)
                    {
                        App.trial--;
                        //HandlePrimaryOptions(1);

                        if (App.trial == 1)
                        {
                            _consoleWriter.StopApp();
                        }
                    }

                }
            }
            return user;
        }

        public void GetContinueInput()
        {
            var input = _consoleWriter.GetContinueInput();
            string[] affirm = ["yes", "y"];

            bool cont = affirm.Contains(input.ToLower());

            if (cont)
            {
                _consoleWriter.ClearConsole();
                GetPrimaryInput();
            }
        }

        public int ValidateCurrentOption(int maxLimit, string? input)
        {

            int option = 0;
            if (!Int32.TryParse(input, out option))
            {
                App.trial--;
                retry = true;

            }
            else if (option > 0 && option <= maxLimit)
            {
                App.trial = 3;
                retry = false;

            }
            else
            {
                App.trial--;
                retry = true;
            }

            return option;

        }
    }
}