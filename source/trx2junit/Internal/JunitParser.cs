using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class JunitParser : ITestResultXmlParser
    {
        private readonly XElement _junit;
        private readonly Test _test = new Test();
        //---------------------------------------------------------------------
        public JunitParser(XElement? junit) => _junit = junit ?? throw new ArgumentNullException(nameof(junit));
        //---------------------------------------------------------------------
        public Test Result => throw new NotImplementedException();

        public void Parse() => throw new NotImplementedException();
    }
}
