namespace Airports_Logic.Services
{
    using System.Collections.Generic;
    using Airports_IO.Models;
    using Airports_Logic.Model;

    public interface IAirportsDataConverter
    {
        public AirportsExtractedBundle ConvertToModel(List<AirportsParseResult> airportsParseResult);
    }
}
