using NJ07_Airports.Logging;
using NJ07_Airports.Model;
using NJ07_Airports.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NJ07_Airports
{
    public struct ColumnHeaderInfo
    {
        public string ClassPropName;
        public int IndexInFileRow;
        public bool NotEmpty;
    }

    public static class CsvHelper
    {
        //private static int s_nextCountryId = 0;
        //TODO ezeket majd metódusokba kitenni
        private static int s_nextCityId = 1;
        private static int s_ignoredRows = 0;
        private static int s_nextCountryId = 1;

        // ----------------------------- //
        // GENERIC PARSING FUNCTIONALITY //
        // ----------------------------- //

        public static List<T> Parse<T>(string filePath) where T : new()
        {
            string[] inputRows = System.IO.File.ReadAllLines(filePath);
            List<ColumnHeaderInfo> FileHeaderInfos = null;

            List<T> parsedObjects = new List<T>();

            int actualLineNumber = 0;

            foreach (var line in inputRows)
            {
                if (actualLineNumber == 0) FileHeaderInfos = ParseHeader<T>(line);
                else parsedObjects.Add(ParseDataRow<T>(line, FileHeaderInfos));

                actualLineNumber++;
            }

            return parsedObjects;
        }

        private static List<ColumnHeaderInfo> ParseHeader<T>(string headerRow) where T : new()
        {
            List<ColumnHeaderInfo> ColumnHeaderInfoList = new List<ColumnHeaderInfo>();
            var splittedEHeaderRow = headerRow.Split(',');

            var PropetiesOfObjectToBeCrafted = typeof(T).GetProperties();

            foreach (var prop in PropetiesOfObjectToBeCrafted)
            {
                var columnAttribute = prop.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "Column");
                var notEmptyAttribute = prop.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "NotEmpty");

                string propNameInFile = "";
                if (columnAttribute?.ConstructorArguments.Count > 0)
                {
                    propNameInFile =
                        columnAttribute?.ConstructorArguments[0].Value.ToString().ToLower();
                }
                else
                {
                    propNameInFile = prop.Name.ToLower();
                }

                ColumnHeaderInfoList.Add(new ColumnHeaderInfo()
                {
                    ClassPropName = prop.Name,
                    NotEmpty = notEmptyAttribute == null,
                    IndexInFileRow = Array.FindIndex(splittedEHeaderRow, w => w.ToLower() == propNameInFile)
                });
            }

            return ColumnHeaderInfoList;
        }

        private static T ParseDataRow<T>(string line, List<ColumnHeaderInfo> columnHeaderInfos) where T : new()
        {
            T DeserializedObject = new T();
            PropertyInfo[] DeserializedObjectProperties = DeserializedObject.GetType().GetProperties();
            var splittedLine = line.Split(',');

            foreach (var DeserializedObjectProperty in DeserializedObjectProperties)
            {
                var ColumnHeaderInfo = columnHeaderInfos.Where(infoObj => infoObj.ClassPropName == DeserializedObjectProperty.Name).Cast<ColumnHeaderInfo?>().FirstOrDefault();

                if (ColumnHeaderInfo.HasValue && ColumnHeaderInfo.Value.IndexInFileRow >= 0)
                {
                    var ColumnStringValue = splittedLine[ColumnHeaderInfo.Value.IndexInFileRow];

                    if (ColumnHeaderInfo.Value.NotEmpty && string.IsNullOrEmpty(ColumnStringValue))
                    {
                        continue;
                    }

                    DeserializedObjectProperty.SetValue(DeserializedObject,
                        Convert.ChangeType(ColumnStringValue, DeserializedObjectProperty.PropertyType));
                }
            }

            return DeserializedObject;
        }

        // ------------------------ //
        // PARSING THE AIRPORT FILE //
        // ------------------------ //

        /* Airports, Cities and countries are available in a single input file, so they have to be processed at once. */
        public static ParsedAirportsDataBundle ParseAirportFile(string inputFilePath, ILogger logger)
        {
            List<Airport> airports = new List<Airport>();
            List<City> cities = new List<City>();
            List<Country> countries = new List<Country>();

            string[] rows = System.IO.File.ReadAllLines(inputFilePath);

            string validationPattern = @"^\d+,(""[a-zA-Z ]*"",){5}(-?[0-9.]+,){4}""[A-Z]""$";
            Regex regex = new Regex(validationPattern);

            foreach (var row in rows)
            {
                if (regex.IsMatch(row))
                {
                    CraftNewCountryObjectIfNotExistsAndThenAppendToCountriesList(row, countries);
                    CraftNewCityObjectIfNotExistsAndThenAppendToCitiesList(row, cities, countries);
                    CraftAirportObjectAndThenAppendToAirportsList(row, cities, countries, airports);
                }
                else
                {
                    logger.LogError(new Exception("$!!! The line { row } is malformed so it is ignored. !!!"));
                    s_ignoredRows++;
                }
            }

            PopulateTimeZoneDataOfAllLists(cities, airports);

            //logger.Log($"There were a total of {s_ignoredRows} rows ignored.");

            var result = new ParsedAirportsDataBundle()
            {
                Airports = airports,
                Cities = cities,
                Countries = countries
            };

            return result;
        }

        private static void CraftNewCountryObjectIfNotExistsAndThenAppendToCountriesList(string row, List<Country> countries)
        {
            string[] rowColumns = row.Split(",");
            rowColumns = rowColumns.Select(rc => rc.Replace("\"", "")).ToArray();
            string countryName = rowColumns[3];

            if (countries.FirstOrDefault(c => c.Name == countryName) == null)
            {
                var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
                RegionInfo currentRegion = regions.FirstOrDefault(region => region.EnglishName.Contains(countryName));

                var newCountry = new Country()
                {
                    Name = countryName,
                    Id = s_nextCountryId,
                    ThreeLetterISOCode = currentRegion?.ThreeLetterISORegionName,
                    TwoLetterISOCode = currentRegion?.TwoLetterISORegionName
                };

                countries.Add(newCountry);
                s_nextCountryId++;
            }
        }

        private static void CraftNewCityObjectIfNotExistsAndThenAppendToCitiesList(string row, List<City> cities, List<Country> countries)
        {
            string[] rowColumns = row.Split(',');

            string cityName = RemoveSpecChars(rowColumns[2]);
            string countryName = RemoveSpecChars(rowColumns[3]);

            if (cities.FirstOrDefault(c => c.Name == cityName) == null)
            {
                var relatedCountry = countries.First(c => c.Name == countryName);

                City newCity = new City()
                {
                    Name = cityName,
                    Id = s_nextCityId,
                    CountryId = relatedCountry.Id,
                    TimeZoneName = "",
                    TimeZoneInfo = null
                };

                cities.Add(newCity);

                s_nextCityId++;
            }
        }

        private static string RemoveSpecChars(string input)
        {
            input = input.Replace("\\", "");
            return input.Replace("\"", "");
        }

        private static void CraftAirportObjectAndThenAppendToAirportsList(string row, List<City> cities, List<Country> countries, List<Airport> airports)
        {
            string[] rowColumns = row.Split(',');

            string id = RemoveSpecChars(rowColumns[0]);
            string name = RemoveSpecChars(rowColumns[1]);
            string cityName = RemoveSpecChars(rowColumns[2]);
            string countryName = RemoveSpecChars(rowColumns[3]);
            string iataCode = RemoveSpecChars(rowColumns[4]);
            string icaoCode = RemoveSpecChars(rowColumns[5]);
            string gpsLatitude = RemoveSpecChars(rowColumns[6]);
            string gpsLongitude = RemoveSpecChars(rowColumns[7]);
            string gpsAltitude = RemoveSpecChars(rowColumns[8]);

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

        private static void PopulateTimeZoneDataOfAllLists(List<City> cities, List<Airport> airports)
        {
            string inputRawJsonData = System.IO.File.ReadAllText(@"data/timezoneinfo.json");

            IEnumerable<TimeZoneInputData> parsedRows = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TimeZoneInputData>>(inputRawJsonData);

            foreach (var timeZoneData in parsedRows)
            {
                TimeZoneInfo tzObject = TimeZoneInfo.FindSystemTimeZoneById(timeZoneData.TimeZoneInfoId);

                var airport = airports.FirstOrDefault(ai => ai.Id == timeZoneData.AirportId);
                if (airport != null) {
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
