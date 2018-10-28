using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

#if NETCOREAPP2_1
using System.Threading;
#endif

namespace trx2junit
{
    public class Trx2JunitConverter : ITrx2JunitConverter
    {
#if !NETCOREAPP2_1
#pragma warning disable CS1998
#endif
        public async Task Convert(Stream trxInput, TextWriter jUnitOutput)
        {
#if NETCOREAPP2_1
            XElement trx = await XElement.LoadAsync(trxInput, LoadOptions.None, CancellationToken.None);
#else
            XElement trx = XElement.Load(trxInput);
#endif
            var trxParser = new TrxParser(trx);
            trxParser.Parse();

            var jUnitBuilder = new JUnitBuilder(trxParser.Result);
            jUnitBuilder.Build();
#if NETCOREAPP2_1
            await jUnitBuilder.Result.SaveAsync(jUnitOutput, SaveOptions.None, CancellationToken.None);
#else
            jUnitBuilder.Result.Save(jUnitOutput, SaveOptions.None);
#endif
        }
#if !NETCOREAPP2_1
#pragma warning restore CS1998
#endif
    }
}
