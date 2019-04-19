using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsTestSample
{
    [TestClass]
    public class DataDriven
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void Two_pass_one_fails(int arg)
        {
            if (arg % 2 != 0)
                Assert.Fail("Failing for demo purposes");
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
    }
}
