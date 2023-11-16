using TCGBase;

namespace TCGBase
{
    public class PersonalEffect : Persistent<ICardPersistnet>
    {
        public PersonalEffect(ICardPersistnet ef, AbstractPersistent? bind = null) : base(ef, bind)
        {
        }
    }
}
