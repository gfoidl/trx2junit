using System.IO;
using System.Threading.Tasks;

namespace trx2junit
{
    public interface ITestResultXmlConverter
    {
        Task ConvertAsync(Stream input, TextWriter output);
        string GetOutputFile(string inputFile, string? outputPath = null);
    }
}
