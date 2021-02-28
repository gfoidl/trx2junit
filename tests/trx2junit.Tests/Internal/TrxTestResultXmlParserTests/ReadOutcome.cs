using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.TrxTestResultXmlParserTests
{
    [TestFixture]
    public class ReadOutcome
    {
        [Test]
        public void Null_or_empty_and_outcome_is_not_required___null([Values(null, "")] string value)
        {
            TrxOutcome? actual = TrxTestResultXmlParser.ReadOutcome(value, false);

            Assert.IsFalse(actual.HasValue);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Null_or_empty_and_outcome_is_required___throws_Exception([Values(null, "")] string value)
        {
            var ex = Assert.Throws<Exception>(() => TrxTestResultXmlParser.ReadOutcome(value));

            Assert.AreEqual("outcome is required according the xml-schema", ex.Message);
        }
        //---------------------------------------------------------------------
        [Test, TestCaseSource(nameof(Value_given___correct_outcome_parsed_TestCases))]
        public TrxOutcome? Value_given___correct_outcome_parsed(string value, bool isRequired)
        {
            return TrxTestResultXmlParser.ReadOutcome(value, isRequired);
        }
        //---------------------------------------------------------------------
        private static IEnumerable<TestCaseData> Value_given___correct_outcome_parsed_TestCases()
        {
            foreach (string value in Enum.GetNames(typeof(TrxOutcome)))
            {
                TrxOutcome expected = Enum.Parse<TrxOutcome>(value);
                yield return new TestCaseData(value, true).Returns(expected);
                yield return new TestCaseData(value, false).Returns(expected);
            }
        }
    }
}
