using System.Reflection;
using log4net;
using Newtonsoft.Json;

namespace Logging
{
    public static class LoggingUtility
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public static void LogIncomingRequest(IncomingRequestLog requestLog)
        {
            string jsonString = JsonConvert.SerializeObject(requestLog, Formatting.None);
            Logger.Info(jsonString);
        }
        public static void LogCompletedRequest(CompletedRequestLog requestLog)
        {
            string jsonString = JsonConvert.SerializeObject(requestLog, Formatting.None);
            Logger.Info(jsonString);
        }
    }
}