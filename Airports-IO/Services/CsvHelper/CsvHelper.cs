namespace Airports_IO.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Airports_IO.Model;

    public class CsvHelper : ICsvHelper
    {
        public List<T> Parse<T>(string filePath)
            where T : new()
        {
            string[] inputRows = System.IO.File.ReadAllLines(filePath);
            List<ColumnHeaderInfo> fileHeaderInfos = null;

            List<T> parsedObjects = new List<T>();

            int actualLineNumber = 0;

            foreach (var line in inputRows)
            {
                if (actualLineNumber == 0)
                {
                    fileHeaderInfos = this.ParseHeader<T>(line);
                }
                else
                {
                    parsedObjects.Add(this.ParseDataRow<T>(line, fileHeaderInfos));
                }

                actualLineNumber++;
            }

            return parsedObjects;
        }

        private List<ColumnHeaderInfo> ParseHeader<T>(string headerRow)
            where T : new()
        {
            List<ColumnHeaderInfo> columnHeaderInfoList = new List<ColumnHeaderInfo>();
            var splittedEHeaderRow = headerRow.Split(',');

            var propetiesOfObjectToBeCrafted = typeof(T).GetProperties();

            foreach (var prop in propetiesOfObjectToBeCrafted)
            {
                var columnAttribute = prop.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "Column");
                var notEmptyAttribute = prop.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "NotEmpty");

                string propNameInFile = string.Empty;
                if (columnAttribute?.ConstructorArguments.Count > 0)
                {
                    propNameInFile =
                        columnAttribute?.ConstructorArguments[0].Value.ToString().ToLower();
                }
                else
                {
                    propNameInFile = prop.Name.ToLower();
                }

                columnHeaderInfoList.Add(new ColumnHeaderInfo()
                {
                    ClassPropName = prop.Name,
                    NotEmpty = notEmptyAttribute == null,
                    IndexInFileRow = Array.FindIndex(splittedEHeaderRow, w => w.ToLower() == propNameInFile),
                });
            }

            return columnHeaderInfoList;
        }

        private T ParseDataRow<T>(string line, List<ColumnHeaderInfo> columnHeaderInfos)
            where T : new()
        {
            T deserializedObject = new T();
            PropertyInfo[] deserializedObjectProperties = deserializedObject.GetType().GetProperties();
            var splittedLine = line.Split(',');

            foreach (var deserializedObjectProperty in deserializedObjectProperties)
            {
                var columnHeaderInfo = columnHeaderInfos.Where(infoObj => infoObj.ClassPropName == deserializedObjectProperty.Name).Cast<ColumnHeaderInfo?>().FirstOrDefault();

                if (columnHeaderInfo.HasValue && columnHeaderInfo.Value.IndexInFileRow >= 0)
                {
                    var columnStringValue = splittedLine[columnHeaderInfo.Value.IndexInFileRow];

                    if (columnHeaderInfo.Value.NotEmpty && string.IsNullOrEmpty(columnStringValue))
                    {
                        continue;
                    }

                    deserializedObjectProperty.SetValue(deserializedObject, Convert.ChangeType(columnStringValue, deserializedObjectProperty.PropertyType));
                }
            }

            return deserializedObject;
        }
    }
}
