using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airports_Client.ViewModels
{
    public class FlightSearchViewModel
    {
        public bool IsReturnFlight { get; set; }
        public DateTime DepartureDate { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfInfants { get; set; }
    }
}
