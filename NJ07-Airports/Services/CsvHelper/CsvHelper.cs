using NJ07_Airports.Services.CsvHelper;
using NJ07_Airports.Services.CsvHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NJ07_Airports
{
    public class CsvHelper : ICsvHelper
    {
        public List<T> Parse<T>(string filePath) where T : new()
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

        private List<ColumnHeaderInfo> ParseHeader<T>(string headerRow) where T : new()
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

        private T ParseDataRow<T>(string line, List<ColumnHeaderInfo> columnHeaderInfos) where T : new()
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
    }
}
