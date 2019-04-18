using NUnit.Framework;

namespace trx2junit.Tests
{
    [TestFixture]
    public class DummyFailure
    {
        [Test]
        public void Dummy()
        {
            Assert.Fail("demo -- must not be empty for trx2junit 1.2.3");
        }
    }
}
