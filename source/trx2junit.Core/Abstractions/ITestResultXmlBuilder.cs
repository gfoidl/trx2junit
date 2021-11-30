// (c) gfoidl, all rights reserved

using System.Xml.Linq;

namespace gfoidl.Trx2Junit.Core.Abstractions;

internal interface ITestResultXmlBuilder<TTest> where TTest : Models.Test
{
    TTest Test      { get; }
    XElement Result { get; }
    //-------------------------------------------------------------------------
    void Build();
}
