using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace trx2junit.Tests.Internal.JUnitTestResultXmlBuilderTests.Build
{
    public abstract class Base
    {
        protected readonly Models.TrxTest _trxTest = new Models.TrxTest();
        protected Models.JUnitTest        _junitTest;
        //---------------------------------------------------------------------
        protected List<XElement> GetTestSuites()
        {
            var builder = new JUnitTestResultXmlBuilder(_junitTest);

            builder.Build();

            return builder.Result.Elements("testsuite").ToList();
        }
    }
}
