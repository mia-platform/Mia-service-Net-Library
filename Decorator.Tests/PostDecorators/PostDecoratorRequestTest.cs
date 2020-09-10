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

            _postDecoratorRequest = new PostDecoratorRequest()
            {
                Request = new DecoratorRequest
                {
                    Method = "GET",
                    Path = "test",
                    Headers = new Dictionary<string, string> {{"foo", "bar"}},
                    Query = new Dictionary<string, string> {{"baz", "bam"}},
                    Body = requestBody
                },
                Response =
                {
                    StatusCode = 200,
                    Headers = new Dictionary<string, string> {{"bar", "foo"}},
                    Body = responseBody
                }
            };
        }

        [Test]
        public void TestChangeOriginalResponse()
        {
            var newResponse = _postDecoratorRequest.ChangeOriginalResponse();

            Check.That(newResponse).IsEqualTo(null);
        }

        [Test]
        public void TestOriginalRequestGetsModified()
        {
            dynamic newBody = new ExpandoObject();
            newBody.foo = 42;

            var newRequest = _postDecoratorRequest
                .ChangeOriginalResponse()
                .StatusCode(201)
                .Headers(new Dictionary<string, string> {{"new", "header"}})
                .Body(newBody)
                .Change();

            Check.That(((PostDecoratorRequest) newRequest).Response.StatusCode).IsEqualTo(201);
           // Assert.IsFalse(((PostDecoratorRequest) newRequest).Response.Headers.Equals(_postDecoratorRequest.Body));
            //Assert.IsFalse(((PostDecoratorRequest) newRequest).Headers.Equals(_postDecoratorRequest.Headers));
        }
    }
}
