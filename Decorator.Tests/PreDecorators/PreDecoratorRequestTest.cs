using System.Collections.Generic;
using System.Dynamic;
using Decorators.PreDecorators;
using NFluent;
using NUnit.Framework;

namespace Decorator.Tests.PreDecorators
{
    public class PreDecoratorRequestTest
    {
        private PreDecoratorRequest _preDecoratorRequest;

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
        }

        [Test]
        public void TestOriginalRequestUnmodified()
        {
            var newRequest = _preDecoratorRequest.LeaveOriginalRequestUnmodified();

            Check.That(newRequest).IsEqualTo(null);
        }

        [Test]
        public void TestOriginalRequestGetsModified()
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

            Check.That(((PreDecoratorRequest) newRequest).Method).IsEqualTo("POST");
            Check.That(((PreDecoratorRequest) newRequest).Path).IsEqualTo("new-path");
            Assert.IsFalse(((PreDecoratorRequest) newRequest).Body.Equals(_preDecoratorRequest.Body));
            Assert.IsFalse(((PreDecoratorRequest) newRequest).Headers.Equals(_preDecoratorRequest.Headers));
            Assert.IsFalse(((PreDecoratorRequest) newRequest).Query.Equals(_preDecoratorRequest.Query));
        }
        
        [Test]
        public void TestDeepCloneIsCreated()
        {
            dynamic newBody = new ExpandoObject();
            newBody.foo = 42;

            var newRequest = _preDecoratorRequest
                .ChangeOriginalRequest()
                .Change();

            Check.That(newRequest.Method).IsEqualTo(_preDecoratorRequest.Method);
            Check.That(newRequest.Path).IsEqualTo(_preDecoratorRequest.Path);
            Assert.IsFalse(newRequest.Body.Equals(_preDecoratorRequest.Body));
            Assert.IsFalse(newRequest.Headers.Equals(_preDecoratorRequest.Headers));
            Assert.IsFalse(newRequest.Query.Equals(_preDecoratorRequest.Query));
        }
    }
}
