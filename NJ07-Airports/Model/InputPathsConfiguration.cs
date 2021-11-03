namespace NJ07_Airports.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class InputPathsConfiguration
    {
        public string AirportsRawFileName { get; set; }

        public string AirlinesRawFileName { get; set; }

        public string FlightsRawFileName { get; set; }

        public string SegmentsRawFileName { get; set; }

        public string CitiesCacheFileName { get; set; }

        public string CountriesCacheFileName { get; set; }

        public string AirlinesCacheFileName { get; set; }

        public string FlightsCacheFileName { get; set; }

        public string CacheFolderName { get; set; }

        public string RawFolderName { get; set; }
    }
}
