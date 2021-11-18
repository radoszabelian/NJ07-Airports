namespace Airports_IO.Services
{
    using System.Collections.Generic;
    using Airports_IO.Model;

    public interface IAirportsDataConverter
    {
        public AirportsExtractedBundle ConvertToModel(List<AirportsParseResult> airportsParseResult);
    }
}
