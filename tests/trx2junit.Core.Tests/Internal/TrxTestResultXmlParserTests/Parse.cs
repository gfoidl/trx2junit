// (c) gfoidl, all rights reserved

using System.Linq;
using System.Xml.Linq;
using gfoidl.Trx2Junit.Core.Internal;
using gfoidl.Trx2Junit.Core.Models.Trx;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Internal.TrxTestResultXmlParserTests;

[TestFixture]
public class Parse
{
    [Test]
    [TestCase("./data/trx/mstest.trx"                     , 3, 3)]
    [TestCase("./data/trx/mstest-datadriven.trx"          , 5, 3)]
    [TestCase("./data/trx/mstest-ignore.trx"              , 4, 4)]
    [TestCase("./data/trx/nunit.trx"                      , 3, 3)]
    [TestCase("./data/trx/nunit-datadriven.trx"           , 5, 5)]
    [TestCase("./data/trx/nunit-ignore.trx"               , 5, 5)]
    [TestCase("./data/trx/nunit-memberdata.trx"           , 5, 5)]
    [TestCase("./data/trx/xunit.trx"                      , 3, 3)]
    [TestCase("./data/trx/xunit-datadriven.trx"           , 5, 5)]
    [TestCase("./data/trx/xunit-ignore.trx"               , 4, 4)]
    [TestCase("./data/trx/xunit-memberdata.trx"           , 5, 3)]
    [TestCase("./data/trx/nunit-testresultaggregation.trx", 3, 3)]
    public void File_given___correct_counts(string trxFile, int expectedUnitTestResultsCount, int expectedTestDefinitionsCount)
    {
        XElement trx = XElement.Load(trxFile);
        var sut      = new TrxTestResultXmlParser(trx);

        sut.Parse();
        TrxTest actual = sut.Result;

        Assert.Multiple(() =>
        {
            Assert.AreEqual(expectedUnitTestResultsCount, actual.UnitTestResults.Count, nameof(expectedUnitTestResultsCount));
            Assert.AreEqual(expectedTestDefinitionsCount, actual.TestDefinitions.Count, nameof(expectedTestDefinitionsCount));
        });
    }
    //-------------------------------------------------------------------------
    [Test]
    [TestCase("./data/trx/mstest.trx"                       , 3, 1)]
    [TestCase("./data/trx/mstest-datadriven.trx"            , 5, 2)]
    [TestCase("./data/trx/mstest-ignore.trx"                , 4, 1)]
    [TestCase("./data/trx/mstest-datadriven-no-failures.trx", 5, 0)]
    [TestCase("./data/trx/nunit.trx"                        , 3, 1)]
    [TestCase("./data/trx/nunit-datadriven.trx"             , 5, 2)]
    [TestCase("./data/trx/nunit-ignore.trx"                 , 5, 1)]
    [TestCase("./data/trx/nunit-memberdata.trx"             , 5, 2)]
    [TestCase("./data/trx/xunit.trx"                        , 3, 1)]
    [TestCase("./data/trx/xunit-datadriven.trx"             , 5, 2)]
    [TestCase("./data/trx/xunit-ignore.trx"                 , 4, 1)]
    [TestCase("./data/trx/xunit-memberdata.trx"             , 5, 2)]
    [TestCase("./data/trx/nunit-testresultaggregation.trx"  , 3, 1)]
    public void File_given___correct_ResultSummary_total_failed(string trxFile, int expectedTotalCount, int expectedFailedCount)
    {
        XElement trx = XElement.Load(trxFile);
        var sut      = new TrxTestResultXmlParser(trx);

        sut.Parse();
        TrxTest actual = sut.Result;

        Assert.Multiple(() =>
        {
            Assert.AreEqual(expectedTotalCount , actual.ResultSummary.Total , nameof(expectedTotalCount));
            Assert.AreEqual(expectedFailedCount, actual.ResultSummary.Failed, nameof(expectedFailedCount));
        });
    }
    //-------------------------------------------------------------------------
    [Test]
    [TestCase("./data/trx/nunit-datadriven.trx", "Two_pass_one_fails(2)")]
    [TestCase("./data/trx/nunit-memberdata.trx", "Two_pass_one_fails({ index = 1 })")]
    [TestCase("./data/trx/xunit-datadriven.trx", "Two_pass_one_fails(arg: 0)")]
    public void File_given___corrent_TestName(string trxFile, string expectedName)
    {
        XElement trx = XElement.Load(trxFile);
        var sut      = new TrxTestResultXmlParser(trx);

        sut.Parse();
        TrxTest actual = sut.Result;

        string actualTestName = actual.UnitTestResults
            .First(u => u.TestName.Contains("Two_pass_one_fails"))
            .TestName;

        Assert.AreEqual(expectedName, actualTestName);
    }
}
