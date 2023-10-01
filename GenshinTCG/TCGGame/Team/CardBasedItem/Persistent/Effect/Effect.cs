using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Effect : AbstractPersistent<AbstractCardEffect>
    {
        public Effect(string nameid) : base(nameid, Registry.Instance.Effects.GetValueOrDefault(nameid))
        {
        }
        public Effect(AbstractCardEffect ef) : base(ef.NameID, ef)
        {
        }
    }
}
