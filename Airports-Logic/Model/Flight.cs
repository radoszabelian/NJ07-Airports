namespace Airports_Logic.Model
{
    using Airports_Logic.Attributes;
    public class Flight
    {
        [Column("arrivalTime")]
        public string ArrivalTime { get; set; }

        [Column("departureTime")]
        public string DepartureTime { get; set; }

        [Column("id")]
        public int Id { get; set; }

        [Column("number")]
        public string Number { get; set; }

        [Column("segmentId")]
        public int SegmentId { get; set; }
    }
}
