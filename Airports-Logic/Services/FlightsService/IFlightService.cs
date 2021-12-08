namespace Airports_Logic.Services.FlightsService
{
    using System.Collections.Generic;

    public interface IFlightService
    {
        public IEnumerable<FlightsSearchResult> SearchFlights(FlightSearchArguments searchArguments);
    }
}
