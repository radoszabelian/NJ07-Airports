namespace NJ07_Airports.SerializeToJson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class Serializer
    {
        public static void SerializeToJson<T>(T objectsToSerialize, string outputFilePath)
        {
            var outputString = JsonConvert.SerializeObject(objectsToSerialize);
            System.IO.File.WriteAllText(outputFilePath, outputString);
        }

        public static T DeserializeFromJson<T>(string inputFilePath)
        {
            var rawText = System.IO.File.ReadAllText(inputFilePath);

            return JsonConvert.DeserializeObject<T>(rawText);
        }
    }
}
