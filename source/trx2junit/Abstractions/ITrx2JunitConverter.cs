using System.IO;
using System.Threading.Tasks;

#nullable enable

namespace trx2junit
{
    public interface ITrx2JunitConverter
    {
        Task Convert(Stream? trxInput, TextWriter? jUnitOutput);
    }
}
