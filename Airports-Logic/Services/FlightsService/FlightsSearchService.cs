namespace Airports_Logic.Services.FlightsService
{
    using Aiports_Model;
    using Airports_IO.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FlightsSearchService : IFlightService
    {
        private IDataAccessor _dataAccessor;

        public FlightsSearchService(IDataAccessor dataAccessor)
        {
            _dataAccessor = dataAccessor;
        }

        public IEnumerable<FlightsSearchResult> SearchFlights(FlightSearchArguments searchArguments)
        {
            var result =
                from flight in this._dataAccessor.Flights
                join segment in this._dataAccessor.Segments on flight.SegmentId equals segment.Id
                join airline in this._dataAccessor.Airlines on segment.AirlineId equals airline.Id
                join departureAirport in this._dataAccessor.Airports on segment.DepartureAirportId equals departureAirport.Id
                join arrivalAirport in this._dataAccessor.Airports on segment.ArrivalAirportId equals arrivalAirport.Id
                where flight.DepartureTime >= searchArguments.Departure &&
                      departureAirport.Id == searchArguments.DepartureAirportId &&
                      arrivalAirport.Id == searchArguments.ArrivalAirportId
                select new FlightsSearchResult
                {
                    ArrivalAirport = arrivalAirport,
                    DepartureAirport = departureAirport,
                    Flight = flight,
                };

            if (searchArguments.IsReturnFlight && searchArguments.Arrival != null)
            {
                result = result.Where(r => r.Flight.ArrivalTime >= searchArguments.Arrival);
            }

            return result;
        }
    }
}
