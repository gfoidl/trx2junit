using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnitSample
{
    public class MemberData
    {
        private static IEnumerable<object[]> TestData = new[]
        {
            new object[] { new { index = 0 } },
            new object[] { new { index = 1 } },
            new object[] { new { index = 2 } }
        };
        //---------------------------------------------------------------------
        [Test, TestCaseSource(nameof(TestData))]
        public void Two_pass_one_fails(dynamic obj)
        {
            if (obj.index % 2 == 0)
                Assert.True(true);
            else
                Assert.True(false, "Failing for demo purposes");
        }
        //---------------------------------------------------------------------
        [Test]
        public void Failing_test()
        {
            // This will cause the CI-test-scripts to fail the build.
            // So use 'set +e' in the test-scripts to avoid the fail -- here
            // it is just for demonstration.
            Assert.True(false, "Failing for demo purposes");
        }
        //---------------------------------------------------------------------
        [Test]
        public Task Slow_test()
        {
            return Task.Delay(1000);
        }
    }
}
