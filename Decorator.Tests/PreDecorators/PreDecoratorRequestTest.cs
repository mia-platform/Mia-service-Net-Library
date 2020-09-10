using System.Collections.Generic;
using System.Dynamic;
using Decorators.PreDecorators;
using Environment;
using NFluent;
using NUnit.Framework;

namespace Decorator.Tests.PreDecorators
{
    public class PreDecoratorRequestTest
    {
        private PreDecoratorRequest _preDecoratorRequest;
        private MiaEnvConfiguration _miaEnvConfiguration;
        private const string BackOfficeHeaderKey = "backoffice";
        private const string ClientTypeHeaderKey = "client-type";
        private const string UserIdHeaderKey = "user-id";
        private const string GroupsHeaderKey = "groups";

        [SetUp]
        public void Setup()
        {
            dynamic body = new ExpandoObject();
            body.foo = "bar";
            body.baz = "bam";

            _preDecoratorRequest = new PreDecoratorRequest
            {
                Method = "GET",
                Path = "test",
                Headers = new Dictionary<string, string> {{"foo", "bar"}},
                Query = new Dictionary<string, string> {{"baz", "bam"}},
                Body = body
            };

            _miaEnvConfiguration = new MiaEnvConfiguration
            {
                BACKOFFICE_HEADER_KEY = BackOfficeHeaderKey,
                CLIENTTYPE_HEADER_KEY = ClientTypeHeaderKey,
                USERID_HEADER_KEY = UserIdHeaderKey,
                GROUPS_HEADER_KEY = GroupsHeaderKey,
            };

            SetupHeaders();
        }

        private void SetupHeaders()
        {
            _preDecoratorRequest.Headers[BackOfficeHeaderKey] = "true";
            _preDecoratorRequest.Headers[ClientTypeHeaderKey] = "foo";
            _preDecoratorRequest.Headers[UserIdHeaderKey] = "42";
            _preDecoratorRequest.Headers[GroupsHeaderKey] = "100";
        }

        [Test]
        public void TestLeaveOriginalRequestUnmodified()
        {
            var newRequest = _preDecoratorRequest.LeaveOriginalRequestUnmodified();

            Check.That(newRequest).IsEqualTo(null);
        }

        [Test]
        public void TestChangeOriginalRequest()
        {
            dynamic newBody = new ExpandoObject();
            newBody.foo = 42;

            var newRequest = _preDecoratorRequest
                .ChangeOriginalRequest()
                .Method("POST")
                .Path("new-path")
                .Headers(new Dictionary<string, string> {{"new", "header"}})
                .Query(new Dictionary<string, string> {{"new", "query"}})
                .Body(newBody)
                .Change();

            Check.That(((PreDecoratorRequest) newRequest).Method).Equals("POST");
            Check.That(((PreDecoratorRequest) newRequest).Path).Equals("new-path");
            Check.That(((PreDecoratorRequest) newRequest).Headers).Equals(new Dictionary<string, string> {{"new", "header"}});
            Check.That(((PreDecoratorRequest) newRequest).Query).Equals(new Dictionary<string, string> {{"new", "query"}});
            Check.That(((PreDecoratorRequest) newRequest).Body).Equals(newBody);
        }
        
        [Test]
        public void TestDeepCloneIsCreated()
        {
            var newRequest = _preDecoratorRequest
                .ChangeOriginalRequest()
                .Change();

            Check.That(newRequest.Method).Equals(_preDecoratorRequest.Method);
            Check.That(newRequest.Path).Equals(_preDecoratorRequest.Path);
            Check.That(newRequest.Headers).Equals(_preDecoratorRequest.Headers);
            Check.That(newRequest.Query).Equals(_preDecoratorRequest.Query);
            Check.That(newRequest.Body).Equals(_preDecoratorRequest.Body);
            Assert.AreNotSame(newRequest.Body, _preDecoratorRequest.Body);
            Assert.AreNotSame(newRequest.Headers, _preDecoratorRequest.Headers);
            Assert.AreNotSame(newRequest.Query, _preDecoratorRequest.Query);
        }
        
        [Test]
        public void TestGetUserId()
        {
            var userId = _preDecoratorRequest.GetUserId(_miaEnvConfiguration);

            Check.That(userId).IsEqualTo("42");
        }
        
        [Test]
        public void TestGetGroups()
        {
            var userId = _preDecoratorRequest.GetGroups(_miaEnvConfiguration);

            Check.That(userId).IsEqualTo("100");
        }
        
        [Test]
        public void TestGetClientType()
        {
            var userId = _preDecoratorRequest.GetClientType(_miaEnvConfiguration);

            Check.That(userId).IsEqualTo("foo");
        }
        
        [Test]
        public void TestIsFromBackOffice()
        {
            var userId = _preDecoratorRequest.IsFromBackOffice(_miaEnvConfiguration);

            Check.That(userId).IsEqualTo("true");
        }
    }
}
