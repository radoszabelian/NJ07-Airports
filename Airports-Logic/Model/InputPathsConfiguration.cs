﻿namespace Airports_Logic.Model
{
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