namespace Airports_Logic.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Airports_IO.Models;
    using Airports_IO.Services;
    using Airports_Logic.Model;

    public class DataHandler : IDataHandler
    {
        private readonly InputPathsConfiguration options;
        private IConfig configService;
        private ICsvHelper csvHelper;
        private IAirportsDataConverter airportsDataConverter;
        private ISerializer serializer;

        public DataHandler(IConfig configService,
            ICsvHelper csvHelper,
            IAirportsDataConverter airportsDataConverter,
            ISerializer serializer)
        {
            this.configService = configService;
            this.csvHelper = csvHelper;
            this.airportsDataConverter = airportsDataConverter;
            this.serializer = serializer;
            this.options = this.configService.Config;

            this.InitializeAppData();
        }

        public List<City> Cities { get; set; }

        public List<Country> Countries { get; set; }

        public List<Airport> Airports { get; set; }

        public List<Airline> Airlines { get; set; }

        public List<Flight> Flights { get; set; }

        private void InitializeAppData()
        {
            if (this.IsCacheAvailable())
            {
                this.ReadDataFromCache();
            }
            else
            {
                this.ParseRawDataFiles();
                this.SaveDataToCache();
            }
        }

        private bool IsCacheAvailable()
        {
            return File.Exists(Path.Combine(this.options.CacheFolderName, this.options.CountriesCacheFileName))
                    && File.Exists(Path.Combine(this.options.CacheFolderName, this.options.AirportsRawFileName))
                    && File.Exists(Path.Combine(this.options.CacheFolderName, this.options.CitiesCacheFileName))
                    && File.Exists(Path.Combine(this.options.CacheFolderName, this.options.AirlinesCacheFileName))
                    && File.Exists(Path.Combine(this.options.CacheFolderName, this.options.FlightsCacheFileName));
        }

        private void ReadDataFromCache()
        {
            this.Airports = this.serializer.DeserializeFromJson<IEnumerable<Airport>>(Path.Combine(this.options.CacheFolderName, this.options.AirportsRawFileName)).ToList();
            this.Cities = this.serializer.DeserializeFromJson<IEnumerable<City>>(Path.Combine(this.options.CacheFolderName, this.options.CitiesCacheFileName)).ToList();
            this.Countries = this.serializer.DeserializeFromJson<IEnumerable<Country>>(Path.Combine(this.options.CacheFolderName, this.options.CountriesCacheFileName)).ToList();
            this.Airlines = this.serializer.DeserializeFromJson<IEnumerable<Airline>>(Path.Combine(this.options.CacheFolderName, this.options.AirlinesCacheFileName)).ToList();
            this.Flights = this.serializer.DeserializeFromJson<IEnumerable<Flight>>(Path.Combine(this.options.CacheFolderName, this.options.FlightsCacheFileName)).ToList();
        }

        private void ParseRawDataFiles()
        {
            List<AirportsParseResult> airportsParseResult = this.csvHelper.Parse<AirportsParseResult>(Path.Combine(this.options.RawFolderName, this.options.AirportsRawFileName));
            var airportsConversionResult = this.airportsDataConverter.ConvertToModel(airportsParseResult);

            this.Airports = airportsConversionResult.Airports;
            this.Cities = airportsConversionResult.Cities;
            this.Countries = airportsConversionResult.Countries;
            this.Airlines = this.csvHelper.Parse<Airline>(Path.Combine(this.options.RawFolderName, this.options.AirlinesRawFileName));
            this.Flights = this.csvHelper.Parse<Flight>(Path.Combine(this.options.RawFolderName, this.options.FlightsRawFileName));
        }

        private void SaveDataToCache()
        {
            if (!Directory.Exists(this.options.CacheFolderName))
            {
                Directory.CreateDirectory(this.options.CacheFolderName);
            }

            this.serializer.SerializeToJson(this.Airports, Path.Combine(this.options.CacheFolderName, this.options.AirportsRawFileName));
            this.serializer.SerializeToJson(this.Cities, Path.Combine(this.options.CacheFolderName, this.options.CitiesCacheFileName));
            this.serializer.SerializeToJson(this.Countries, Path.Combine(this.options.CacheFolderName, this.options.CountriesCacheFileName));
            this.serializer.SerializeToJson(this.Flights, Path.Combine(this.options.CacheFolderName, this.options.FlightsCacheFileName));
            this.serializer.SerializeToJson(this.Airlines, Path.Combine(this.options.CacheFolderName, this.options.AirlinesCacheFileName));
        }
    }
}
