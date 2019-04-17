using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace trx2junit.Tests.Internal.JUnitBuilderTests.Build
{
    public abstract class Base
    {
        protected readonly Models.Test _testData = new Models.Test();
        //---------------------------------------------------------------------
        protected List<XElement> GetTestSuites()
        {
            var builder = new JUnitBuilder(_testData);

            builder.Build();

            return builder.Result.Elements("testsuite").ToList();
        }
    }
}
