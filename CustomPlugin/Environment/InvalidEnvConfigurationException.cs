using System;

namespace CustomPlugin.Environment
{
    public class InvalidEnvConfigurationException : Exception
    {
        public InvalidEnvConfigurationException(string message) : base(message)
        {
        }
    }
}