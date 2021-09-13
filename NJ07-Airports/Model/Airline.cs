using NJ07_Airports.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports.Model
{
    public class Airline
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Column("iata")]
        [NotEmpty]
        public string IATACode { get; set; }

        [Column("icao")]
        [NotEmpty]
        public string ICAOCode { get; set; }

        [Column("callsign")]
        public string CallSign { get; set; }
    }
}
