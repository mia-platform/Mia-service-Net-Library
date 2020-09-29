using System;
using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Logging
{
    public class LoggingUtility
    {
        private readonly ILog _logger;

        public LoggingUtility(ILog logger)
        {
            _logger = logger;
        }
        
        private void LogWithLevel(LogLevels level, string message)
        {
            switch (level)
            {
                case LogLevels.Trace:
                    _logger.Trace(message);
                    break;
                case LogLevels.Debug:
                    _logger.Debug(message);
                    break;
                case LogLevels.Info:
                    _logger.Info(message);
                    break;
                case LogLevels.Warn:
                    _logger.Warn(message);
                    break;
                case LogLevels.Error:
                    _logger.Error(message);
                    break;
                case LogLevels.Fatal:
                    _logger.Fatal(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
        
        public void LogRequest(RequestLog requestLog)
        {
            var jsonString = JsonConvert.SerializeObject(requestLog, Formatting.None);
            _logger.Info(jsonString);
        }

        public void LogMessage(HttpRequest req, LogLevels logLevel, object logProperties, string message)
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