namespace Aiports_Model
{
    public class Segment
    {
        public int Id { get; set; }

        public int AirlineId { get; set; }

        public int ArrivalAirportId { get; set; }

        public int DepartureAirportId { get; set; }
    }
}
