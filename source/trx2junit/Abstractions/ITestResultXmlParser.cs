using trx2junit.Models;

namespace trx2junit
{
    public interface ITestResultXmlParser
    {
        Test Result { get; }

        void Parse();
    }
}
