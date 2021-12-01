namespace Airports_DB.Entities
{
    public class Airport
    {
        public int Id { get; set; }

        public int? CityId { get; set; }

        public City City { get; set; }

        public int? CountryId { get; set; }

        public Country Country { get; set; }

        public string FullName { get; set; }

        public string IATACode { get; set; }

        public string ICAOCode { get; set; }

        public string Name { get; set; }

        public string TimeZoneName { get; set; }

        public string TimeZoneInfoId { get; set; }

        public int? LocationId { get; set; }

        public Location Location { get; set; }
    }
}
