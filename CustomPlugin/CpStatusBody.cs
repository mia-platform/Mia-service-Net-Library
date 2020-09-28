using System;

namespace CustomPlugin
{
    public class CpStatusBody
    {
        public string Name => System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;

        public string Version => System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString();

        public string Status { get; set; }
    }
}
