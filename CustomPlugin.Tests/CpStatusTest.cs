using System.Reflection;
using NFluent;
using NUnit.Framework;

namespace CustomPlugin.Tests
{
    public class CpStatusTest
    {
        [Test]
        public void TestOk()
        {
            var result = CpStatus.Ok();
            Check.That(result.Name).Equals(Assembly.GetEntryAssembly()?.GetName().Name);
            Check.That(result.Version).Equals(Assembly.GetEntryAssembly()?.GetName().Version);
            Check.That(result.Status).Equals("OK");
        }
        
        [Test]
        public void TestKo()
        {
            var result = CpStatus.Ko();
            Check.That(result.Name).Equals(Assembly.GetEntryAssembly()?.GetName().Name);
            Check.That(result.Version).Equals(Assembly.GetEntryAssembly()?.GetName().Version);
            Check.That(result.Status).Equals("KO");
        }
    }
}
