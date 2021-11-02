using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NJ07_Airports.Commands.GeoLocation;
using NJ07_Airports.Logging;
using NJ07_Airports.Model;
using NJ07_Airports.Services.CsvHelper;

namespace NJ07_Airports
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://pradeeploganathan.com/dotnet/configuration-in-a-net-core-console-application/
            var inputPathsConfiguration = new InputPathsConfiguration();

            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();

            Configuration.Bind("InputPaths", inputPathsConfiguration);

            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILogger, LoggerService>()
                .AddSingleton(inputPathsConfiguration)
                .AddSingleton<ExerciseResultsUtility>()
                .AddSingleton<IAirportsDataConverter, AirportsDataConverter>()
                .AddSingleton<ICacheAndDataHandler, CacheAndDataHandler>()
                .AddSingleton<ICsvHelper, CsvHelper>()
                .AddSingleton<GeoLocation>()
                .AddSingleton<Menu>()
                .BuildServiceProvider();

            serviceProvider.GetService<Menu>().Start();
        }
    }
}
