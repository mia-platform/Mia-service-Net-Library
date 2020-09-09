using Environment;
using NUnit.Framework;

namespace Environment.Tests
{
    public class ConfigValidatorTest
    {
        [Test]
        public void TestValidConfig()
        {
            var config = new MiaEnvConfiguration
            {
                UserIdHeaderKey = "userid",
                GroupsHeaderKey = "usergroups",
                ClientTypeHeaderKey = "clienttype",
                BackOfficeHeaderKey = "isbackoffice",
                MicroserviceGatewayServiceName = "microservice-gateway",
                CrudPath = "crud-path"
            };

            Assert.DoesNotThrow(() => ConfigValidator.ValidateConfig(config));
        }

        [Test]
        public void TestInvalidConfigAllPropsNull()
        {
            var config = new MiaEnvConfiguration();

            Assert.Throws(typeof(InvalidEnvConfigurationException), () => ConfigValidator.ValidateConfig(config));
        }
        
        [Test]
        public void TestInvalidConfigOnePropNull()
        {
            var config = new MiaEnvConfiguration
            {
                UserIdHeaderKey = "userid",
                GroupsHeaderKey = "usergroups",
                ClientTypeHeaderKey = "clienttype",
                BackOfficeHeaderKey = "isbackoffice",
                MicroserviceGatewayServiceName = "microservice-gateway",
            };

            Assert.Throws(typeof(InvalidEnvConfigurationException), () => ConfigValidator.ValidateConfig(config));
        }
    }
}
