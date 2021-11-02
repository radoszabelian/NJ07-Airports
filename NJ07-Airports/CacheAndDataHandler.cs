using NJ07_Airports.Logging;
using NJ07_Airports.Model;
using NJ07_Airports.Parser;
using NJ07_Airports.SerializeToJson;
using NJ07_Airports.Services.CsvHelper;
using NJ07_Airports.Services.CsvHelper.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NJ07_Airports
{
    public class CacheAndDataHandler : ICacheAndDataHandler
    {
        public List<City> Cities { get; set; }
        public List<Country> Countries { get; set; }
        public List<Airport> Airports { get; set; }
        public List<Airline> Airlines { get; set; }
        public List<Flight> Flights { get; set; }

        private readonly InputPathsConfiguration _options;

        private ICsvHelper _csvHelper;
        private IAirportsDataConverter _airportsDataConverter;

        public CacheAndDataHandler(InputPathsConfiguration options,
            ICsvHelper csvHelper, IAirportsDataConverter airportsDataConverter)
        {
            _options = options;
            _csvHelper = csvHelper;
            _airportsDataConverter = airportsDataConverter;

            InitializeAppData();
        }

        private void InitializeAppData()
        {
            if (IsCacheAvailable())
            {
                ReadDataFromCache();
            }
            else
            {
                ParseRawDataFiles();
                SaveDataToCache();
            }
        }

        private bool IsCacheAvailable()
        {
            return File.Exists(Path.Combine(_options.CacheFolderName, _options.CountriesCacheFileName))
                    && File.Exists(Path.Combine(_options.CacheFolderName, _options.AirportsRawFileName))
                    && File.Exists(Path.Combine(_options.CacheFolderName, _options.CitiesCacheFileName))
                    && File.Exists(Path.Combine(_options.CacheFolderName, _options.AirlinesCacheFileName))
                    && File.Exists(Path.Combine(_options.CacheFolderName, _options.FlightsCacheFileName));
        }

        private void ReadDataFromCache()
        {
            Airports = Serializer.DeserializeFromJson<IEnumerable<Airport>>(Path.Combine(_options.CacheFolderName, _options.AirportsRawFileName)).ToList();
            Cities = Serializer.DeserializeFromJson<IEnumerable<City>>(Path.Combine(_options.CacheFolderName, _options.CitiesCacheFileName)).ToList();
            Countries = Serializer.DeserializeFromJson<IEnumerable<Country>>(Path.Combine(_options.CacheFolderName, _options.CountriesCacheFileName)).ToList();
            Airlines = Serializer.DeserializeFromJson<IEnumerable<Airline>>(Path.Combine(_options.CacheFolderName, _options.AirlinesCacheFileName)).ToList();
            Flights = Serializer.DeserializeFromJson<IEnumerable<Flight>>(Path.Combine(_options.CacheFolderName, _options.FlightsCacheFileName)).ToList();
        }

        private void ParseRawDataFiles()
        {
            List<AirportsParseResult> airportsParseResult = _csvHelper.Parse<AirportsParseResult>(Path.Combine(_options.RawFolderName, _options.AirportsRawFileName));
            var airportsConversionResult = _airportsDataConverter.ConvertToModel(airportsParseResult);

            Airports = airportsConversionResult.Airports;
            Cities = airportsConversionResult.Cities;
            Countries = airportsConversionResult.Countries;

            Airlines = _csvHelper.Parse<Airline>(Path.Combine(_options.RawFolderName, _options.AirlinesRawFileName));
            Flights = _csvHelper.Parse<Flight>(Path.Combine(_options.RawFolderName, _options.FlightsRawFileName));
        }

        private void SaveDataToCache()
        {
            if (!Directory.Exists(_options.CacheFolderName))
            {
                Directory.CreateDirectory(_options.CacheFolderName);
            }

            Serializer.SerializeToJson(Airports, Path.Combine(_options.CacheFolderName, _options.AirportsRawFileName));
            Serializer.SerializeToJson(Cities, Path.Combine(_options.CacheFolderName, _options.CitiesCacheFileName));
            Serializer.SerializeToJson(Countries, Path.Combine(_options.CacheFolderName, _options.CountriesCacheFileName));
            Serializer.SerializeToJson(Flights, Path.Combine(_options.CacheFolderName, _options.FlightsCacheFileName));
            Serializer.SerializeToJson(Airlines, Path.Combine(_options.CacheFolderName, _options.AirlinesCacheFileName));
        }
    }
}
