using System.Collections.Generic;
using Crud;
using Environment;
using Service;

namespace CustomPlugin
{
    public class ServiceClientFactory
    {
        private readonly MiaEnvConfiguration _miaEnvConfiguration;
        private static Dictionary<string, string> _miaHeaders;

        public static void SetMiaHeaders(MiaHeadersPropagator miaHeadersPropagator)
        {
            _miaHeaders = miaHeadersPropagator.Headers;
        }

        public ServiceClientFactory(MiaEnvConfiguration miaEnvConfiguration)
        {
            _miaEnvConfiguration = miaEnvConfiguration;
        }

        public IServiceProxy GetDirectServiceProxy(string serviceName, InitServiceOptions options)
        {
            return new ServiceProxy(_miaHeaders, serviceName, options);
        }

        public IServiceProxy GetServiceProxy(InitServiceOptions options)
        {
            var microserviceNameKey = _miaEnvConfiguration.MICROSERVICE_GATEWAY_SERVICE_NAME;
            return string.IsNullOrEmpty(microserviceNameKey)
                ? null
                : new ServiceProxy(_miaHeaders, microserviceNameKey, options);
        }

        public ICrudServiceClient GetCrudServiceClient(
            string apiPath = default(string),
            string apiSecret = default(string), int crudVersion = default(int))
        {
            return new CrudServiceClient(_miaHeaders, apiPath, apiSecret, crudVersion); 
        }
    }
}
