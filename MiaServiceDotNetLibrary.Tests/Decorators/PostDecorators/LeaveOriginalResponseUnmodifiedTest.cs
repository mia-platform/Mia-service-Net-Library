using Decorators.PostDecorators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using NUnit.Framework;

namespace MiaServiceDotNetLibrary.Tests.Decorators.PostDecorators
{
    public class LeaveOriginalResponseUnmodifiedTest
    {
        private LeaveOriginalResponseUnmodified _decoratorResponse;

        [SetUp]
        public void Setup()
        {
            _decoratorResponse = new LeaveOriginalResponseUnmodified();
        }

        [Test]
        public void TestConstructor()
        {
            Check.That(_decoratorResponse.StatusCode).IsEqualTo(204);
            Check.That(_decoratorResponse.Headers["Content-Type"]).IsEqualTo("application/json; charset=utf-8");
            Check.That(_decoratorResponse.Body).IsEqualTo(null);
        }

        [Test]
        public void TestToActionResult()
        {
            var context = new DefaultHttpContext();
            var result = _decoratorResponse.ToActionResult(context);
            Check.That(result).IsInstanceOf<NoContentResult>();
        }
    }
}
