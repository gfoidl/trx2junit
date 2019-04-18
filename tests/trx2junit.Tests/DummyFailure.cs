using NUnit.Framework;

namespace trx2junit.Tests
{
    [TestFixture]
    public class DummyFailure
    {
        [Test]
        public void Dummy()
        {
            Assert.Fail();
        }
    }
}
