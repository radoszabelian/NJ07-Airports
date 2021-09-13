using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports
{
    public struct ParseFieldInfo
    {
        public string ClassPropName;
        public int indexInLine;
        public bool notEmpty;
    }

    public static class CsvHelper
    {
        public static List<T> Parse<T>(string filePath) where T : new()
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            List<T> result = new List<T>();
            List<ParseFieldInfo> parseFieldInfos = new List<ParseFieldInfo>();

            int lineNumber = 0;
            foreach (var line in lines)
            {
                var splittedLine = line.Split(',');

                //header line with column names
                if (lineNumber == 0)
                {
                    parseFieldInfos = GetParseFieldInfo<T>(line);
                } else 
                //normal data line
                {
                    T newItem = new T();

                    var properties = newItem.GetType().GetProperties();

                    foreach (var prop in properties)
                    {
                        var matchedPfi = parseFieldInfos.Where(pfi => pfi.ClassPropName == prop.Name).Cast<ParseFieldInfo?>().FirstOrDefault();

                        if (matchedPfi.HasValue && matchedPfi.Value.indexInLine >= 0)
                        {
                            var dataLinePropValue = splittedLine[matchedPfi.Value.indexInLine];
                            if (matchedPfi.Value.notEmpty && String.IsNullOrEmpty(dataLinePropValue))
                            {
                                continue;
                            }

                            prop.SetValue(newItem, Convert.ChangeType(dataLinePropValue, prop.PropertyType));    
                        }
                    }
                    result.Add(newItem);
                }

                lineNumber++;
            }

            return result;

            //string validationPattern = @"^\d+,(""[a-zA-Z ]*"",){5}(-?[0-9.]+,){4}""[A-Z]""$";

            //Regex regex = new Regex(validationPattern);

            //foreach (var line in lines)
            //{
            //    if (regex.IsMatch(line))
            //    {
            //        string[] lineProperties = line.Split(",");

            //        for (int i = 0; i < lineProperties.Length; i++)
            //        {
            //            lineProperties[i] = lineProperties[i].Replace("\"", "");
            //        }

            //        //create country if needed
            //        AddOrGetCountry(countryName: lineProperties[3], countries);

            //        //create city if needed
            //        AddOrGetCity(cityName: lineProperties[2], countryName: lineProperties[3], cities, countries);

            //        AddAirport(lineProperties, cities, countries, airports);
            //    }
            //}

        }

        private static List<ParseFieldInfo> GetParseFieldInfo<T>(string line) where T: new()
        {
            // hasonlítsd össze a szövegben lévőt a valós propertykkel, ha mindenre van column egyezés akkor oké
            List<ParseFieldInfo> parseFieldInfos = new List<ParseFieldInfo>();

            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                var columnAttribute = prop.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "Column");
                var notEmptyAttribute = prop.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "NotEmpty");

                if (columnAttribute != null)
                {
                    var dataPropName = columnAttribute.ConstructorArguments[0].Value.ToString().ToLower();
                    parseFieldInfos.Add(new ParseFieldInfo()
                    {
                        ClassPropName = prop.Name,
                        notEmpty = notEmptyAttribute == null,
                        indexInLine = Array.FindIndex(line.Split(','), w => w.ToLower() == dataPropName)
                    });
                }
            }

            return parseFieldInfos;
        }
    }
}
