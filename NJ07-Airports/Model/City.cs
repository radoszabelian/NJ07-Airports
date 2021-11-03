namespace NJ07_Airports.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class City
    {
        public int CountryId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string TimeZoneName { get; set; }

        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
