using System.Reflection;
using log4net;
using Newtonsoft.Json;

namespace Logging
{
    public static class LoggingUtility
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public static void LogRequest(CompletedRequestLog requestLog)
        {
            var jsonString = JsonConvert.SerializeObject(requestLog, Formatting.None);
            Logger.Info(jsonString);
        }
    }
}