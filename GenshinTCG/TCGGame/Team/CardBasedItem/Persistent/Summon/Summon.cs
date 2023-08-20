using TCGCard;
using TCGRule;

namespace TCGGame
{
    public class Summon : AbstractPersistent<ISummon>
    {
        public Summon(string nameid) : base(nameid, Registry.Instance.Summons.GetValueOrDefault(nameid))
        {

        }
    }
}
