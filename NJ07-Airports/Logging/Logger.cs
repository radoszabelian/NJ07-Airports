using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports.Logging
{
    public static class Logger
    {
        private static readonly NLog.Logger LoggerService = NLog.LogManager.GetCurrentClassLogger();

        static Logger()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
        }

        public static void LogError(string message)
        {
            LoggerService.Error(message);
        }

        public static void LogInfo(string message)
        {
            LoggerService.Info(message);
        }
    }
}
