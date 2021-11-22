namespace Airports_IO.Entities
{
    using System;

    public class City
    {
        public int? CountryId { get; set; }

        public Country Country { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string TimeZoneName { get; set; }

        public string TimeZoneInfoId { get; set; }
    }
}
