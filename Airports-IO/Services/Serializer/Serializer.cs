namespace Airports_IO.Services
{
    using Newtonsoft.Json;
    using System.IO;

    public class Serializer : ISerializer
    {
        private string _rootPath;

        public Serializer(string rootPath)
        {
            _rootPath = rootPath;
        }

        public void SerializeToJson<T>(T objectsToSerialize, string outputFilePath)
        {
            var outputString = JsonConvert.SerializeObject(objectsToSerialize);
            System.IO.File.WriteAllText(Path.Combine(_rootPath, outputFilePath), outputString);
        }

        public T DeserializeFromJson<T>(string inputFilePath)
        {
            var rawText = System.IO.File.ReadAllText(Path.Combine(_rootPath, inputFilePath));

            return JsonConvert.DeserializeObject<T>(rawText);
        }
    }
}
