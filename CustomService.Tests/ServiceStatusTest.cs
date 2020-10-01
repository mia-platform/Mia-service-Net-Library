using System.Reflection;
using NFluent;
using NUnit.Framework;

namespace CustomService.Tests
{
    public class ServiceStatusTest
    {
        [Test]
        public void TestOk()
        {
            var result = ServiceStatus.Ok();
            Check.That(result.Name).Equals(Assembly.GetEntryAssembly()?.GetName().Name);
            Check.That(result.Version).Equals(Assembly.GetEntryAssembly()?.GetName().Version?.ToString());
            Check.That(result.Status).Equals("OK");
        }
        
        [Test]
        public void TestKo()
        {
            var result = ServiceStatus.Ko();
            Check.That(result.Name).Equals(Assembly.GetEntryAssembly()?.GetName().Name);
            Check.That(result.Version).Equals(Assembly.GetEntryAssembly()?.GetName().Version?.ToString());
            Check.That(result.Status).Equals("KO");
        }
    }
}
