using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using CustomPlugin.Environment;
using Microsoft.AspNetCore.Http;
using Service;
using Crud;

namespace CustomPlugin
{
    public class ServiceClientFactory
    {
        private readonly MiaEnvConfiguration _miaEnvConfiguration;
        private static Dictionary<string, string> _miaHeaders;
        private static Dictionary<string, string> MiaHeaders => _miaHeaders;

        public static void SetMiaHeaders(MiaHeadersPropagator miaHeadersPropagator)
        {
            _miaHeaders = miaHeadersPropagator.Headers;
        }

        public ServiceClientFactory(MiaEnvConfiguration miaEnvConfiguration)
        {
            _miaEnvConfiguration = miaEnvConfiguration;
        }

        public ServiceProxy GetDirectServiceProxy(string serviceName, InitServiceOptions options)
        {
            var proxy = new ServiceProxy(serviceName, options);
            proxy.AddMiaHeaders(_miaHeaders);
            return proxy;
        }

        public ServiceProxy GetServiceProxy(InitServiceOptions options)
        {
            var microserviceNameKey = _miaEnvConfiguration.MICROSERVICE_GATEWAY_SERVICE_NAME;
            var proxy = string.IsNullOrEmpty(microserviceNameKey)
                ? null
                : new ServiceProxy(microserviceNameKey, options);

            if (proxy == null) return null;
            proxy.AddMiaHeaders(_miaHeaders);
            return proxy;
        }

        public CrudServiceClient GetCrudServiceClient(
            string apiPath = default(string),
            string apiSecret = default(string), int crudVersion = default(int))
        {
            return new CrudServiceClient(_miaHeaders, apiPath, apiSecret, crudVersion); 
        }
    }
}
