namespace Airports_Logic.Model
{
    using Airports_Logic.Attributes;

    public class Airline
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Column("iata")]
        [NotEmpty]
        public string IATACode { get; set; }

        [Column("icao")]
        [NotEmpty]
        public string ICAOCode { get; set; }

        [Column("callsign")]
        public string CallSign { get; set; }
    }
}
