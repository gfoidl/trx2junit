using trx2junit.Models;

#nullable enable

namespace trx2junit
{
    public interface ITrxParser
    {
        Test Result { get; }

        void Parse();
    }
}
