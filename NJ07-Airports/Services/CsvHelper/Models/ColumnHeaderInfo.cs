﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports.Services.CsvHelper.Models
{
    public struct ColumnHeaderInfo
    {
        public string ClassPropName;
        public int IndexInFileRow;
        public bool NotEmpty;
    }
}
