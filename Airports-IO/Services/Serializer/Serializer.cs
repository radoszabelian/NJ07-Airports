namespace Airports_IO.Services
{
    using Newtonsoft.Json;

    public class Serializer : ISerializer
    {
        public void SerializeToJson<T>(T objectsToSerialize, string outputFilePath)
        {
            var outputString = JsonConvert.SerializeObject(objectsToSerialize);
            System.IO.File.WriteAllText(outputFilePath, outputString);
        }

        public T DeserializeFromJson<T>(string inputFilePath)
        {
            var rawText = System.IO.File.ReadAllText(inputFilePath);

            return JsonConvert.DeserializeObject<T>(rawText);
        }
    }
}
