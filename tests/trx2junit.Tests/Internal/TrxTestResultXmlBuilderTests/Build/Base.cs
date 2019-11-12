using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace trx2junit.Tests.Internal.TrxTestResultXmlBuilderTests.Build
{
    public abstract class Base
    {
        protected readonly Models.TrxTest _testData = new Models.TrxTest();
        //---------------------------------------------------------------------
        protected List<XElement> GetFoo()
        {
            var builder = new TrxTestResultXmlBuilder(_testData);

            builder.Build();

            return builder.Result.Elements("foo").ToList();
        }
    }
}
