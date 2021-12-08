namespace Airports_Logic.Services.FlightsService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FlightSearchArguments
    {
        public bool IsReturnFlight { get; set; }

        public TimeSpan Departure { get; set; }

        public int DepartureAirportId { get; set; }

        public int? ArrivalAirportId { get; set; }

        public TimeSpan? Arrival { get; set; }

        public int NumberOfAdults { get; set; }

        public int NumberOfChildren { get; set; }

        public int NumberOfInfants { get; set; }
    }
}
