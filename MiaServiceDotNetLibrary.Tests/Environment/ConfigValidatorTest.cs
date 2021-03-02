using System.ComponentModel.DataAnnotations;
using MiaServiceDotNetLibrary.Environment;
using NUnit.Framework;

namespace MiaServiceDotNetLibrary.Tests.Environment
{
    class CustomEnvsSchema : MiaEnvsSchema
    {
        [Required]
        [MinLength(10)]
        public string MyCustomEnvVariable { get; set; }

        [MaxLength(5)]
        public string MyMandatoryVariable { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Range(10, 10000000)]
        public int MyNumberVariable { get; set; }
    }

    class CustomEnvsSchemaWithCustomValidation: MiaEnvsSchema
    {
        [Required]
        public string MyStaticString { get; set; }

        public override void Validate()
        {
            if(MyStaticString != "test string")
            {
                throw new ValidationException($"The string MyStaticString must be 'test string'; actual value: {MyStaticString}");
            }
        }
    }

    public class ConfigValidatorTest
    {
        [Test]
        public void TestValidConfig()
        {
            var config = new CustomEnvsSchema
            {
                USERID_HEADER_KEY = "userheaderkey",
                USER_PROPERTIES_HEADER_KEY = "userproperties",
                GROUPS_HEADER_KEY = "usergroups",
                CLIENTTYPE_HEADER_KEY = "clienttype",
                BACKOFFICE_HEADER_KEY = "isbackoffice",
                MICROSERVICE_GATEWAY_SERVICE_NAME = "microservice-gateway",
                MyCustomEnvVariable = "customEnvVariable",
                MyMandatoryVariable = "var",
                MyNumberVariable = 54
            };

            Assert.DoesNotThrow(() => config.Validate());
        }

        [Test]
        public void TestInvalidConfigAllPropsNull()
        {
            var config = new CustomEnvsSchema();

            Assert.Throws(typeof(ValidationException), () => config.Validate());
        }

        [Test]
        public void TestInvalidCustomPropertyConstraintNotSatisfied()
        {
            var config = new CustomEnvsSchema()
            {
                USERID_HEADER_KEY = "userheaderkey",
                USER_PROPERTIES_HEADER_KEY = "userproperties",
                GROUPS_HEADER_KEY = "usergroups",
                CLIENTTYPE_HEADER_KEY = "clienttype",
                BACKOFFICE_HEADER_KEY = "isbackoffice",
                MICROSERVICE_GATEWAY_SERVICE_NAME = "microservice-gateway",
                MyCustomEnvVariable = "customEnvVariable",
                MyNumberVariable = 1
            };

            Assert.Throws(typeof(ValidationException), () => config.Validate());
        }

        [Test]
        public void TestValidConfigsWithCustomValidator_BaseValidatorNotUsed()
        {
            var config = new CustomEnvsSchemaWithCustomValidation()
            {
                MyStaticString = "test string"
            };

            Assert.DoesNotThrow(() => config.Validate());
        }

        [Test]
        public void TestInvalidConfigsWithCustomValidator_BaseValidatorNotUsed()
        {
            var config = new CustomEnvsSchemaWithCustomValidation()
            {
                MyStaticString = "wrong string"
            };

            Assert.Throws(typeof(ValidationException), () => config.Validate());
        }

        //[Test]
        //public void TestInvalidConfigOnePropNull()
        //{
        //    var config = new MiaEnvConfiguration
        //    {
        //        USER_PROPERTIES_HEADER_KEY = "userproperties",
        //        GROUPS_HEADER_KEY = "usergroups",
        //        CLIENTTYPE_HEADER_KEY = "clienttype",
        //        BACKOFFICE_HEADER_KEY = "isbackoffice",
        //        MICROSERVICE_GATEWAY_SERVICE_NAME = "microservice-gateway",
        //    };

        //    Assert.Throws(typeof(InvalidEnvConfigurationException), () => ConfigValidator.ValidateConfig(config));
        //}
    }
}
