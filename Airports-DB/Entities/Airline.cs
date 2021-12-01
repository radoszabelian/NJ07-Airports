
namespace Airports_DB.Entities
{
    public class Airline
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IATACode { get; set; }

        public string ICAOCode { get; set; }

        public string CallSign { get; set; }
    }
}
