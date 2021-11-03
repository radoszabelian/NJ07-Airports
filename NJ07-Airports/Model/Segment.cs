namespace NJ07_Airports.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Segment
    {
        public int AirlineId { get; set; }

        public int ArrivalAirportId { get; set; }

        public int DepartureAirportId { get; set; }

        public int Id { get; set; }
    }
}
