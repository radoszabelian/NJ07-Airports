namespace Airports_IO.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using BsModel = Aiports_Model;
    using IOModel = Airports_IO.Model;
    using Airports_Settings.Model;
    using AutoMapper;
    using System;
    using Airports_Settings.Services;

    public class DataAccessor : BsModel.IDataAccessor
    {
        private readonly FilePaths options;
        private IConfig configService;
        private ICsvHelper csvHelper;
        private IAirportsDataConverter airportsDataConverter;
        private ISerializer serializer;

        private MapperConfiguration mapperConfiguration;

        public DataAccessor(IConfig configService,
            ICsvHelper csvHelper,
            IAirportsDataConverter airportsDataConverter,
            ISerializer serializer)
        {
            this.configService = configService;
            this.csvHelper = csvHelper;
            this.airportsDataConverter = airportsDataConverter;
            this.serializer = serializer;
            this.options = this.configService.GetConfigSection<FilePaths>("FilePaths");

            this.InitializeAppData();
        }

        public IEnumerable<BsModel.City> Cities { get; set; }

        public IEnumerable<BsModel.Country> Countries { get; set; }

        public IEnumerable<BsModel.Airport> Airports { get; set; }

        public IEnumerable<BsModel.Airline> Airlines { get; set; }

        public IEnumerable<BsModel.Flight> Flights { get; set; }

        private void InitializeAppData()
        {
            CreateAutoMapperMappings();

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

        private void CreateAutoMapperMappings()
        {
            mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IOModel.Airline, BsModel.Airline>();
                cfg.CreateMap<IOModel.Airport, BsModel.Airport>();
                cfg.CreateMap<IOModel.City, BsModel.City>();
                cfg.CreateMap<IOModel.Country, BsModel.Country>();
                cfg.CreateMap<IOModel.Flight, BsModel.Flight>();
                cfg.CreateMap<IOModel.Location, BsModel.Location>();
                cfg.CreateMap<IOModel.Segment, BsModel.Segment>();
            });
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
            var ioAirports = this.serializer.DeserializeFromJson<IEnumerable<IOModel.Airport>>(Path.Combine(this.options.CacheFolderName, this.options.AirportsRawFileName)).ToList();
            this.Airports = Convert<IOModel.Airport, BsModel.Airport>(ioAirports);

            var ioCities = this.serializer.DeserializeFromJson<IEnumerable<IOModel.City>>(Path.Combine(this.options.CacheFolderName, this.options.CitiesCacheFileName)).ToList();
            this.Cities = Convert<IOModel.City, BsModel.City>(ioCities);
            
            var ioCountries = this.serializer.DeserializeFromJson<IEnumerable<IOModel.Country>>(Path.Combine(this.options.CacheFolderName, this.options.CountriesCacheFileName)).ToList();
            this.Countries = Convert<IOModel.Country, BsModel.Country>(ioCountries);
            
            var ioAirlines = this.serializer.DeserializeFromJson<IEnumerable<IOModel.Airline>>(Path.Combine(this.options.CacheFolderName, this.options.AirlinesCacheFileName)).ToList();
            this.Airlines = Convert<IOModel.Airline, BsModel.Airline>(ioAirlines);

            var ioFlights = this.serializer.DeserializeFromJson<IEnumerable<IOModel.Flight>>(Path.Combine(this.options.CacheFolderName, this.options.FlightsCacheFileName)).ToList();
            this.Flights = Convert<IOModel.Flight, BsModel.Flight>(ioFlights);
        }

        private IEnumerable<TARGET_TYPE> Convert<SOURCE_TYPE, TARGET_TYPE>(IEnumerable<SOURCE_TYPE> sourceCollection)
        {
            var mapper = new Mapper(mapperConfiguration);
            List<TARGET_TYPE> results = new List<TARGET_TYPE>();

            foreach (var item in sourceCollection)
            {
                var mappedItem = mapper.Map<TARGET_TYPE>(item);
                results.Add(mappedItem);
            }

            return results;
        }

        private void ParseRawDataFiles()
        {
            List<IOModel.AirportsParseResult> airportsParseResult = this.csvHelper.Parse<IOModel.AirportsParseResult>(Path.Combine(this.options.RawFolderName, this.options.AirportsRawFileName));
            var airportsConversionResult = this.airportsDataConverter.ConvertToModel(airportsParseResult);

            var ioAirports = airportsConversionResult.Airports;
            this.Airports = Convert<IOModel.Airport, BsModel.Airport>(ioAirports);

            var ioCities = airportsConversionResult.Cities;
            this.Cities = Convert<IOModel.City, BsModel.City>(ioCities);

            var ioCountries = airportsConversionResult.Countries;
            this.Countries = Convert<IOModel.Country, BsModel.Country>(ioCountries);

            var ioAirlines = this.csvHelper.Parse<IOModel.Airline>(Path.Combine(this.options.RawFolderName, this.options.AirlinesRawFileName));
            this.Airlines = Convert<IOModel.Airline, BsModel.Airline>(ioAirlines);

            var ioFlights = this.csvHelper.Parse<IOModel.Flight>(Path.Combine(this.options.RawFolderName, this.options.FlightsRawFileName));
            this.Flights = Convert<IOModel.Flight, BsModel.Flight>(ioFlights);
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
