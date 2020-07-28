using NUnit.Framework;
using Service.Environment;

namespace Service.Tests.Environment
{
    public class ConfigValidatorTest
    {
        [Test]
        public void TestValidConfig()
        {
            var config = new MiaEnvConfiguration
            {
                USERID_HEADER_KEY = "userid",
                GROUPS_HEADER_KEY = "usergroups",
                CLIENTTYPE_HEADER_KEY = "clienttype",
                BACKOFFICE_HEADER_KEY = "isbackoffice",
                MICROSERVICE_GATEWAY_SERVICE_NAME = "microservice-gateway",
                CRUD_PATH = "crud-path"
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
                USERID_HEADER_KEY = "userid",
                GROUPS_HEADER_KEY = "usergroups",
                CLIENTTYPE_HEADER_KEY = "clienttype",
                BACKOFFICE_HEADER_KEY = "isbackoffice",
                MICROSERVICE_GATEWAY_SERVICE_NAME = "microservice-gateway",
            };

            Assert.Throws(typeof(InvalidEnvConfigurationException), () => ConfigValidator.ValidateConfig(config));
        }
    }
}