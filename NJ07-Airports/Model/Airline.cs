namespace NJ07_Airports.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NJ07_Airports.Attributes;

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
