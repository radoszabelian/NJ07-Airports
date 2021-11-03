namespace NJ07_Airports.Logging
{
    using System;
    using NLog;

    public class LoggerService : ILogger
    {
        private Logger nlogService;

        public LoggerService(Logger nLogService)
        {
            this.nlogService = nLogService;

            this.ConfigureNLogService();
        }

        public void LogError(Exception exception)
        {
            this.nlogService.Error($"{exception.GetType()} - {exception.Message}");
        }

        public void LogLine(string message, int lineNumber)
        {
            this.nlogService.Info($"{lineNumber}. {message}");
        }

        private void ConfigureNLogService()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
        }
    }
}
