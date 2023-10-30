using TCGBase;

namespace TCGBase
{
    public class Effect : Persistent<AbstractCardPersistentEffect>
    {
        public Effect(AbstractCardPersistentEffect ef, AbstractPersistent? bind=null) : base(ef.NameID, ef,bind)
        {
        }
    }
}
