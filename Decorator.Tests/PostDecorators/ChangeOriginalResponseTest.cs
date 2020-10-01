using System.Collections.Generic;
using System.Dynamic;
using Decorators;
using Decorators.PostDecorators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using NUnit.Framework;

namespace Decorator.Tests.PostDecorators
{
    public class ChangeOriginalResponseTest
    {
        private ChangeOriginalResponse _decoratorResponse;
        private DecoratorResponse _newResponse;

        [SetUp]
        public void Setup()
        {
            dynamic newBody = new ExpandoObject();
            newBody.foo = "bar";
            newBody.baz = "bam";

            _newResponse = new DecoratorResponse
            {
                StatusCode = 200,
                Headers = new Dictionary<string, string> {{"foo", "bar"}},
                Body = newBody
            };

            _decoratorResponse = new ChangeOriginalResponse(_newResponse);
        }

        [Test]
        public void TestConstructor()
        {
            Check.That(_decoratorResponse.StatusCode).IsEqualTo(200);
            Check.That(_decoratorResponse.Headers["Content-Type"]).IsEqualTo("application/json; charset=utf-8");
            Check.That(_decoratorResponse.Body).IsEqualTo(_newResponse.ToExpandoObject());
        }

        [Test]
        public void TestToActionResult()
        {
            var context = new DefaultHttpContext();
            var result = (ContentResult) _decoratorResponse.ToActionResult(context);
            Check.That(result).IsInstanceOf<ContentResult>();
            Check.That(result.StatusCode).IsEqualTo(200);
            Check.That(result.Content)
                .IsEqualTo(
                    @"{""statusCode"":200,""headers"":{""foo"":""bar""},""body"":{""foo"":""bar"",""baz"":""bam""}}");
        }
    }
}
