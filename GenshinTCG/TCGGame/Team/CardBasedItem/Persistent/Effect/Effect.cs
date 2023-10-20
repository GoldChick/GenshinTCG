using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Effect : AbstractPersistent<AbstractCardPersistentEffect>
    {
        public Effect(string nameid) : base(nameid, Registry.Instance.Effects.GetValueOrDefault(nameid))
        {
        }
        public Effect(AbstractCardPersistentEffect ef, AbstractPersistent? bind=null) : base(ef.NameID, ef,bind)
        {
        }
    }
}
