using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiaServiceDotNetLibrary.Logging
{
    public static class Logger
    {
        private static ILog _loggerInstance = LogManager.GetLogger(typeof(Logger));
        
        public static void SetInstance(ILog loggerInstance)
        {
            _loggerInstance = loggerInstance;
        }
        
        public static void Trace(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Trace, customProperties, message);
            _loggerInstance.Trace(jsonString);
        }

        public static void Trace(string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(null, LogLevels.Trace, customProperties, message);
            _loggerInstance.Trace(jsonString);
        }
        
        public static void Debug(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Debug, customProperties, message);
            _loggerInstance.Debug(jsonString);
        }

        public static void Debug(string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(null, LogLevels.Debug, customProperties, message);
            _loggerInstance.Debug(jsonString);
        }
        
        public static void Info(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Info, customProperties, message);
            _loggerInstance.Info(jsonString);
        }

        public static void Info(string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(null, LogLevels.Info, customProperties, message);
            _loggerInstance.Info(jsonString);
        }
        
        public static void Warn(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Warn, customProperties, message);
            _loggerInstance.Warn(jsonString);
        }

        public static void Warn(string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(null, LogLevels.Warn, customProperties, message);
            _loggerInstance.Warn(jsonString);
        }
        
        public static void Error(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Error, customProperties, message);
            _loggerInstance.Error(jsonString);
        }

        public static void Error(string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(null, LogLevels.Error, customProperties, message);
            _loggerInstance.Error(jsonString);
        }
        
        public static void Fatal(HttpRequest request, string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(request, LogLevels.Fatal, customProperties, message);
            _loggerInstance.Fatal(jsonString);
        }

        public static void Fatal(string message, object customProperties = null)
        {
            var jsonString = GetJsonLog(null, LogLevels.Fatal, customProperties, message);
            _loggerInstance.Fatal(jsonString);
        }
        
        internal static void LogRequest(CompletedRequestLog completedRequestLog)
        {
            CamelCaseSerialize();
            var jsonString = JsonConvert.SerializeObject(completedRequestLog, Formatting.None);
            _loggerInstance.Info(jsonString);
        }
        
        internal static void LogRequest(IncomingRequestLog incomingRequestLog)
        {
            CamelCaseSerialize();
            var jsonString = JsonConvert.SerializeObject(incomingRequestLog, Formatting.None);
            _loggerInstance.Trace(jsonString);
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
            CamelCaseSerialize();
            var messageDictionary = new Dictionary<string, object>
            {
                {"Level", (int) logLevel},
                {"Time", DateTimeOffset.Now.ToUnixTimeMilliseconds()},
                {"Msg", message}
            };
            if (req != null) 
            {
                string reqId;
                try 
                {
                    reqId = (string)req.HttpContext.Items[RequestResponseLoggingMiddleware.RequestIdDictionaryKey];
                }
                catch (Exception)
                {
                    reqId = RequestResponseLoggingMiddleware.GetOrGenerateReqId(req);
                }
                messageDictionary.Add("ReqId", reqId);
            }

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

        private static void CamelCaseSerialize()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}