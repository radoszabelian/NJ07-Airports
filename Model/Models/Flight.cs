using System;

namespace Aiports_Model
{
    public class Flight
    {
        public TimeSpan ArrivalTime { get; set; }

        public TimeSpan DepartureTime { get; set; }

        public int Id { get; set; }

        public string Number { get; set; }

        public int SegmentId { get; set; }
    }
}
