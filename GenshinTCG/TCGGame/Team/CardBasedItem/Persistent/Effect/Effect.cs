using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Effect : AbstractPersistent<AbstractCardPersistentEffect>
    {
        public Effect(string nameid) : base(nameid, Registry.Instance.Effects.GetValueOrDefault(nameid))
        {
        }
        public Effect(AbstractCardPersistentEffect ef) : base(ef.NameID, ef)
        {
        }
    }
}
