using NJ07_Airports.Model;
using NJ07_Airports.SerializeToJson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NJ07_Airports
{
    class Program
    {
        static void Main(string[] args)
        {
            new Menu().Start();

            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine(dataHandler.Airlines.Count());
            //Console.WriteLine(dataHandler.Flights.Count());
            //Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
