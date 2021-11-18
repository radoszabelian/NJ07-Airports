namespace Airports_Logic.Services
{
    using System;
    using System.Linq;
    using Aiports_Model;
    using Airports_Logic.Services.GeoLocation;
    using Geolocation;

    /// <summary>
    /// This Class serves as an executable feature, it asks for a geo location or iata code and
    /// displays the closest airport to the coordinates.
    /// </summary>
    public class GeoLocationService : IGeoLocationService
    {
        private IDataAccessor dataAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLocationService"/> class.
        /// </summary>
        /// <param name="dataAccessor">DataAccessor object to get the data from some data source.</param>
        public GeoLocationService(IDataAccessor dataAccessor)
        {
            this.dataAccessor = dataAccessor;
        }

        public Airport GetClosestAirportByGps(string gpsCoordinates)
        {
            string[] coordinates = gpsCoordinates.Split(", ");
            Coordinate origin = new Coordinate(Convert.ToDouble(coordinates[0]), Convert.ToDouble(coordinates[1]));

            var closestAirport = this.dataAccessor.Airports.Select(airport => new
            {
                Distance =
                GeoCalculator.GetDistance(origin, new Coordinate(
                    Convert.ToDouble(airport.Location.Latitude),
                    Convert.ToDouble(airport.Location.Longitude))),
                Airport = airport,
            }).OrderBy(a => a.Distance).First();

            return closestAirport.Airport;
        }

        public Airport GetAirportByIATA(string iata)
        {
            var airport = this.dataAccessor.Airports.FirstOrDefault(a => a.IATACode == iata);
            return airport;
        }
    }
}
