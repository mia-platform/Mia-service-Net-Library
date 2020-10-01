using System;

namespace CustomService
{
    public class ServiceStatusBody
    {
        public string Name => System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;

        public string Version => System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString();

        public string Status { get; set; }
    }
}
