using trx2junit.Models;

namespace trx2junit
{
    public interface ITrxParser
    {
        Test Result { get; }

        void Parse();
    }
}
