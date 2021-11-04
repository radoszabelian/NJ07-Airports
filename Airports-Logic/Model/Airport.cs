namespace Airports_Logic.Model
{
    using System;

    public class Airport
    {
        public int CityId { get; set; }

        public int CountryId { get; set; }

        public string FullName { get; set; }

        public string IATACode { get; set; }

        public string ICAOCode { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string TimeZoneName { get; set; }

        public TimeZoneInfo TimeZoneInfo { get; set; }

        public Location Location { get; set; }
    }
}
