using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using CustomPlugin.Environment;
using Microsoft.AspNetCore.Http;
using Service;

namespace CustomPlugin
{
    public class ServiceClientFactory
    {
        private readonly MiaEnvConfiguration _miaEnvConfiguration;
        private static  HttpRequestHeaders _miaHeaders = ServiceProxy.GetDefaultHeaders();
        private static HttpRequestHeaders MiaHeaders => _miaHeaders;

        public static void SetMiaHeaders(MiaHeadersPropagator miaHeadersPropagator)
        {
            _miaHeaders = ServiceProxy.GetDefaultHeaders();

            foreach (var (key, value) in miaHeadersPropagator.Headers)
            {
                MiaHeaders.Remove(key);
                MiaHeaders.Add(key, value);
            }
        }

        public ServiceClientFactory(MiaEnvConfiguration miaEnvConfiguration)
        {
            _miaEnvConfiguration = miaEnvConfiguration;
        }

        public ServiceProxy GetDirectServiceProxy(string serviceName, InitServiceOptions options)
        {
            return new ServiceProxy(serviceName, options);
        }

        public ServiceProxy GetServiceProxy(InitServiceOptions options)
        {
            var microserviceNameKey = _miaEnvConfiguration.MicroserviceGatewayServiceName;
            return string.IsNullOrEmpty(microserviceNameKey)
                ? null
                : new ServiceProxy(microserviceNameKey, options);
        }
    }
}
