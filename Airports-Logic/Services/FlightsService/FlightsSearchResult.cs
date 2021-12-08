namespace Airports_Logic.Services.FlightsService
{
    using Aiports_Model;

    public class FlightsSearchResult
    {
        public Flight Flight { get; set; }

        public Airport DepartureAirport { get; set; }

        public Airport? ArrivalAirport { get; set; }
    }
}
