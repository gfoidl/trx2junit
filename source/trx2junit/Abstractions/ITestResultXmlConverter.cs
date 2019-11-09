using System.IO;
using System.Threading.Tasks;

namespace trx2junit
{
    public interface ITestResultXmlConverter
    {
        Task Convert(Stream? input, TextWriter? output);
    }
}
