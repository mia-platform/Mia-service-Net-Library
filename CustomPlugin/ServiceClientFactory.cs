using System;
using Service;

namespace CustomPlugin
{
    public static class ServiceClientFactory
    {
        const string MICROSERVICE_GATEWAY_SERVICE_NAME_KEY = "MICROSERVICE_GATEWAY_SERVICE_NAME";

        public static ServiceProxy GetDirectServiceProxy(string serviceName, InitServiceOptions options)
        {
            return new ServiceProxy(serviceName, options);
        }

        public static ServiceProxy GetServiceProxy(InitServiceOptions options)
        {
            string microserviceNameKey = Environment.GetEnvironmentVariable(MICROSERVICE_GATEWAY_SERVICE_NAME_KEY);
            if (microserviceNameKey == null)
            {
                return null;
            }

            return new ServiceProxy(microserviceNameKey, options);
        }
    }
}