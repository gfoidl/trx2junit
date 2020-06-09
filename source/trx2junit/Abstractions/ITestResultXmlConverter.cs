using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace trx2junit
{
    public interface ITestResultXmlConverter
    {
        void ValidateAgainstSchema(string? inputFile);
        Task ConvertAsync(Stream? input, TextWriter? output);
        string GetOutputFile(string? inputFile, string? outputPath = null);
    }
}
