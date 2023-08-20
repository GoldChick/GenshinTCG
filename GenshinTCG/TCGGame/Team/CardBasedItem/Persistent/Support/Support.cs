using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Support : AbstractPersistent<ISupport>
    {
        public Support(string nameid) : base(nameid, Registry.Instance.Supports.GetValueOrDefault(nameid))
        {
        }
    }
}
