using System;
using NUnit.Framework;
using trx2junit.Models;

namespace trx2junit.Tests.Internal.JUnit2TrxTestConverterTests
{
    [TestFixture]
    public class SetCreationTimestamp
    {
        [Test]
        public void TrxTimes_Creation_is_null___TimeStamp_set()
        {
            var trxTimes       = new TrxTimes();
            var junitTestSuite = new JUnitTestSuite { TimeStamp = DateTime.Now };

            JUnit2TrxTestConverter.SetCreationTimestamp(junitTestSuite, trxTimes);

            Assert.AreEqual(junitTestSuite.TimeStamp, trxTimes.Creation);
        }
        //---------------------------------------------------------------------
        [Test]
        public void TrxTimes_Creation_has_value_TimeStamp_is_smaller___TimeStamp_set()
        {
            var trxTimes       = new TrxTimes       { Creation  = DateTime.Now };
            var junitTestSuite = new JUnitTestSuite { TimeStamp = DateTime.Now.AddSeconds(-1) };

            JUnit2TrxTestConverter.SetCreationTimestamp(junitTestSuite, trxTimes);

            Assert.AreEqual(junitTestSuite.TimeStamp, trxTimes.Creation.Value);
        }
        //---------------------------------------------------------------------
        [Test]
        public void TrxTimes_Creation_has_value_TimeStamp_is_greater___nop()
        {
            DateTime now       = DateTime.Now;
            var trxTimes       = new TrxTimes       { Creation  = now };
            var junitTestSuite = new JUnitTestSuite { TimeStamp = DateTime.Now.AddSeconds(1) };

            JUnit2TrxTestConverter.SetCreationTimestamp(junitTestSuite, trxTimes);

            Assert.AreEqual(now, trxTimes.Creation.Value);
        }
    }
}
