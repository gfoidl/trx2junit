using System.Xml.Linq;

namespace trx2junit
{
    public interface IJUnitBuilder
    {
        XElement Result { get; }

        void Build();
    }
}
