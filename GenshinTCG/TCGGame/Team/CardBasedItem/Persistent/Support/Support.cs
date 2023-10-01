using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Support : AbstractPersistent<AbstractCardSupport>
    {
        public Support(string nameid) : base(nameid, Registry.Instance.Supports.GetValueOrDefault(nameid))
        {
        }
        public Support(AbstractCardSupport ef) : base(ef.NameID, ef)
        {
        }
    }
}
