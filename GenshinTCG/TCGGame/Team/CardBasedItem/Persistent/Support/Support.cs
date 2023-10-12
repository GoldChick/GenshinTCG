using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Support : AbstractPersistent<AbstractCardPersistentSupport>
    {
        public Support(string nameid) : base(nameid, Registry.Instance.Supports.GetValueOrDefault(nameid))
        {
        }
        public Support(AbstractCardPersistentSupport ef) : base(ef.NameID, ef)
        {
        }
    }
}
