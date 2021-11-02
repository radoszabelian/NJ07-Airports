using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports.Logging
{
    public interface ILogger
    {
        void LogLine(string message, int lineNumber);
        void LogError(Exception exception);
    }
}
