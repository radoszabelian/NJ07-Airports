namespace NJ07_Airports.Services.CsvHelper
{
    using System.Collections.Generic;
    using NJ07_Airports.Services.CsvHelper.Models;

    public interface IAirportsDataConverter
    {
        public AirportsExtractedBundle ConvertToModel(List<AirportsParseResult> airportsParseResult);
    }
}