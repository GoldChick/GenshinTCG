namespace TCGBase
{
    public class Enchant : AbstractCardPersistentEffect
    {
        private readonly string _nameid;
        private readonly int _maxusetimes;
        public readonly PersistentTriggerDictionary _triggerdic;
        /// <param name="round">为true是回合制effect，为false是次数effect</param>
        public Enchant(string nameid, int element, int maxusetimes, bool round = true, int adddamage = 0)
        {
            _nameid = nameid;
            _maxusetimes = maxusetimes;
            _triggerdic = new()
            {
                { SenderTag.ElementEnchant, new PersistentElementEnchant(element, !round,adddamage) }
            };
            if (round)
            {
                _triggerdic.Add(SenderTag.RoundOver, (me, p, s, v) => p.AvailableTimes--);
            }
        }
        public override int MaxUseTimes => _maxusetimes;

        public override PersistentTriggerDictionary TriggerDic => _triggerdic;

        public override string NameID => _nameid;
    }
}
