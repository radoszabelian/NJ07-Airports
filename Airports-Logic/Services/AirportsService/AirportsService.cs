namespace Airports_Logic.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Aiports_Model;

    public class AirportsService : IAirportsService
    {
        protected IDataAccessor _dataAccessor;

        public AirportsService(IDataAccessor dataAccessor)
        {
            _dataAccessor = dataAccessor;
        }

        public IEnumerable<Airport> GetAllAirports()
        {
            return _dataAccessor.Airports.ToList();
        }
    }
}
