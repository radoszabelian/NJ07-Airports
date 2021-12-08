namespace Airports_Logic.Services
{
    using System.Collections.Generic;
    using Aiports_Model;

    public interface IAirportsService
    {
        IEnumerable<Airport> GetAllAirports();
    }
}
