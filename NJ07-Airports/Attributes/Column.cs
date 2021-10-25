using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports
{
    public class Column : Attribute
    {

        public Column(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
