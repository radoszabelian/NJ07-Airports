namespace Airports_IO.Services
{
    public interface ISerializer
    {
        public void SerializeToJson<T>(T objectsToSerialize, string outputFilePath);
        public T DeserializeFromJson<T>(string inputFilePath);
    }
}
