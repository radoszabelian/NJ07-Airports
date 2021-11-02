using NJ07_Airports.Services.CsvHelper.Models;
using System.Collections.Generic;

namespace NJ07_Airports.Services.CsvHelper
{
    public interface IAirportsDataConverter
    {
        public AirportsExtractedBundle ConvertToModel(List<AirportsParseResult> airportsParseResult);
    }
}