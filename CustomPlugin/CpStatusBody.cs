using System;

namespace CustomPlugin
{
    public class CpStatusBody
    {
        public string Name => System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;

        public Version Version => System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version;

        public string Status { get; set; }
    }
}
