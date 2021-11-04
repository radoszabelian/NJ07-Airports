namespace Airports_Logic.Services
{
    using System;

    public interface ILogger
    {
        void LogLine(string message, int lineNumber);

        void LogError(Exception exception);
    }
}
