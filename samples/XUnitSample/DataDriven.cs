using System.Threading.Tasks;
using Xunit;

namespace XUnitSample
{
    public class DataDriven
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Two_pass_one_fails(int arg)
        {
            if (arg % 2 == 0)
                Assert.True(true);
            else
                Assert.True(false, "Failing for demo purposes");
        }
        //---------------------------------------------------------------------
        [Fact]
        public void Failing_test()
        {
            // This will cause the CI-test-scripts to fail the build.
            // So use 'set +e' in the test-scripts to avoid the fail -- here
            // it is just for demonstration.
            Assert.True(false, "Failing for demo purposes");
        }
        //---------------------------------------------------------------------
        [Fact]
        public Task Slow_test()
        {
            return Task.Delay(1000);
        }
    }
}
