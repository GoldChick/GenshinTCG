using TCGBase;
using TCGRule;

namespace TCGGame
{
    public class PersonalEffect : Persistent<AbstractCardPersistentEffect>
    {
        public PersonalEffect(string nameid) : base(nameid, Registry.Instance.Effects.GetValueOrDefault(nameid))
        {
        }
        public PersonalEffect(AbstractCardPersistentEffect ef, AbstractPersistent? bind = null) : base(ef.NameID, ef, bind)
        {
        }
    }
}
