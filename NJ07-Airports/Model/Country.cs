using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports.Model
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ThreeLetterISOCode { get; set; }
        public string TwoLetterISOCode { get; set; }
    }
}
