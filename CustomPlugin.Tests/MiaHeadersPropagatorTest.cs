using CustomPlugin.Environment;
using Microsoft.AspNetCore.Http;
using NFluent;
using NUnit.Framework;

namespace CustomPlugin.Tests
{
    public class MiaHeadersPropagatorTest
    {
        private MiaHeadersPropagator _miaHeadersPropagator;
        private MiaEnvConfiguration _envConfig = new MiaEnvConfiguration
        {
            USERID_HEADER_KEY = "userid",
            GROUPS_HEADER_KEY = "usergroups",
            CLIENTTYPE_HEADER_KEY = "clienttype",
            BACKOFFICE_HEADER_KEY = "isbackoffice",
            MICROSERVICE_GATEWAY_SERVICE_NAME = "microservice-gateway",
            CRUD_PATH = "crud-path"
        };
        
        [SetUp]
        public void StartMockServer()
        {
            var headers = new HeaderDictionary
            {
                {"userid", "foo"}, {"usergroups", "bar"}, {"clienttype", "baz"}, {"isbackoffice", "true"}
            };
            _miaHeadersPropagator = new MiaHeadersPropagator(headers, _envConfig);
        }

        [Test]
        public void TestGetUserId()
        {
            Check.That(_miaHeadersPropagator.GetUserId()).IsEqualTo("foo");
        }
        
        [Test]
        public void TestGetUserGroups()
        {
            Check.That(_miaHeadersPropagator.GetGroups()).IsEqualTo("bar");
        }
        
        [Test]
        public void TestGetClientType()
        {
            Check.That(_miaHeadersPropagator.GetClientType()).IsEqualTo("baz");
        }
        
        [Test]
        public void TestIsFromBackOfficeTruthy()
        {
            Check.That(_miaHeadersPropagator.IsFromBackOffice()).IsEqualTo(true);
        }
        
        [Test]
        public void TestIsFromBackOfficeFalse()
        {
            var headers = new HeaderDictionary
            {
                {"userid", "foo"}, {"usergroups", "bar"}, {"clienttype", "baz"}, {"isbackoffice", "false"}
            };
            _miaHeadersPropagator = new MiaHeadersPropagator(headers, _envConfig);
            Check.That(_miaHeadersPropagator.IsFromBackOffice()).IsEqualTo(false);
        }
        
        [Test]
        public void TestIsFromBackOfficeFalsy()
        {
            var headers = new HeaderDictionary
            {
                {"userid", "foo"}, {"usergroups", "bar"}, {"clienttype", "baz"}, {"isbackoffice", "foo"}
            };
            _miaHeadersPropagator = new MiaHeadersPropagator(headers, _envConfig);
            Check.That(_miaHeadersPropagator.IsFromBackOffice()).IsEqualTo(false);
        }
        
        [Test]
        public void TestIsFromBackOfficeNull()
        {
            var headers = new HeaderDictionary
            {
                {"userid", "foo"}, {"usergroups", "bar"}, {"clienttype", "baz"}
            };
            _miaHeadersPropagator = new MiaHeadersPropagator(headers, _envConfig);
            Check.That(_miaHeadersPropagator.IsFromBackOffice()).IsEqualTo(false);
        }
    }
}