using System.Collections.Generic;
using System.ComponentModel.Design;
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
        public void TestLeaveOriginalRequestUnmodified()
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
            
            Check.That(result.StatusCode).IsEqualTo(204);
            Check.That(result.Headers["Content-Type"]).IsEqualTo("application/json; charset=utf-8");
            Check.That(result.Body).IsEqualTo(null);
        }
    }
}
