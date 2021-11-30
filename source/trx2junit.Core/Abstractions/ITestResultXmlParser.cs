using trx2junit.Models;

namespace trx2junit
{
    public interface ITestResultXmlParser<TTest> where TTest : Test
    {
        TTest Result { get; }

        void Parse();
    }
}
