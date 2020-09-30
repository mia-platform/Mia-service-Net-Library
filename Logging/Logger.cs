using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Logging
{
    public class Logger
    {
        private readonly ILog _logger;
        
        public Logger(ILog logger)
        {
            _logger = logger;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public void Trace(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Trace, customProperties, message);
            _logger.Trace(jsonString);
        }
        
        public void Debug(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Debug, customProperties, message);
            _logger.Debug(jsonString);
        }
        
        public void Info(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Info, customProperties, message);
            _logger.Info(jsonString);
        }
        
        public void Warn(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Warn, customProperties, message);
            _logger.Warn(jsonString);
        }
        
        public void Error(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Error, customProperties, message);
            _logger.Error(jsonString);
        }
        
        public void Fatal(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Fatal, customProperties, message);
            _logger.Fatal(jsonString);
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

        private static string GetJsonLog(HttpRequest req, LogLevels logLevel, object customProperties, string message)
        {
            var headerId = req.Headers["x-request-id"];
            var reqId = string.IsNullOrEmpty(headerId) ? 0 : int.Parse(headerId);
            var messageDictionary = new Dictionary<string, object>
            {
                {"Level", (int) logLevel},
                {"Time", DateTimeOffset.Now.ToUnixTimeMilliseconds()},
                {"ReqId", reqId},
                {"Msg", message}
            };
            var propsAsDict = ToDictionary(customProperties);
            var merged = messageDictionary
                .Concat(propsAsDict)
                .GroupBy(i => i.Key)
                .ToDictionary(
                    group => @group.Key,
                    group => @group.First().Value);
            var jsonString = JsonConvert.SerializeObject(merged, Formatting.None);
            return jsonString;
        }
    }
}