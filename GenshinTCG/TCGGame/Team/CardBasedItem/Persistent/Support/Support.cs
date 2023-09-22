using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Support : AbstractPersistent<ISupport>
    {
        public Support(string nameid) : base(nameid, Registry.Instance.Supports.GetValueOrDefault(nameid))
        {
        }
        public Support(ISupport ef) : base(ef.NameID, ef)
        {
        }
    }
}
