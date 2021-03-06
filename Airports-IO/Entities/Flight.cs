namespace Airports_IO.Entities
{
    public class Flight
    {
        public string ArrivalTime { get; set; }

        public string DepartureTime { get; set; }

        public int Id { get; set; }

        public string Number { get; set; }

        public int? SegmentId { get; set; }

        public Segment Segment { get; set; }
    }
}
