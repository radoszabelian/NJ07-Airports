namespace Airports_CLI
{
    using Aiports_Model;
    using Airports_IO.Services;
    using Airports_Logic.Services;
    using Airports_Logic.Services.GeoLocation;
    using Airports_Settings.Services;
    using Autofac;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DataAccessor>().As<IDataAccessor>();
            builder.RegisterType<LoggerService>().As<ILogger>();
            builder.RegisterInstance(new ConfigService("appSettings.json")).As<IConfig>();
            builder.RegisterType<AirportsDataConverter>().As<IAirportsDataConverter>();
            builder.RegisterType<GeoLocationService>().As<IGeoLocationService>();
            builder.RegisterType<GeoLocationCommand>();
            builder.RegisterType<QueryResultsCommand>();
            builder.RegisterType<CsvHelper>().As<ICsvHelper>();
            builder.RegisterType<Serializer>().As<ISerializer>();
            builder.RegisterType<Menu>();
            var container = builder.Build();

            container.Resolve<Menu>().Start();
        }
    }
}
