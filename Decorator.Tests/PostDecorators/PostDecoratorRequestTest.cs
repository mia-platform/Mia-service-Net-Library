using System.Collections.Generic;
using System.Dynamic;
using Decorators;
using Decorators.PostDecorators;
using Decorators.PreDecorators;
using NFluent;
using NUnit.Framework;

namespace Decorator.Tests.PostDecorators
{
    public class PostDecoratorRequestTest
    {
        private PostDecoratorRequest _postDecoratorRequest;

        [SetUp]
        public void Setup()
        {
            dynamic requestBody = new ExpandoObject();
            requestBody.foo = "bar";
            requestBody.baz = "bam";
            
            dynamic responseBody = new ExpandoObject();
            requestBody.bar = "foo";
            requestBody.bam = "baz";

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
    }
}
