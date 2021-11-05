namespace Airports_Logic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Airports_IO.Models;
    using Airports_Logic.Model;

    public class AirportsDataConverter : IAirportsDataConverter
    {
        private int nextCityId = 0;
        private int nextCountryId = 0;

        public AirportsExtractedBundle ConvertToModel(List<AirportsParseResult> airportsParseResult)
        {
            List<Airport> airports = new List<Airport>();
            List<City> cities = new List<City>();
            List<Country> countries = new List<Country>();

            var resultBundle = new AirportsExtractedBundle()
            {
                Airports = airports,
                Cities = cities,
                Countries = countries,
            };

            if (airportsParseResult == null)
            {
                return resultBundle;
            }

            foreach (var airportsParseResultItem in airportsParseResult)
            {
                var existingCountry = countries.FirstOrDefault(c => c.Name == airportsParseResultItem.CountryName);
                if (existingCountry == null)
                {
                    countries.Add(this.CreateNewCountryObject(airportsParseResultItem));
                }

                var existingCity = cities.FirstOrDefault(c => c.Name == airportsParseResultItem.CityName);
                if (existingCity == null)
                {
                    cities.Add(this.CreateNewCityObject(airportsParseResultItem, countries));
                }

                airports.Add(this.CreateNewAirportObject(airportsParseResultItem, countries, cities));
            }

            this.PopulateTimeZoneDataOfAllLists(cities, airports);

            return resultBundle;
        }

        private Airport CreateNewAirportObject(AirportsParseResult airportsParseResultItem, List<Country> countries, List<City> cities)
        {
            string id = airportsParseResultItem.Id;
            string name = airportsParseResultItem.AirportName;
            string cityName = airportsParseResultItem.CityName;
            string countryName = airportsParseResultItem.CountryName;
            string iataCode = airportsParseResultItem.IATA;
            string icaoCode = airportsParseResultItem.ICAO;
            string gpsLatitude = airportsParseResultItem.Latitude;
            string gpsLongitude = airportsParseResultItem.Longitude;
            string gpsAltitude = airportsParseResultItem.Altitude;

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
                    Longitude = gpsLongitude,
                },
                TimeZoneName = relatedCity.TimeZoneName,
            };

            return newAirport;
        }

        private Country CreateNewCountryObject(AirportsParseResult airportsParseResultItem)
        {
            string countryName = airportsParseResultItem.CountryName;

            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            RegionInfo currentRegion = regions.FirstOrDefault(region => region.EnglishName.Contains(countryName));

            var newCountry = new Country()
            {
                Name = countryName,
                Id = this.nextCountryId,
                ThreeLetterISOCode = currentRegion?.ThreeLetterISORegionName,
                TwoLetterISOCode = currentRegion?.TwoLetterISORegionName,
            };

            this.nextCountryId++;

            return newCountry;
        }

        private City CreateNewCityObject(AirportsParseResult airportsParseResultItem, List<Country> countries)
        {
            string cityName = airportsParseResultItem.CityName;
            string countryName = airportsParseResultItem.CountryName;

            var relatedCountry = countries.First(c => c.Name == countryName);

            City newCity = new City()
            {
                Name = cityName,
                Id = this.nextCityId,
                CountryId = relatedCountry.Id,
                TimeZoneName = string.Empty,
                TimeZoneInfo = null,
            };

            this.nextCityId++;
            return newCity;
        }

        private void PopulateTimeZoneDataOfAllLists(List<City> cities, List<Airport> airports)
        {
            string inputRawJsonData = System.IO.File.ReadAllText(@"data/timezoneinfo.json");

            IEnumerable<TimeZoneInputData> parsedRows = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TimeZoneInputData>>(inputRawJsonData);

            foreach (var timeZoneData in parsedRows)
            {
                TimeZoneInfo tzObject = TimeZoneInfo.FindSystemTimeZoneById(timeZoneData.TimeZoneInfoId);

                var airport = airports.FirstOrDefault(ai => ai.Id == timeZoneData.AirportId);
                if (airport != null)
                {
                    airport.TimeZoneName = tzObject.StandardName;
                    airport.TimeZoneInfo = tzObject;

                    var relatedCity = cities.First(c => c.Id == airport.CityId);
                    if (relatedCity.TimeZoneInfo == null)
                    {
                        relatedCity.TimeZoneInfo = tzObject;
                        relatedCity.TimeZoneName = tzObject.StandardName;
                    }
                }
            }
        }
    }
}
