using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JWTProcessConsole {
    class Program
    {
        public static void Main(String[] args)
        {

            /*

            IConfiguration configuration = new ConfigurationBuilder.AddJsonFile()
            need to add jwtauthenticationservice
            var serviceProvider = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = new ConfigurationBuilder().
                    }
                })

                /// add this to the csproj file
                /// console doesn't automatically add the appsettings.json to bin folder
                /// 
                    <ItemGroup>
                        <None Update="appsettings.json">
                        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
                        </None>
                    </ItemGroup>
            */
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false,
            reloadOnChange: true);
            //if not set to base path, it expects the json inside bin folder

            IConfiguration configuration = builder.Build();

            var services = new ServiceCollection();

            //reference to IServiceCollection
            services.AddSingleton<ICustomConsoleHandler, CustomConsoleHandler>()
           .AddSingleton<ICustomTokenHandler, CustomTokenHandler>()
           .AddSingleton<IConsoleWriter, ConsoleWriter>()
           .AddSingleton<ITokenGenerator, TokenGenerator>()
           .AddSingleton<ITokenParser, TokenParser>()
           .AddSingleton<ITokenValidator, TokenValidator>()
           //.AddSingleton<UserContext>()
           .AddSingleton<App>()
           .AddSingleton<IConfiguration>(configuration)
           .AddDbContext<AppDbContext>(options=> options.UseInMemoryDatabase("TestDB"));
           //options => options.UseSqlServer("connectionString")

            //returns AuthenticationBuilder
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
                        };
                    });

            var serviceProvider = services.BuildServiceProvider();
            var app = serviceProvider.GetRequiredService<App>();

            app.StartProcess();
        }
        
    }

    class App
    {
        private readonly ICustomConsoleHandler _consoleHandler;
        public static int trial = 3;
        public App(ICustomConsoleHandler consoleHandler)
        {
            _consoleHandler = consoleHandler;
        }

        public void StartProcess()
        {
            _consoleHandler.GetPrimaryInput();
        }
        

    }

    
    
}