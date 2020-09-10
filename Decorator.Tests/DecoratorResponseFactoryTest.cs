using System.Collections.Generic;
using System.Dynamic;
using Decorators;
using Decorators.PreDecorators;
using NFluent;
using NUnit.Framework;

namespace Decorator.Tests
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
    }
}
