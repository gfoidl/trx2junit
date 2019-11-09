using trx2junit.Models;

namespace trx2junit
{
    public interface IJunitParser
    {
        Test Result { get; }

        void Parse();
    }
}
