using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace trx2junit
{
    public class Junit2TrxConverter : ITestResultXmlConverter
    {
        public async Task Convert(Stream? trxInput, TextWriter? jUnitOutput)
        {
            throw new NotImplementedException();
        }
    }
}
