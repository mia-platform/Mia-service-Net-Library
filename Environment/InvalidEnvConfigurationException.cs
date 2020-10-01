using System;

namespace Environment
{
    public class InvalidEnvConfigurationException : Exception
    {
        public InvalidEnvConfigurationException(string message) : base("Invalid configuration: " + message)
        {
        }
    }
}
