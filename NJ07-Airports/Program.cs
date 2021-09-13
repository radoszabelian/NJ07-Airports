using NJ07_Airports.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NJ07_Airports
{
    class Program
    {
        static int NextCountryId = 0;
        static int NextCityId = 0;

        static void Main(string[] args)
        {
            List<Airport> airports = new List<Airport>();
            List<City> cities = new List<City>();
            List<Country> countries = new List<Country>();

            //ParseDataFileAndFillLists(airports, cities, countries);
            var airlines = CsvHelper.Parse<Airline>(@"data/airlines.dat");
            var flights = CsvHelper.Parse<Flight>(@"data/flights.dat");
            
            Console.WriteLine(airlines.Count());
            Console.WriteLine(flights.Count());


            //ShowResults(airports, cities, countries);
        }

        private static void ShowResults(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            Console.WriteLine($"Parsing complete. Total entries: airports: {airports.Count()}, cities: {cities.Count()}, countries: {countries.Count()}");

            ShowTaskA(airports, cities, countries);

            ShowTaskB(airports, cities, countries);

            ShowTaskC(airports, cities, countries);

            ShowTaskD(airports, cities, countries);

            ShowTaskE(airports, cities, countries);
        }

        private static void ShowTaskE(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            string pattern = @"[euioa]";
            Regex regex = new Regex(pattern);

            var query = from airport in airports
                        select new { airport.Name, Vowels = regex.Matches(airport.Name).Count() };

            var result = query.OrderByDescending(a => a.Vowels).First();

            Console.WriteLine($"The airport {result.Name} has the most vowels with {result.Vowels}");
        }

        private static void ShowTaskD(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            var query = from city in cities
                        join airport in airports on city.Id equals airport.CityId
                        select new { CityName = city.Name, AirportName = airport.Name };

            var longestAirportNameCity = query.OrderByDescending(c => c.AirportName.Length).First();
            Console.WriteLine($"The city with the longest airport name is: {longestAirportNameCity.CityName}. Airport name is: {longestAirportNameCity.AirportName}");
        }

        private static void ShowTaskC(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            var query = from country in countries
                        orderby country.Name descending
                        join airport in airports on country.Id equals airport.CountryId
                        group airport by country.Name;

            foreach (var countryAirports in query)
            {
                Console.WriteLine($"-- {countryAirports.Key} --");
                foreach (var airport in countryAirports)
                {
                    Console.WriteLine($"\t{airport.Name}");
                }
            }
        }

        private static void ShowTaskB(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            var citiesAndAirports = from airport in airports
                                    join city in cities on airport.CityId equals city.Id
                                    group airport by city.Name into cityairport
                                    orderby cityairport.Count() descending
                                    select new { Name = cityairport.Key, AirportsCount = cityairport.Count() };
            int maxCount = citiesAndAirports.First().AirportsCount;

            citiesAndAirports = citiesAndAirports.TakeWhile(ca => ca.AirportsCount == maxCount);

            foreach (var cityAirportItem in citiesAndAirports)
            {
                Console.WriteLine($"{cityAirportItem.Name} has {cityAirportItem.AirportsCount} airports.");
            }
        }

        private static void ShowTaskA(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            var CountryAirportsQuery = from airport in airports
                                       join country in countries on airport.CountryId equals country.Id
                                       orderby country.Name ascending
                                       group airport by country.Name;

            foreach (var countryAirport in CountryAirportsQuery)
            {

                foreach (var item in countryAirport)
                {
                }
                Console.WriteLine($"--> Country {countryAirport.Key} has {countryAirport.Count()} airports.");
            }
        }

        private static void ParseDataFileAndFillLists(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            string[] lines = System.IO.File.ReadAllLines(@"airports.dat");
            string validationPattern = @"^\d+,(""[a-zA-Z ]*"",){5}(-?[0-9.]+,){4}""[A-Z]""$";

            Regex regex = new Regex(validationPattern);

            foreach (var line in lines)
            {
                if (regex.IsMatch(line))
                {
                    string[] lineProperties = line.Split(",");

                    for (int i = 0; i < lineProperties.Length; i++)
                    {
                        lineProperties[i] = lineProperties[i].Replace("\"", "");
                    }

                    //create country if needed
                    AddOrGetCountry(countryName: lineProperties[3], countries);

                    //create city if needed
                    AddOrGetCity(cityName: lineProperties[2], countryName: lineProperties[3], cities, countries);

                    AddAirport(lineProperties, cities, countries, airports);
                }
            }
        }

        private static void AddAirport(string[] lineProperties, List<City> cities, List<Country> countries, List<Airport> airports)
        {
            string id = lineProperties[0];
            string name = lineProperties[1];
            string cityName = lineProperties[2];
            string countryName = lineProperties[3];
            string iataCode = lineProperties[4];
            string icaoCode = lineProperties[5];
            string gpsLongitude = lineProperties[6];
            string gpsLatitude = lineProperties[7];
            string gpsAltitude = lineProperties[8];

            City relatedCity = cities.First(c => c.Name == cityName);
            Country relatedCountry = countries.First(c => c.Id == relatedCity.CountryId);

            Airport newAirport = new Airport()
            {
                Id = Convert.ToInt32(id),
                CityId = relatedCity.Id,
                CountryId = relatedCountry.Id,
                Name = name,
                FullName = $"{name} Airport",
                IATACode = iataCode,
                ICAOCode = icaoCode,
                Location = new Location()
                {
                    Altitude = gpsAltitude,
                    Latitude = gpsLatitude,
                    Longitude = gpsLongitude
                },
                TimeZoneName = relatedCity.TimeZoneName
            };

            airports.Add(newAirport);
        }

        private static Country AddOrGetCountry(string countryName, List<Country> countries)
        {
            var searchResult = countries.FirstOrDefault(c => c.Name == countryName);

            if (searchResult == null)
            {
                var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
                RegionInfo currentRegion = regions.FirstOrDefault(region => region.EnglishName.Contains(countryName));

                Country newCountry = new Country()
                {
                    Name = countryName,
                    Id = NextCountryId,
                    ThreeLetterISOCode = currentRegion?.ThreeLetterISORegionName,
                    TwoLetterISOCode = currentRegion?.TwoLetterISORegionName
                };

                NextCountryId++;

                countries.Add(newCountry);

                return newCountry;
            }
            else
            {
                return searchResult;
            }
        }

        private static City AddOrGetCity(string cityName, string countryName, List<City> cities, List<Country> countries)
        {
            City city = cities.FirstOrDefault(c => c.Name == cityName);

            if (city == null)
            {
                var relatedCountry = countries.First(c => c.Name == countryName);

                City newCity = new City()
                {
                    Name = cityName,
                    Id = NextCityId,
                    CountryId = relatedCountry.Id,
                    TimeZoneName = "" //TODO
                };

                cities.Add(newCity);

                NextCityId++;

                return newCity;

            }
            else
            {
                return city;
            }
        }


    }
}
