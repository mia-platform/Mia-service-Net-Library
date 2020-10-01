using System.Collections.Generic;
using System.Dynamic;
using MiaServiceDotNetLibrary.Decorators;
using MiaServiceDotNetLibrary.Decorators.PostDecorators;
using MiaServiceDotNetLibrary.Decorators.PreDecorators;
using NFluent;
using NUnit.Framework;

namespace MiaServiceDotNetLibrary.Tests.Decorators
{
    public class Tests
    {
        private DecoratorResponseFactory _decoratorResponseFactory;

        [SetUp]
        public void Setup()
        {
            _decoratorResponseFactory = new DecoratorResponseFactory();
        }

        [Test]
        public void TestPreDecoratorLeaveOriginalRequestUnmodified()
        {
            dynamic body = new ExpandoObject();
            body.foo = "bar";
            body.baz = "bam";

            var preDecoratorRequest = new PreDecoratorRequest
            {
                Method = "GET",
                Path = "test",
                Headers = new Dictionary<string, string> {{"foo", "bar"}},
                Body = body
            };

            var result =
                _decoratorResponseFactory.MakePreDecoratorResponse(preDecoratorRequest
                    .LeaveOriginalRequestUnmodified());

            Check.That(result).IsInstanceOf<LeaveOriginalRequestUnmodified>();
        }

        [Test]
        public void TestPreDecoratorChangeOriginalRequest()
        {
            dynamic body = new ExpandoObject();
            body.foo = "bar";
            body.baz = "bam";

            var preDecoratorRequest = new PreDecoratorRequest
            {
                Method = "GET",
                Path = "test",
                Headers = new Dictionary<string, string> {{"foo", "bar"}},
                Query = new Dictionary<string, string> {{"baz", "bam"}},
                Body = body
            };

            var newRequest = preDecoratorRequest.ChangeOriginalRequest()
                .Query(new Dictionary<string, string> {{"foo", "bar"}})
                .Change();

            var result =
                _decoratorResponseFactory.MakePreDecoratorResponse(newRequest);

            Check.That(result).IsInstanceOf<ChangeOriginalRequest>();
        }

        [Test]
        public void TestPostDecoratorLeaveOriginalResponseUnmodified()
        {
            dynamic requestBody = new ExpandoObject();
            requestBody.foo = "bar";
            requestBody.baz = "bam";

            dynamic responseBody = new ExpandoObject();
            requestBody.bar = "foo";
            requestBody.bam = "baz";

            var postDecoratorRequest = new PostDecoratorRequest
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
            
            
            var result =
                _decoratorResponseFactory.MakePostDecoratorResponse(postDecoratorRequest.LeaveOriginalResponseUnmodified());

            Check.That(result).IsInstanceOf<LeaveOriginalResponseUnmodified>();
        }

        [Test]
        public void TestPostDecoratorChangeOriginalResponse()
        {
            dynamic requestBody = new ExpandoObject();
            requestBody.foo = "bar";
            requestBody.baz = "bam";

            dynamic responseBody = new ExpandoObject();
            requestBody.bar = "foo";
            requestBody.bam = "baz";

            var postDecoratorRequest = new PostDecoratorRequest
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
            
            var newRequest = postDecoratorRequest.ChangeOriginalResponse()
                .StatusCode(201)
                .Headers(new Dictionary<string, string> {{"foo", "bar"}})
                .Change();

            var result =
                _decoratorResponseFactory.MakePostDecoratorResponse(newRequest);

            Check.That(result).IsInstanceOf<ChangeOriginalResponse>();
        }
    }
}
