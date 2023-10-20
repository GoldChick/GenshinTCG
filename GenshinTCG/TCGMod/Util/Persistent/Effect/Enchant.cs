using TCGBase;
using TCGCard;

namespace TCGMod
{
    public class Enchant : AbstractCardPersistentEffect
    {
        private readonly string _nameid;
        private readonly int _maxusetimes;
        public readonly Dictionary<string, PersistentTrigger> _triggerdic;
        /// <param name="round">为true是回合制effect，为false是次数effect</param>
        public Enchant(string nameid, int element, int maxusetimes, bool round = true, int adddamage = 0)
        {
            _nameid = nameid;
            _maxusetimes = maxusetimes;
            _triggerdic = new()
            {
                { Tags.SenderTags.ELEMENT_ENCHANT, new PersistentElementEnchant(element, !round,adddamage) }
            };
            if (round)
            {
                _triggerdic.Add(Tags.SenderTags.ROUND_START, new((me, p, s, v) => p.AvailableTimes--));
            }
        }
        public override int MaxUseTimes => _maxusetimes;

        public override Dictionary<string, PersistentTrigger> TriggerDic => _triggerdic;

        public override string NameID => _nameid;
    }
}
