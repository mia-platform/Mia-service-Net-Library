using System.Collections.Generic;
using Crud;
using Environment;
using Microsoft.Extensions.Primitives;
using NFluent;
using NUnit.Framework;
using Service;

namespace CustomPlugin.Tests
{
    public class ServiceClientFactoryTest
    {
        private ServiceClientFactory _serviceClientFactory;
        private MiaEnvConfiguration _miaEnvConfiguration;

        [SetUp]
        public void SetUp()
        {
            _miaEnvConfiguration = new MiaEnvConfiguration
            {
                USERID_HEADER_KEY = "1",
                GROUPS_HEADER_KEY = "2",
                CLIENTTYPE_HEADER_KEY = "3",
                BACKOFFICE_HEADER_KEY = "4",
                MICROSERVICE_GATEWAY_SERVICE_NAME = "gateway-name"
            };
            _serviceClientFactory = new ServiceClientFactory(_miaEnvConfiguration);
            var miaHeaders = new Dictionary<string, StringValues>
            {
                {"1", "_"},
                {"2", "_"},
                {"3", "_"},
                {"4", "_"},
                {"gateway-name", "bar"},
            };
            var miaHeadersPropagator = new MiaHeadersPropagator(miaHeaders, _miaEnvConfiguration);
            ServiceClientFactory.SetMiaHeaders(miaHeadersPropagator);
        }

        [Test]
        public void TestGetDirectServiceProxy()
        {
            var result = _serviceClientFactory.GetDirectServiceProxy("service", new InitServiceOptions());
            Check.That(result).IsInstanceOf<ServiceProxy>();
        }

        [Test]
        public void TestGetServiceProxyWithEnv()
        {
            var result = _serviceClientFactory.GetServiceProxy(new InitServiceOptions());
            Check.That(result).IsInstanceOf<ServiceProxy>();
        }

        [Test]
        public void TestGetServiceProxyWithoutEnv()
        {
            var configWithoutMsGatewayName = new MiaEnvConfiguration
            {
                USERID_HEADER_KEY = "1",
                GROUPS_HEADER_KEY = "2",
                CLIENTTYPE_HEADER_KEY = "3",
                BACKOFFICE_HEADER_KEY = "4",
                MICROSERVICE_GATEWAY_SERVICE_NAME = ""
            };
            
            var factory = new ServiceClientFactory(configWithoutMsGatewayName);
            var result = factory.GetServiceProxy(new InitServiceOptions());

            Check.That(result).IsNull();
        }
        
        [Test]
        public void TestGetCrudServiceClientRequiredParams()
        {
            var result = _serviceClientFactory.GetCrudServiceClient("http://foo:8080");
            Check.That(result).IsInstanceOf<CrudServiceClient>();
        }
        
        [Test]
        public void TestGetCrudServiceClientAllParams()
        {
            var result = _serviceClientFactory.GetCrudServiceClient("http://foo:8080", "secret", 3);
            Check.That(result).IsInstanceOf<CrudServiceClient>();
        }
    }
}
