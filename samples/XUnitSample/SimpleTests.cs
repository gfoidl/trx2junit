using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitSample
{
    public class SimpleTests
    {
        [Fact]
        public void Passing_test()
        {
            Assert.True(true);
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
