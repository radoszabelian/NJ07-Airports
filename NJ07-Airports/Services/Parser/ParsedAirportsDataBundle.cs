using NJ07_Airports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports.Parser
{
    public class ParsedAirportsDataBundle
    {
        public List<Airport> Airports { get; set; }
        public List<City> Cities { get; set; }
        public List<Country> Countries { get; set; }
    }
}
