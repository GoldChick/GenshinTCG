using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Effect : AbstractPersistent<IEffect>
    {
        public Effect(string nameid) : base(nameid, Registry.Instance.Effects.GetValueOrDefault(nameid))
        {

        }
    }
}
