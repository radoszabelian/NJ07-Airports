using NJ07_Airports.Model;
using NJ07_Airports.Parser;
using NJ07_Airports.SerializeToJson;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NJ07_Airports
{
    public class CacheAndDataHandler
    {
        public List<City> Cities { get; set; }
        public List<Country> Countries { get; set; }
        public List<Airport> Airports { get; set; }
        public List<Airline> Airlines { get; set; }
        public List<Flight> Flights { get; set; }

        private readonly string _cacheFolder;
        private readonly string _rawFolder;

        private readonly string _airportsRawFileName;

        private readonly string _airportCacheFileName;
        private readonly string _citiesCacheFileName;
        private readonly string _countriesCacheFileName;
        private readonly string _flightsCacheFileName;
        private readonly string _airlinesCacheFileName;

        public CacheAndDataHandler(string airportRawFileName,
            string airportCacheFileName,
            string citiesCacheFileName,
            string countriesCacheFileName,
            string airlinesCacheFileName,
            string flightsCacheFileName,
            string cacheFolder,
            string rawFolder)
        {
            _airportsRawFileName = airportRawFileName;
            _airportCacheFileName = airportCacheFileName;
            _citiesCacheFileName = citiesCacheFileName;
            _countriesCacheFileName = countriesCacheFileName;
            _flightsCacheFileName = flightsCacheFileName;
            _airlinesCacheFileName = airlinesCacheFileName;

            _cacheFolder = cacheFolder;
            _rawFolder = rawFolder;
        }

        public void InitializeAppData()
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
            return File.Exists(Path.Combine(_cacheFolder, _countriesCacheFileName))
                    && File.Exists(Path.Combine(_cacheFolder, _airportCacheFileName))
                    && File.Exists(Path.Combine(_cacheFolder, _citiesCacheFileName))
                    && File.Exists(Path.Combine(_cacheFolder, _airlinesCacheFileName))
                    && File.Exists(Path.Combine(_cacheFolder, _flightsCacheFileName));
        }

        private void ReadDataFromCache()
        {
            Airports = Serializer.DeserializeFromJson<IEnumerable<Airport>>(Path.Combine(_cacheFolder, _airportCacheFileName)).ToList();
            Cities = Serializer.DeserializeFromJson<IEnumerable<City>>(Path.Combine(_cacheFolder, _citiesCacheFileName)).ToList();
            Countries = Serializer.DeserializeFromJson<IEnumerable<Country>>(Path.Combine(_cacheFolder, _countriesCacheFileName)).ToList();
            Airlines = Serializer.DeserializeFromJson<IEnumerable<Airline>>(Path.Combine(_cacheFolder, _airlinesCacheFileName)).ToList();
            Flights = Serializer.DeserializeFromJson<IEnumerable<Flight>>(Path.Combine(_cacheFolder, _flightsCacheFileName)).ToList();
        }

        private void ParseRawDataFiles()
        {
            ParsedAirportsDataBundle result = CsvHelper.ParseAirportFile(Path.Combine(_rawFolder, _airportsRawFileName));

            Airports = result.Airports;
            Cities = result.Cities;
            Countries = result.Countries;

            Airlines = CsvHelper.Parse<Airline>(Path.Combine(_rawFolder, "airlines.dat"));
            Flights = CsvHelper.Parse<Flight>(Path.Combine(_rawFolder, "flights.dat"));
        }

        private void SaveDataToCache()
        {
            if (!Directory.Exists(@"cache"))
            {
                Directory.CreateDirectory(@"cache");
            }

            Serializer.SerializeToJson(Airports, Path.Combine(_cacheFolder, _airportCacheFileName));
            Serializer.SerializeToJson(Cities, Path.Combine(_cacheFolder, _citiesCacheFileName));
            Serializer.SerializeToJson(Countries, Path.Combine(_cacheFolder, _countriesCacheFileName));
            Serializer.SerializeToJson(Flights, Path.Combine(_cacheFolder, _flightsCacheFileName));
            Serializer.SerializeToJson(Airlines, Path.Combine(_cacheFolder, _airlinesCacheFileName));
        }
    }
}
