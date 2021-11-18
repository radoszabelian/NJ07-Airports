namespace Airports_Logic.Services.GeoLocation
{
    using Aiports_Model;

    public interface IGeoLocationService
    {
        public Airport GetClosestAirportByGps(string gpsCoordinates);

        public Airport GetAirportByIATA(string iata);
    }
}
