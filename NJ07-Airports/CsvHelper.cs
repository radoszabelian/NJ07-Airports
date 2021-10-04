using NJ07_Airports.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        private static int NextCountryId = 0;
        private static int NextCityId = 0;

        public static List<T> Parse<T>(string filePath) where T : new()
        {
            string[] inputRows = System.IO.File.ReadAllLines(filePath);
            List<ColumnHeaderInfo> FileHeaderInfos = null;

            List<T> deserializedObjects = new List<T>();

            int actualLineNumber = 0;

            foreach (var line in inputRows)
            {
                if (actualLineNumber == 0) FileHeaderInfos = ParseHeader<T>(line);
                else deserializedObjects.Add(ParseDataRow<T>(line, FileHeaderInfos));

                actualLineNumber++;
            }

            return deserializedObjects;
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

        public static void ParseAirportFileAndFillLists(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            string[] lines = System.IO.File.ReadAllLines(@"data\airports.dat");
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
