using System.IO;
using System.Threading.Tasks;

namespace trx2junit
{
    public interface ITrx2JunitConverter
    {
        Task Convert(Stream? trxInput, TextWriter? jUnitOutput);
    }
}
