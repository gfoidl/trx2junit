// (c) gfoidl, all rights reserved

using System.Collections.Generic;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Internal.TypeInfoHelperTests;

[TestFixture]
public class StripTypeInfo
{
    [Test, TestCaseSource(nameof(Name_given___correct_Name_returned_TestCases))]
    public string Name_given___correct_Name_returned(string name)
    {
        return name.StripTypeInfo();
    }
    //-------------------------------------------------------------------------
    private static IEnumerable<TestCaseData> Name_given___correct_Name_returned_TestCases()
    {
        yield return new TestCaseData(null).Returns(null);
        yield return new TestCaseData("").Returns("");

        yield return new TestCaseData("Method1").Returns("Method1");
        yield return new TestCaseData("Method1(arg: { a = 3 })").Returns("Method1(arg: { a = 3 })");

        yield return new TestCaseData("Class1.Method1").Returns("Method1");
        yield return new TestCaseData("Class1.Method1(arg: { a = 3 })").Returns("Method1(arg: { a = 3 })");

        yield return new TestCaseData("SimpleUnitTest.Class1.Method1").Returns("Method1");
        yield return new TestCaseData("SimpleUnitTest.Class1.Method1(arg: { a = 3 })").Returns("Method1(arg: { a = 3 })");
    }
}
