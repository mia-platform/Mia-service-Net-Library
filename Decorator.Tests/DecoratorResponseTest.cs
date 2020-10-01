using System.Collections.Generic;
using System.Dynamic;
using Decorators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using NFluent;
using NUnit.Framework;

namespace Decorator.Tests
{
    public class DecoratorResponseTest
    {
        private DecoratorResponse _decoratorResponse;
        
        [SetUp]
        public void SetUp()
        {
            dynamic body = new ExpandoObject();
            body.test = "bla";

            _decoratorResponse = new DecoratorResponse
            {
                StatusCode = 200,
                Headers = new Dictionary<string, string> {{"foo", "bar"}},
                Body = body
            };
        }
        
        [Test]
        public void TestToExpandoObject()
        {
            dynamic result = _decoratorResponse.ToExpandoObject();

            Check.That(result.statusCode).Equals("GET");
            Check.That(result.headers).Equals(new Dictionary<string, string> {{"foo", "bar"}});
            Check.That(result.body.test).Equals("bla");
        }
    }
}
