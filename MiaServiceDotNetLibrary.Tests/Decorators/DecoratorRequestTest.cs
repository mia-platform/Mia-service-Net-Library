using System.Collections.Generic;
using System.Dynamic;
using Decorators;
using NFluent;
using NUnit.Framework;

namespace MiaServiceDotNetLibrary.Tests.Decorators
{
    public class DecoratorRequestTest
    {
        private DecoratorRequest _decoratorRequest;
        
        [SetUp]
        public void SetUp()
        {
            dynamic body = new ExpandoObject();
            body.test = "bla";

            _decoratorRequest = new DecoratorRequest
            {
                Method = "GET",
                Path = "test",
                Headers = new Dictionary<string, string> {{"foo", "bar"}},
                Query = new Dictionary<string, string> {{"baz", "bam"}},
                Body = body
            };
        }
        
        [Test]
        public void TestToExpandoObject()
        {
            dynamic result = _decoratorRequest.ToExpandoObject();

            Check.That(result.method).Equals("GET");
            Check.That(result.path).Equals("test");
            Check.That(result.headers).Equals(new Dictionary<string, string> {{"foo", "bar"}});
            Check.That(result.query).Equals(new Dictionary<string, string> {{"baz", "bam"}});
            Check.That(result.body.test).Equals("bla");
        }
    }
}
