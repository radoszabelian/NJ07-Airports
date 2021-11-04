namespace Airports_CLI
{
    using Airports_IO.Services;
    using Airports_Logic;
    using Airports_Logic.Services;
    using Autofac;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DataHandler>().As<IDataHandler>();
            builder.RegisterType<LoggerService>().As<ILogger>();
            builder.RegisterInstance(new ConfigService("appSettings.json")).As<IConfig>();
            builder.RegisterType<AirportsDataConverter>().As<IAirportsDataConverter>();
            builder.RegisterType<LoggerService>().As<ILogger>();
            builder.RegisterType<GeoLocation>();
            builder.RegisterType<ExerciseResultsUtility>();
            builder.RegisterType<CsvHelper>().As<ICsvHelper>();
            builder.RegisterType<Serializer>().As<ISerializer>();
            builder.RegisterType<Menu>();

            var container = builder.Build();

            container.Resolve<Menu>().Start();
        }
    }
}
