using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Effect : AbstractPersistent<IEffect>
    {
        public Effect(string nameid) : base(nameid, Registry.Instance.Effects.GetValueOrDefault(nameid))
        {
        }
        public Effect(IEffect ef) : base(ef.NameID, ef)
        {
        }
    }
}
