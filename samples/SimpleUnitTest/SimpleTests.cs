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
            // This will cause the CI-test-scripts to fail the build.
            // So use 'set +e' in the test-scripts to avoid the fail -- here
            // it is just for demonstration.
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
