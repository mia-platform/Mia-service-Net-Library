using System.Collections.Generic;
using System.Dynamic;
using Decorators.PreDecorators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using NUnit.Framework;

namespace Decorator.Tests.PreDecorators
{
    public class ChangeOriginalRequestTest
    {
        private ChangeOriginalRequest _decoratorResponse;
        [SetUp]
        public void Setup()
        {
            dynamic newBody = new ExpandoObject();
            newBody.foo = "bar";
            newBody.baz = "bam";
            
            var newRequest = new PreDecoratorRequest
            {
                Method = "GET",
                Path = "test",
                Headers = new Dictionary<string, string> {{"foo", "bar"}},
                Query = new Dictionary<string, string> {{"baz", "bam"}},
                Body = newBody
            };
            
            _decoratorResponse = new ChangeOriginalRequest(newRequest);
        }
        
        [Test]
        public void TestConstructor()
        {
            Check.That(_decoratorResponse.StatusCode).IsEqualTo(200);
            Check.That(_decoratorResponse.Headers["Content-Type"]).IsEqualTo("application/json; charset=utf-8");
            Check.That(_decoratorResponse.Body).IsEqualTo(_decoratorResponse.Body);
        }
        
        [Test]
        public void TestToActionResult()
        {
            var context = new DefaultHttpContext();
            var result = (ContentResult) _decoratorResponse.ToActionResult(context);
            Check.That(result).IsInstanceOf<ContentResult>();
            Check.That(result.StatusCode).IsEqualTo(200);
            Check.That(result.Content).IsEqualTo(@"{""method"":""GET"",""path"":""test"",""headers"":{""foo"":""bar""},""query"":{""baz"":""bam""},""body"":{""foo"":""bar"",""baz"":""bam""}}");
        }
    }
}
