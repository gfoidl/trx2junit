using System.Xml.Linq;

namespace trx2junit
{
    public interface ITrxBuilder
    {
        XElement Result { get; }

        void Build();
    }
}
