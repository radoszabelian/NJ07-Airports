﻿namespace NJ07_Airports.Services.CsvHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICsvHelper
    {
        public List<T> Parse<T>(string filePath)
            where T : new();
    }
}