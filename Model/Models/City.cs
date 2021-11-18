namespace Aiports_Model
{
    using System;

    public class City
    {
        public int CountryId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string TimeZoneName { get; set; }

        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
