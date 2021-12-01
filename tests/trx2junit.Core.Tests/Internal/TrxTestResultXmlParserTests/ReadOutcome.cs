// (c) gfoidl, all rights reserved

using System;
using System.Collections.Generic;
using gfoidl.Trx2Junit.Core.Internal;
using gfoidl.Trx2Junit.Core.Models.Trx;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Internal.TrxTestResultXmlParserTests;

[TestFixture]
public class ReadOutcome
{
    [Test]
    public void Null_or_empty_and_outcome_is_not_required___null([Values(null, "")] string value)
    {
        TrxOutcome? actual = TrxTestResultXmlParser.ReadOutcome(value, false);

        Assert.IsFalse(actual.HasValue);
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Null_or_empty_and_outcome_is_required___throws_Exception([Values(null, "")] string value)
    {
        var ex = Assert.Throws<Exception>(() => TrxTestResultXmlParser.ReadOutcome(value));

        Assert.AreEqual("outcome is required according the xml-schema", ex.Message);
    }
    //-------------------------------------------------------------------------
    [Test, TestCaseSource(nameof(Value_given___correct_outcome_parsed_TestCases))]
    public int? Value_given___correct_outcome_parsed(string value, bool isRequired)
    {
        TrxOutcome? actual = TrxTestResultXmlParser.ReadOutcome(value, isRequired);

        return actual.HasValue
            ? (int)actual
            : null;
    }
    //-------------------------------------------------------------------------
    private static IEnumerable<TestCaseData> Value_given___correct_outcome_parsed_TestCases()
    {
        foreach (string value in Enum.GetNames(typeof(TrxOutcome)))
        {
            TrxOutcome tmp = Enum.Parse<TrxOutcome>(value);
            int expected   = (int)tmp;

            yield return new TestCaseData(value, true) .Returns(expected);
            yield return new TestCaseData(value, false).Returns(expected);
        }
    }
}
