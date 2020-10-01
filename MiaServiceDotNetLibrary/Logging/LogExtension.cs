using System;
using log4net;

namespace MiaServiceDotNetLibrary.Logging
{
    public static class LogExtension
    {
        public static void Trace(this ILog log, string message, Exception exception)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType,
                log4net.Core.Level.Trace, message, exception);
        }

        public static void Trace(this ILog log, string message)
        {
            log.Trace(message, null);
        }
    }
}
