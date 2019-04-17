using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnitSample
{
    [TestFixture]
    public class DataDriven
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Two_pass_one_fails(int arg)
        {
            if (arg % 2 == 0)
                Assert.Pass();
            else
                Assert.Fail("Failing for demo purposes");
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
