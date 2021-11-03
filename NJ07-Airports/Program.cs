namespace NJ07_Airports
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NJ07_Airports.Commands.GeoLocation;
    using NJ07_Airports.Logging;
    using NJ07_Airports.Model;
    using NJ07_Airports.Services.CsvHelper;

    public class Program
    {
        public static void Main(string[] args)
        {
            // https://pradeeploganathan.com/dotnet/configuration-in-a-net-core-console-application/
            var inputPathsConfiguration = new InputPathsConfiguration();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();

            configuration.Bind("InputPaths", inputPathsConfiguration);

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
