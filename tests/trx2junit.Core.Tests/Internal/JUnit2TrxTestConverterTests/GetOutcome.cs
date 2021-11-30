using NUnit.Framework;
using trx2junit.Models;

namespace trx2junit.Tests.Internal.JUnit2TrxTestConverterTests
{
    [TestFixture]
    public class GetOutcome
    {
        [Test]
        public void All_tests_passed___Passed()
        {
            var jUnitTestSuite = new JUnitTestSuite
            {
                TestCount    = 5,
                ErrorCount   = 0,
                FailureCount = 0,
                SkippedCount = 0
            };

            TrxOutcome actual = JUnit2TrxTestConverter.GetOutcome(jUnitTestSuite);

            Assert.AreEqual(TrxOutcome.Passed, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Tests_contain_failures___Failed()
        {
            var jUnitTestSuite = new JUnitTestSuite
            {
                TestCount    = 5,
                ErrorCount   = 0,
                FailureCount = 1,
                SkippedCount = 0
            };

            TrxOutcome actual = JUnit2TrxTestConverter.GetOutcome(jUnitTestSuite);

            Assert.AreEqual(TrxOutcome.Failed, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Tests_contain_skipped___NotExecuted()
        {
            var jUnitTestSuite = new JUnitTestSuite
            {
                TestCount    = 5,
                ErrorCount   = 0,
                FailureCount = 0,
                SkippedCount = 1
            };

            TrxOutcome actual = JUnit2TrxTestConverter.GetOutcome(jUnitTestSuite);

            Assert.AreEqual(TrxOutcome.NotExecuted, actual);
        }
    }
}
