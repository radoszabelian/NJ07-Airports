using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports.Logging
{
    public class LoggerService : ILogger
    {
        Logger _nlogService;

        public LoggerService(Logger nLogService)
        {
            _nlogService = nLogService;

            ConfigureNLogService();
        }

        public void LogError(Exception exception)
        {
            _nlogService.Error($"{exception.GetType()} - {exception.Message}");
        }

        public void LogLine(string message, int lineNumber)
        {
            _nlogService.Info($"{lineNumber}. {message}");
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
