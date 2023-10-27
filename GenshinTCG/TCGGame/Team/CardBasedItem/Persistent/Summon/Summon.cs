using TCGBase;
using TCGRule;

namespace TCGGame
{
    public class Summon : Persistent<AbstractCardPersistentSummon>
    {
        public Summon(string nameid) : base(nameid, Registry.Instance.Summons.GetValueOrDefault(nameid))
        {

        }
        public Summon(AbstractCardPersistentSummon ef) : base(ef.NameID, ef)
        {
        }
    }
}
