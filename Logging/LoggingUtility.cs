using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        
        private static IDictionary<string, object> ToDictionary(object source)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                var value = property.GetValue(source);
                dictionary.Add(property.Name, value);
            }
            return dictionary;
        }
        
        public void LogMessage(HttpRequest req, LogLevels logLevel, object customProperties, string message)
        {
            var headerId = req.Headers["x-request-id"];
            var reqId = string.IsNullOrEmpty(headerId) ? 0 : int.Parse(headerId);
            var messageDictionary = new Dictionary<string, object>
            {
                {"level", (int) logLevel},
                {"time", DateTimeOffset.Now.ToUnixTimeMilliseconds()},
                {"reqId", reqId},
                {"msg", message}
            };
            var propsAsDict = ToDictionary(customProperties);
            var merged = messageDictionary
                .Concat(propsAsDict)
                .GroupBy(i => i.Key)
                .ToDictionary(
                    group => group.Key, 
                    group => group.First().Value);
            var jsonString = JsonConvert.SerializeObject(merged, Formatting.None);
            LogWithLevel(logLevel, jsonString);
        }
    }
}