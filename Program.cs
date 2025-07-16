using System.Text;
using JWTProcessConsole.Handler;
using JWTProcessConsole.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JWTProcessConsole
{
    class Program
    {
        public static void Main(String[] args)
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false,
            reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConsoleOptionsHandler, ConsoleOptionsHandler>()
           .AddSingleton<ICustomTokenHandler, CustomTokenHandler>()
           .AddSingleton<IConsoleWriter, ConsoleWriter>()
           .AddSingleton<ITokenGenerator, TokenGenerator>()
           .AddSingleton<ITokenParser, TokenParser>()
           .AddSingleton<ITokenValidator, TokenValidator>()
           .AddSingleton<IUserHandler, UserHandler>()
           .AddSingleton<IConfiguration>(configuration)
           .AddSingleton<SeedingInMemoryDB>()
           .AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDB")); 
           
            var serviceProvider = services.BuildServiceProvider();
            var seedClass = serviceProvider.GetRequiredService<SeedingInMemoryDB>();
            seedClass.Seed();

            var app = serviceProvider.GetRequiredService<IConsoleOptionsHandler>();
            app.GetPrimaryInput();
        }

    }
}