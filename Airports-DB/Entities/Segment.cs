namespace Airports_DB.Entities
{
    public class Segment
    {
        public int Id { get; set; }

        public int? AirlineId { get; set; }

        public Airline Airline { get; set; }

        public int? ArrivalAirportId { get; set; }

        public int? DepartureAirportId { get; set; }
    }
}
