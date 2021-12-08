using Airports_Logic.Services.FlightsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airports_Client.ViewModels
{
    public class FlightSearchViewModel
    {
        public bool IsReturnFlight { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfInfants { get; set; }
        public IEnumerable<FlightsSearchResult> FlightsSearchResult { get; set; }
    }
}
