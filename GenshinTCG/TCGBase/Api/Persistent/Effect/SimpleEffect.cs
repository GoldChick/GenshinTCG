namespace TCGBase
{
    /// <summary>
    /// 只是用来给一些触发提供占位符，如[本大爷还没有输]\[霜华矢]
    /// </summary>
    public class SimpleEffect : AbstractCardPersistentEffect
    {
        private readonly string _nameid;
        private readonly PersistentTriggerDictionary _triggerdic;
        private void SetDeActive(PlayerTeam me, AbstractPersistent p, AbstractSender s, AbstractVariable? v) => p.Active = false;

        public SimpleEffect(string nameid)
        {
            _nameid = nameid;
            _triggerdic = new();
        }
        /// <param name="fades">在提供的sendertag触发后消失</param>
        public SimpleEffect(string nameid, params SenderTag[] fades)
        {
            _nameid = nameid;
            _triggerdic = new();
            Array.ForEach(fades, st => _triggerdic.Add(st, SetDeActive));
        }
        /// <param name="fades">在提供的sendertag触发后消失</param>
        public SimpleEffect(string nameid, params string[] fades)
        {
            _nameid = nameid;
            _triggerdic = new();
            Array.ForEach(fades, st => _triggerdic.Add(st, SetDeActive));
        }
        public override int MaxUseTimes => 1;

        public override PersistentTriggerDictionary TriggerDic => _triggerdic;

        public override string NameID => _nameid;
    }
}
