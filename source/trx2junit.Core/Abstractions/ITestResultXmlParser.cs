// (c) gfoidl, all rights reserved

using gfoidl.Trx2Junit.Core.Models;

namespace gfoidl.Trx2Junit.Core.Abstractions;

internal interface ITestResultXmlParser<TTest> where TTest : Test
{
    TTest Result { get; }
    //-------------------------------------------------------------------------
    void Parse();
}
