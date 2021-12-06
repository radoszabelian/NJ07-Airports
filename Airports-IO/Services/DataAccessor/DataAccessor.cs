namespace Airports_IO.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using BsModel = Aiports_Model;
    using IOModel = Airports_IO.Model;
    using Airports_Settings.Model;
    using AutoMapper;
    using Airports_Settings.Services;
    using System;
    using Microsoft.EntityFrameworkCore;

    public class DataAccessor : BsModel.IDataAccessor
    {
        private IEnumerable<BsModel.City> cities;
        private IEnumerable<BsModel.Country> countries;
        private IEnumerable<BsModel.Airport> airports;
        private IEnumerable<BsModel.Airline> airlines;
        private IEnumerable<BsModel.Flight> flights;

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

        public IEnumerable<BsModel.City> Cities
        {
            get
            {
                return cities;
            }
        }

        public IEnumerable<BsModel.Country> Countries
        {
            get
            {
                return countries;
            }
        }

        public IEnumerable<BsModel.Airport> Airports
        {
            get
            {
                return airports;
            }
        }

        public IEnumerable<BsModel.Airline> Airlines
        {
            get
            {
                return airlines;
            }
        }

        public IEnumerable<BsModel.Flight> Flights
        {
            get
            {
                return flights;
            }
        }

        private void InitializeAppData()
        {
            CreateAutoMapperMappings();

            if (!this.IsCacheAvailable())
            {
                this.ParseRawDataFiles();
                this.SaveDataToCache();
            }

            if (!this.IsDbAvailable())
            {
                this.SeedDbFromCache();
            }

            this.LoadDataFromDb();
        }

        private bool IsDbAvailable()
        {
            using (var context = new AirportsContext())
            {
                return context.Airports.Any();
            }
        }

        private void LoadDataFromDb()
        {
            using (var context = new AirportsContext())
            {
                var airportsEntities = context.Airports
                    .Include(a => a.Country)
                    .Include(a => a.City)
                    .ToList();

                this.airports = Convert<Entities.Airport, BsModel.Airport>(airportsEntities);
                var index = 0;
                foreach (var airport in airports)
                {
                    airport.Location = new BsModel.Location()
                    {
                        Altitude = airportsEntities[index].Altitude,
                        Longitude = airportsEntities[index].Latitude,
                        Latitude = airportsEntities[index].Latitude
                    };
                    index++;
                }
                //int index = 0;
                //foreach (var airport in airports)
                //{
                //    try
                //    {
                //        airport.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(airportsEntities[index]?.TimeZoneInfoId) ?? null;
                //    }
                //    catch (Exception ex)
                //    {
                //        airport.TimeZoneInfo = null;
                //    }
                //    index++;
                //}

                var citiesEntities = context.Cities
                    .Include(c => c.Country)
                    .ToList();

                this.cities = Convert<Entities.City, BsModel.City>(citiesEntities);

                //index = 0;
                //foreach (var city in cities)
                //{
                //    try
                //    {
                //        city.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(citiesEntities[index]?.TimeZoneInfoId) ?? null;
                //    }
                //    catch (Exception ex)
                //    {
                //        city.TimeZoneInfo = null;
                //    }
                //    index++;
                //}

                var countriesEntities = context.Countries.ToList();
                this.countries = Convert<Entities.Country, BsModel.Country>(countriesEntities);

                var airlineEntities = context.Airlines.ToList();
                this.airlines = Convert<Entities.Airline, BsModel.Airline>(airlineEntities);

                var flightsEntities = context.Flights.Include(f => f.Segment).ToList();
                this.flights = Convert<Entities.Flight, BsModel.Flight>(flightsEntities);
            }
        }

        private void SeedDbFromCache()
        {
            var ioAirports = this.serializer.DeserializeFromJson<IEnumerable<IOModel.Airport>>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName, this.options.AirportsRawFileName)).ToList();
            var ioCities = this.serializer.DeserializeFromJson<IEnumerable<IOModel.City>>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName, this.options.CitiesCacheFileName)).ToList();
            var ioCountries = this.serializer.DeserializeFromJson<IEnumerable<IOModel.Country>>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName, this.options.CountriesCacheFileName)).ToList();
            var ioAirlines = this.serializer.DeserializeFromJson<IEnumerable<IOModel.Airline>>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName, this.options.AirlinesCacheFileName)).ToList();
            var ioFlights = this.serializer.DeserializeFromJson<IEnumerable<IOModel.Flight>>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName, this.options.FlightsCacheFileName)).ToList();

            var countriesEntities = Convert<IOModel.Country, Entities.Country>(ioCountries);
            var airlinesEntities = Convert<IOModel.Airline, Entities.Airline>(ioAirlines);
            var flightsEntities = Convert<IOModel.Flight, Entities.Flight>(ioFlights);
            var airportEntities = Convert<IOModel.Airport, Entities.Airport>(ioAirports);
            var citiesEntities = Convert<IOModel.City, Entities.City>(ioCities);

            int index = 0;
            foreach (var airportEntity in airportEntities)
            {
                airportEntity.TimeZoneInfoId = ioAirports[index]?.TimeZoneInfo?.Id ?? "";
                airportEntity.Latitude = ioAirports[index].Location.Latitude;
                airportEntity.Altitude = ioAirports[index].Location.Altitude;
                airportEntity.Longitude = ioAirports[index].Location.Longitude;
                index++;
            }

            index = 0;
            foreach (var cityEntity in citiesEntities)
            {
                cityEntity.TimeZoneInfoId = ioCities[index]?.TimeZoneInfo?.Id ?? "";
                index++;
            }

            using (var context = new AirportsContext())
            {
                context.Airlines.AddRange(airlinesEntities);
                context.SaveChanges();
            }
            using (var context = new AirportsContext())
            {
                context.Countries.AddRange(countriesEntities);
                context.SaveChanges();
            }
            using (var context = new AirportsContext())
            {
                context.Cities.AddRange(citiesEntities);
                context.SaveChanges();
            }
            using (var context = new AirportsContext())
            {
                context.Airports.AddRange(airportEntities);
                context.SaveChanges();
            }
            //using (var context = new AirportsContext())
            //{
            //    context.Segments.AddRange(segmentsEntities);
            //    context.SaveChanges();
            //}
            //using (var context = new AirportsContext())
            //{
            //    context.Flights.AddRange(flightsEntities);
            //    context.SaveChanges();
            //}
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
                cfg.CreateMap<Entities.Airline, BsModel.Airline>();
                cfg.CreateMap<Entities.Airport, BsModel.Airport>();
                cfg.CreateMap<Entities.City, BsModel.City>();
                cfg.CreateMap<Entities.Country, BsModel.Country>();
                cfg.CreateMap<Entities.Flight, BsModel.Flight>();
                cfg.CreateMap<Entities.Segment, BsModel.Segment>();
                cfg.CreateMap<IOModel.Airline, Entities.Airline>();
                cfg.CreateMap<IOModel.Airport, Entities.Airport>();
                cfg.CreateMap<IOModel.City, Entities.City>();
                cfg.CreateMap<IOModel.Country, Entities.Country>();
                cfg.CreateMap<IOModel.Flight, Entities.Flight>();
                cfg.CreateMap<IOModel.Segment, Entities.Segment>();
            });
        }

        private bool IsCacheAvailable()
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;

            return File.Exists(Path.Combine(rootPath, this.options.CacheFolderName, this.options.CountriesCacheFileName))
                    && File.Exists(Path.Combine(rootPath, this.options.CacheFolderName, this.options.AirportsRawFileName))
                    && File.Exists(Path.Combine(rootPath, this.options.CacheFolderName, this.options.CitiesCacheFileName))
                    && File.Exists(Path.Combine(rootPath, this.options.CacheFolderName, this.options.AirlinesCacheFileName))
                    && File.Exists(Path.Combine(rootPath, this.options.CacheFolderName, this.options.FlightsCacheFileName));
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
            List<IOModel.AirportsParseResult> airportsParseResult = this.csvHelper.Parse<IOModel.AirportsParseResult>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.RawFolderName, this.options.AirportsRawFileName));
            var airportsConversionResult = this.airportsDataConverter.ConvertToModel(airportsParseResult);

            var ioAirports = airportsConversionResult.Airports;
            this.airports = Convert<IOModel.Airport, BsModel.Airport>(ioAirports);

            var ioCities = airportsConversionResult.Cities;
            this.cities = Convert<IOModel.City, BsModel.City>(ioCities);

            var ioCountries = airportsConversionResult.Countries;
            this.countries = Convert<IOModel.Country, BsModel.Country>(ioCountries);

            var ioAirlines = this.csvHelper.Parse<IOModel.Airline>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.RawFolderName, this.options.AirlinesRawFileName));
            this.airlines = Convert<IOModel.Airline, BsModel.Airline>(ioAirlines);

            var ioFlights = this.csvHelper.Parse<IOModel.Flight>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.RawFolderName, this.options.FlightsRawFileName));
            this.flights = Convert<IOModel.Flight, BsModel.Flight>(ioFlights);
        }

        private void SaveDataToCache()
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName)))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName));
            }

            this.serializer.SerializeToJson(this.Airports, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName, this.options.AirportsRawFileName));
            this.serializer.SerializeToJson(this.Cities, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName, this.options.CitiesCacheFileName));
            this.serializer.SerializeToJson(this.Countries, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName, this.options.CountriesCacheFileName));
            this.serializer.SerializeToJson(this.Flights, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName, this.options.FlightsCacheFileName));
            this.serializer.SerializeToJson(this.Airlines, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.options.CacheFolderName, this.options.AirlinesCacheFileName));
        }
    }
}
