using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Summon : AbstractPersistent<AbstractCardSummon>
    {
        public Summon(string nameid) : base(nameid, Registry.Instance.Summons.GetValueOrDefault(nameid))
        {

        }
        public Summon(AbstractCardSummon ef) : base(ef.NameID, ef)
        {
        }
    }
}
