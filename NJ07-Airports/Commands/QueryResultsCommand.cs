namespace Airports_Logic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Aiports_Model;
    using Airports_CLI;

    public class QueryResultsCommand : ICommand
    {
        private IDataAccessor dataAccessor;

        public QueryResultsCommand(IDataAccessor dataAccessor)
        {
            this.dataAccessor = dataAccessor;
        }

        public void Start()
        {
            var airports = this.dataAccessor.Airports;
            var cities = this.dataAccessor.Cities;
            var countries = this.dataAccessor.Countries;

            Console.WriteLine($"Total entries: airports: {airports.Count()}, cities: {cities.Count()}, countries: {countries.Count()}");

            this.ShowTaskA(airports, cities, countries);

            this.ShowTaskB(airports, cities, countries);

            this.ShowTaskC(airports, cities, countries);

            this.ShowTaskD(airports, cities, countries);

            this.ShowTaskE(airports, cities, countries);
        }

        public string GetDescription()
        {
            return "Shows the results of the queries on the input data.";
        }

        private void ShowTaskE(IEnumerable<Airport> airports, IEnumerable<City> cities, IEnumerable<Country> countries)
        {
            string pattern = @"[euioa]";
            Regex regex = new Regex(pattern);

            var query = from airport in airports
                        select new { airport.Name, Vowels = regex.Matches(airport.Name).Count() };

            var result = query.OrderByDescending(a => a.Vowels).First();

            Console.WriteLine($"The airport {result.Name} has the most vowels with {result.Vowels}");
        }

        private void ShowTaskD(IEnumerable<Airport> airports, IEnumerable<City> cities, IEnumerable<Country> countries)
        {
            var query = from city in cities
                        join airport in airports on city.Id equals airport.CityId
                        select new { CityName = city.Name, AirportName = airport.Name };

            var longestAirportNameCity = query.OrderByDescending(c => c.AirportName.Length).First();
            Console.WriteLine($"The city with the longest airport name is: {longestAirportNameCity.CityName}. Airport name is: {longestAirportNameCity.AirportName}");
        }

        private void ShowTaskC(IEnumerable<Airport> airports, IEnumerable<City> cities, IEnumerable<Country> countries)
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

        private void ShowTaskB(IEnumerable<Airport> airports, IEnumerable<City> cities, IEnumerable<Country> countries)
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

        private void ShowTaskA(IEnumerable<Airport> airports, IEnumerable<City> cities, IEnumerable<Country> countries)
        {
            var countryAirportsQuery = (from airport in airports
                                       join country in countries on airport.CountryId equals country.Id
                                       orderby country.Name ascending
                                       group airport by country.Name).Distinct();

            foreach (var countryAirport in countryAirportsQuery)
            {
                 Console.WriteLine($"--> Country {countryAirport.Key} has {countryAirport.Count()} airports.");
            }
        }
    }
}
