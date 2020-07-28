using System;

namespace Service
{
    public class InvalidEnvConfigurationException : Exception
    {
        public InvalidEnvConfigurationException(string message) : base(message)
        {
        }
    }
}