using System;
using NUnit.Framework;

namespace trx2junit.Tests
{
    [TestFixture]
    public class DummyTests
    {
        [Test]
        [Ignore("For demo")]
        public void Ignored_test_for_demo_in_CI()
        {
            throw new Exception();
        }
        //---------------------------------------------------------------------
        [Test]
        public void Test_with_unmet_assumption_for_demo_in_CI()
        {
            Assume.That(false, "unmet assumption for demo");

            throw new Exception();
        }
    }
}
