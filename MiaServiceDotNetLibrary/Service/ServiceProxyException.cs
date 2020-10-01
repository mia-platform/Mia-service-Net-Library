using System;

namespace Service
{
    public class ServiceProxyException : Exception
    {
        public ServiceProxyException(string message) : base("Service proxy error: " + message)
        {
        }
    }
}
