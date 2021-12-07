using Airports_IO.Attributes;

namespace Airports_IO.Model
{
    public class Segment
    {
        public int Id { get; set; }

        [Column("airline")]
        [NotEmpty]
        public int AirlineId { get; set; }

        [Column("departureAirport")]
        [NotEmpty]
        public int DepartureAirportId { get; set; }

        [Column("arrivalAirport")]
        [NotEmpty]
        public int ArrivalAirportId { get; set; }
    }
}
