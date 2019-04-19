using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsTestSample
{
    [TestClass]
    public class SimpleTests
    {
        [TestMethod]
        public void Passing_test()
        {
        }
        //---------------------------------------------------------------------
        [TestMethod]
        public void Failing_test()
        {
            // This will cause the CI-test-scripts to fail the build.
            // So use 'set +e' in the test-scripts to avoid the fail -- here
            // it is just for demonstration.
            Assert.Fail("Failing for demo purposes");
        }
        //---------------------------------------------------------------------
        [TestMethod]
        public async Task Slow_test()
        {
            await Task.Delay(1000);
        }
        //---------------------------------------------------------------------
        [TestMethod]
        [Ignore("Ignoring for testing ;-)")]
        public void Ignored_test()
        {
            throw new System.Exception();
        }
    }
}
