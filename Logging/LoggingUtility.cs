using System;
using System.Diagnostics;
using System.Reflection;
using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Logging
{
    public class LoggingUtility
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void LogWithLevel(LogLevels level, string message)
        {
            switch (level)
            {
                case LogLevels.Trace:
                    Logger.Trace(message);
                    break;
                case LogLevels.Debug:
                    Logger.Debug(message);
                    break;
                case LogLevels.Info:
                    Logger.Info(message);
                    break;
                case LogLevels.Warn:
                    Logger.Warn(message);
                    break;
                case LogLevels.Error:
                    Logger.Error(message);
                    break;
                case LogLevels.Fatal:
                    Logger.Fatal(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
        
        public static void LogRequest(RequestLog requestLog)
        {
            var jsonString = JsonConvert.SerializeObject(requestLog, Formatting.None);
            Logger.Info(jsonString);
        }

        public static void LogMessage(HttpRequest req, LogLevels logLevel, object logProperties, string message)
        {
            var headerId = req.Headers["x-request-id"];
            var reqId = string.IsNullOrEmpty(headerId) ? 0 : int.Parse(headerId);
            var requestLog = new MessageLog
                {
                    Level = (int) logLevel,
                    Time = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    ReqId = reqId,
                    Msg = message,
                    LogProperties = logProperties
                };
            var jsonString = JsonConvert.SerializeObject(requestLog, Formatting.None);
            LogWithLevel(logLevel, jsonString);
        }
        
    }
}