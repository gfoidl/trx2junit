using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace trx2junit
{
    public class Junit2TrxConverter : TestResultXmlConverter
    {
        protected override string Extension => "trx";
        //---------------------------------------------------------------------
        public override async Task ConvertAsync(Stream? trxInput, TextWriter? jUnitOutput)
        {
            throw new NotImplementedException();
        }
    }
}
