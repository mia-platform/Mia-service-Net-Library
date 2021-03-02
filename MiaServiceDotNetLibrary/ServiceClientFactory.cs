using System.Collections.Generic;
using MiaServiceDotNetLibrary.Crud;
using MiaServiceDotNetLibrary.Environment;
using MiaServiceDotNetLibrary.Service;

namespace MiaServiceDotNetLibrary
{
    public class ServiceClientFactory
    {
        private readonly MiaEnvsConfigurations _envsConfigurations;
        private static Dictionary<string, string> _miaHeaders;

        public static void SetMiaHeaders(MiaHeadersPropagator miaHeadersPropagator)
        {
            _miaHeaders = miaHeadersPropagator.Headers;
        }

        public ServiceClientFactory(MiaEnvsConfigurations envsConfigurations)
        {
            _envsConfigurations = envsConfigurations;
        }

        public IServiceProxy GetDirectServiceProxy(string serviceName, InitServiceOptions options)
        {
            return new ServiceProxy(_miaHeaders, serviceName, options);
        }

        public IServiceProxy GetServiceProxy(InitServiceOptions options)
        {
            var microserviceGatewayName = _envsConfigurations.MICROSERVICE_GATEWAY_SERVICE_NAME;
            
            return string.IsNullOrEmpty(microserviceGatewayName)
                ? null
                : new ServiceProxy(_miaHeaders, microserviceGatewayName, options);
        }

        public ICrudServiceClient GetCrudServiceClient(
            string apiPath,
            string apiSecret = default(string), int crudVersion = default(int))
        {
            return new CrudServiceClient(_miaHeaders, apiPath, apiSecret, crudVersion); 
        }
    }
}
