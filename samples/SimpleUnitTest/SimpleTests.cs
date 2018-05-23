using System.Threading.Tasks;
using NUnit.Framework;

namespace SimpleUnitTest
{
    [TestFixture]
    public class SimpleTests
    {
        [Test]
        public void Passing_test()
        {
            Assert.Pass();
        }
        //---------------------------------------------------------------------
        [Test]
        public void Failing_test()
        {
            Assert.Fail("Failing for demo purposes");
        }
        //---------------------------------------------------------------------
        [Test]
        public Task Slow_test()
        {
            return Task.Delay(1000);
        }
    }
}
