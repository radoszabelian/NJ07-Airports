namespace Airports_IO.Services
{
    using System.Collections.Generic;

    public interface ICsvHelper
    {
        public List<T> Parse<T>(string filePath)
            where T : new();
    }
}
