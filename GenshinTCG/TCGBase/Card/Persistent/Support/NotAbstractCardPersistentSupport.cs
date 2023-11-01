namespace TCGBase
{
    /// <summary>
    /// 为了简化Support卡牌的编写而产生的内部类
    /// </summary>
    internal class CardPersistentSupport : AbstractCardPersistent
    {
        private readonly Func<AbstractPersistent, int[]> _infos;
        public override string TextureNameID { get; }
        public override int InitialUseTimes { get; }
        public override int MaxUseTimes { get; }
        public override bool CustomDesperated { get; }
        public override PersistentTriggerDictionary TriggerDic { get; }
        public override int[] Info(AbstractPersistent p) => _infos(p);

        public CardPersistentSupport(AbstractCardSupport card) : this(card.NameID, card.InitialUseTimes, card.MaxNumPermitted, card.CustomDesperated,  card.TriggerDic, card.Info) { }
        private CardPersistentSupport(string nameid, int initialusetimes, int maxusetimes, bool customdesperated, PersistentTriggerDictionary triggerdic, Func<AbstractPersistent, int[]> infos)
        {
            TextureNameID = nameid;
            InitialUseTimes = initialusetimes;
            MaxUseTimes = maxusetimes;
            CustomDesperated = customdesperated;
            TriggerDic = triggerdic;
            _infos = infos;
        }
    }
}
