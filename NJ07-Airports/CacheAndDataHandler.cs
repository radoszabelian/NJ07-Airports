namespace NJ07_Airports
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NJ07_Airports.Model;
    using NJ07_Airports.SerializeToJson;
    using NJ07_Airports.Services.CsvHelper;
    using NJ07_Airports.Services.CsvHelper.Models;

    public class CacheAndDataHandler : ICacheAndDataHandler
    {
        private readonly InputPathsConfiguration options;

        private ICsvHelper csvHelper;

        private IAirportsDataConverter airportsDataConverter;

        public CacheAndDataHandler(InputPathsConfiguration options, ICsvHelper csvHelper, IAirportsDataConverter airportsDataConverter)
        {
            this.options = options;
            this.csvHelper = csvHelper;
            this.airportsDataConverter = airportsDataConverter;

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
            this.Airports = Serializer.DeserializeFromJson<IEnumerable<Airport>>(Path.Combine(this.options.CacheFolderName, this.options.AirportsRawFileName)).ToList();
            this.Cities = Serializer.DeserializeFromJson<IEnumerable<City>>(Path.Combine(this.options.CacheFolderName, this.options.CitiesCacheFileName)).ToList();
            this.Countries = Serializer.DeserializeFromJson<IEnumerable<Country>>(Path.Combine(this.options.CacheFolderName, this.options.CountriesCacheFileName)).ToList();
            this.Airlines = Serializer.DeserializeFromJson<IEnumerable<Airline>>(Path.Combine(this.options.CacheFolderName, this.options.AirlinesCacheFileName)).ToList();
            this.Flights = Serializer.DeserializeFromJson<IEnumerable<Flight>>(Path.Combine(this.options.CacheFolderName, this.options.FlightsCacheFileName)).ToList();
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

            Serializer.SerializeToJson(this.Airports, Path.Combine(this.options.CacheFolderName, this.options.AirportsRawFileName));
            Serializer.SerializeToJson(this.Cities, Path.Combine(this.options.CacheFolderName, this.options.CitiesCacheFileName));
            Serializer.SerializeToJson(this.Countries, Path.Combine(this.options.CacheFolderName, this.options.CountriesCacheFileName));
            Serializer.SerializeToJson(this.Flights, Path.Combine(this.options.CacheFolderName, this.options.FlightsCacheFileName));
            Serializer.SerializeToJson(this.Airlines, Path.Combine(this.options.CacheFolderName, this.options.AirlinesCacheFileName));
        }
    }
}
