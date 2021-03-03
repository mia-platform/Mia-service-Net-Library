using System.Collections.Generic;
using System.Dynamic;
using MiaServiceDotNetLibrary.Decorators;
using MiaServiceDotNetLibrary.Decorators.PostDecorators;
using NFluent;
using NUnit.Framework;

namespace MiaServiceDotNetLibrary.Tests.Decorators.PostDecorators
{
    public class PostDecoratorRequestTest
    {
        private PostDecoratorRequest _postDecoratorRequest;
        private MiaEnvsConfigurationsImpl _miaEnvConfiguration;
        private const string BackOfficeHeaderKey = "backoffice";
        private const string ClientTypeHeaderKey = "client-type";
        private const string UserIdHeaderKey = "user-id";
        private const string UserPropertiesHeaderKey = "user-props";
        private const string GroupsHeaderKey = "groups";

        [SetUp]
        public void Setup()
        {
            dynamic requestBody = new ExpandoObject();
            requestBody.foo = "bar";
            requestBody.baz = "bam";
            
            dynamic responseBody = new ExpandoObject();
            responseBody.bar = "foo";
            responseBody.bam = "baz";

            _postDecoratorRequest = new PostDecoratorRequest
            {
                Request = new DecoratorRequest
                {
                    Method = "GET",
                    Path = "test",
                    Headers = new Dictionary<string, string> {{"foo", "bar"}},
                    Query = new Dictionary<string, string> {{"baz", "bam"}},
                    Body = requestBody
                },
                Response = new DecoratorResponse(200, new Dictionary<string, string> {{"bar", "foo"}}, responseBody)
            };
            
            _miaEnvConfiguration = new MiaEnvsConfigurationsImpl
            {
                BACKOFFICE_HEADER_KEY = BackOfficeHeaderKey,
                CLIENTTYPE_HEADER_KEY = ClientTypeHeaderKey,
                USERID_HEADER_KEY = UserIdHeaderKey,
                USER_PROPERTIES_HEADER_KEY = UserPropertiesHeaderKey,
                GROUPS_HEADER_KEY = GroupsHeaderKey,
            };
            
            SetupHeaders();
        }

        private void SetupHeaders()
        {
            _postDecoratorRequest.Request.Headers[BackOfficeHeaderKey] = "true";
            _postDecoratorRequest.Request.Headers[ClientTypeHeaderKey] = "foo";
            _postDecoratorRequest.Request.Headers[UserIdHeaderKey] = "42";
            _postDecoratorRequest.Request.Headers[UserPropertiesHeaderKey] = "bar";
            _postDecoratorRequest.Request.Headers[GroupsHeaderKey] = "100";
        }


        [Test]
        public void TestLeaveOriginalResponseUnmodified()
        {
            var newResponse = _postDecoratorRequest.LeaveOriginalResponseUnmodified();

            Check.That(newResponse).IsEqualTo(null);
        }

        [Test]
        public void TestChangeOriginalResponse()
        {
            dynamic newBody = new ExpandoObject();
            newBody.foo = 42;

            var newRequest = _postDecoratorRequest
                .ChangeOriginalResponse()
                .StatusCode(201)
                .Headers(new Dictionary<string, string> {{"new", "header"}})
                .Body(newBody)
                .Change();

            Check.That(((PostDecoratorRequest) newRequest).Response.StatusCode).Equals(201);
            Check.That(((PostDecoratorRequest) newRequest).Response.Headers).Equals(new Dictionary<string, string> {{"new", "header"}});
            Check.That(((PostDecoratorRequest) newRequest).Response.Body).Equals(newBody);
        }
        
        [Test]
        public void TestDeepCloneIsCreated()
        {
            var newResponse = _postDecoratorRequest
                .ChangeOriginalResponse()
                .Change();

            Check.That(newResponse.Response.StatusCode).Equals(_postDecoratorRequest.Response.StatusCode);
            Check.That(newResponse.Response.Headers).Equals(_postDecoratorRequest.Response.Headers);
            Check.That(newResponse.Response.Body).Equals(_postDecoratorRequest.Response.Body);
            Assert.AreNotSame(newResponse.Response.Headers, _postDecoratorRequest.Response.Headers);
            Assert.AreNotSame(newResponse.Response.Body, _postDecoratorRequest.Response.Body);
        }
        
        [Test]
        public void TestGetUserId()
        {
            var userId = _postDecoratorRequest.GetUserId(_miaEnvConfiguration);

            Check.That(userId).IsEqualTo("42");
        }
        
        [Test]
        public void TestGetUserProperties()
        {
            var userId = _postDecoratorRequest.GetUserProperties(_miaEnvConfiguration);

            Check.That(userId).IsEqualTo("bar");
        }
        
        [Test]
        public void TestGetGroups()
        {
            var userId = _postDecoratorRequest.GetGroups(_miaEnvConfiguration);

            Check.That(userId).IsEqualTo("100");
        }
        
        [Test]
        public void TestGetClientType()
        {
            var userId = _postDecoratorRequest.GetClientType(_miaEnvConfiguration);

            Check.That(userId).IsEqualTo("foo");
        }
        
        [Test]
        public void TestIsFromBackOffice()
        {
            var userId = _postDecoratorRequest.IsFromBackOffice(_miaEnvConfiguration);

            Check.That(userId).IsEqualTo("true");
        }

    }
}
