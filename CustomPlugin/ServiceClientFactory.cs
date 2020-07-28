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
            var microserviceNameKey = Environment.GetEnvironmentVariable(MICROSERVICE_GATEWAY_SERVICE_NAME_KEY);
            return string.IsNullOrEmpty(microserviceNameKey) ? null : new ServiceProxy(microserviceNameKey, options);
        }
    }
}