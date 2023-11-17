using TCGBase;

namespace TCGBase
{
    public class PersonalEffect : Persistent<ICardPersistent>
    {
        public PersonalEffect(ICardPersistent ef, AbstractPersistent? bind = null) : base(ef, bind)
        {
        }
    }
}
