using trx2junit.Models;

namespace trx2junit.Abstractions
{
    public interface IJunitParser
    {
        Test Result { get; }

        void Parse();
    }
}
