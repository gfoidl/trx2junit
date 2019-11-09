using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class TrxBuilder : ITestResultXmlBuilder
    {
        private readonly Test _test;
        //---------------------------------------------------------------------
        public TrxBuilder(Test? test) => _test = test ?? throw new ArgumentNullException(nameof(test));
        //---------------------------------------------------------------------

        public XElement Result => throw new NotImplementedException();

        public void Build() => throw new NotImplementedException();
    }
}
