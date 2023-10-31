using TCGBase;

namespace TCGBase
{
    public class PersonalEffect : Persistent<AbstractCardPersistentEffect>
    {
        public PersonalEffect(AbstractCardPersistentEffect ef, AbstractPersistent? bind = null) : base(ef, bind)
        {
        }
    }
}
