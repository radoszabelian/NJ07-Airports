namespace NJ07_Airports.Logging
{
    using System;

    public interface ILogger
    {
        void LogLine(string message, int lineNumber);

        void LogError(Exception exception);
    }
}
