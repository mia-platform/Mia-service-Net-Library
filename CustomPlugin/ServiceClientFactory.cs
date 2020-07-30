using System;
using Service;
using Service.Environment;

namespace CustomPlugin
{
    public class ServiceClientFactory
    {
        private const string MICROSERVICE_GATEWAY_SERVICE_NAME_KEY = "MICROSERVICE_GATEWAY_SERVICE_NAME";
        private readonly MiaEnvConfiguration _miaEnvConfiguration;

        public ServiceClientFactory(MiaEnvConfiguration miaEnvConfiguration)
        {
            _miaEnvConfiguration = miaEnvConfiguration;
        }

        public ServiceProxy GetDirectServiceProxy(string serviceName, InitServiceOptions options)
        {
            return new ServiceProxy(serviceName, options, _miaEnvConfiguration);
        }

        public ServiceProxy GetServiceProxy(InitServiceOptions options)
        {
            var microserviceNameKey = Environment.GetEnvironmentVariable(MICROSERVICE_GATEWAY_SERVICE_NAME_KEY);
            return string.IsNullOrEmpty(microserviceNameKey) ? null : new ServiceProxy(microserviceNameKey, options, _miaEnvConfiguration);
        }
    }
}