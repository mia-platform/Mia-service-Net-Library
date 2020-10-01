using System;

namespace MiaServiceDotNetLibrary.Environment
{
    public class InvalidEnvConfigurationException : Exception
    {
        public InvalidEnvConfigurationException(string message) : base("Invalid configuration: " + message)
        {
        }
    }
}
