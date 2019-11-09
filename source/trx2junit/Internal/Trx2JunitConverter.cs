﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace trx2junit
{
    public class Trx2JunitConverter : TestResultXmlConverter
    {
        protected override string Extension => "xml";
        //---------------------------------------------------------------------
        public override async Task ConvertAsync(Stream? trxInput, TextWriter? jUnitOutput)
        {
            XElement trx = await XElement.LoadAsync(trxInput, LoadOptions.None, CancellationToken.None);

            var trxParser = new TrxParser(trx);
            trxParser.Parse();

            var jUnitBuilder = new JUnitBuilder(trxParser.Result);
            jUnitBuilder.Build();

            await jUnitBuilder.Result.SaveAsync(jUnitOutput, SaveOptions.None, CancellationToken.None);
        }
    }
}
