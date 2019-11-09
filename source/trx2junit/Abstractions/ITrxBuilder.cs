using System.Xml.Linq;

namespace trx2junit.Abstractions
{
    public interface ITrxBuilder
    {
        XElement Result { get; }

        void Build();
    }
}
