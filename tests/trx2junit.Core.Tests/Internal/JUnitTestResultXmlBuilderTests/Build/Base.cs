// (c) gfoidl, all rights reserved

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using gfoidl.Trx2Junit.Core.Internal;
using gfoidl.Trx2Junit.Core.Models.JUnit;
using gfoidl.Trx2Junit.Core.Models.Trx;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Internal.JUnitTestResultXmlBuilderTests.Build;

[TestFixture]
public abstract class Base
{
    private protected readonly TrxTest _trxTest = new();
    private protected JUnitTest        _junitTest;
    //-------------------------------------------------------------------------
    protected List<XElement> GetTestSuites()
    {
        var builder = new JUnitTestResultXmlBuilder(_junitTest);

        builder.Build();

        return builder.Result.Elements("testsuite").ToList();
    }
}
