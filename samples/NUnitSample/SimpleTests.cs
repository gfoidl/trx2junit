using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnitSample
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
        //---------------------------------------------------------------------
        [Test]
        [Ignore("Ignoring for testing ;-)")]
        public void Ignored_test()
        {
            throw new System.Exception();
        }
        //---------------------------------------------------------------------
        [Test]
        public void Test_with_failed_assumption()
        {
            Assume.That(false);

            throw new System.Exception();
        }
    }
}
