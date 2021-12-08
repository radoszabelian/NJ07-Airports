namespace Airports_IO.Model
{
    using Airports_IO.Attributes;
    using System;

    public class Flight
    {
        [Column("arrivalTime")]
        public TimeSpan ArrivalTime { get; set; }

        [Column("departureTime")]
        public TimeSpan DepartureTime { get; set; }

        [Column("id")]
        public int Id { get; set; }

        [Column("number")]
        public string Number { get; set; }

        [Column("segmentId")]
        public int SegmentId { get; set; }
    }
}
