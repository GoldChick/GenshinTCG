namespace TCGBase
{
    /// <summary>
    /// 为了简化Support卡牌的编写而产生的内部类
    /// </summary>
    internal class CardPersistentSupport : AbstractCardPersistent
    {
        private readonly Func<AbstractPersistent, int[]> _infos;
        public override string NameID { get; }
        public override int InitialUseTimes { get; }
        public override int MaxUseTimes { get; }
        public override bool DeleteWhenUsedUp { get; }
        public override PersistentTriggerDictionary TriggerDic { get; }
        public override int[] Info(AbstractPersistent p) => _infos(p);

        public CardPersistentSupport(AbstractCardSupport card) : this(card.NameID, card.InitialUseTimes, card.MaxNumPermitted, card.DeleteWhenUsedUp, card.TriggerDic, card.Info) { }
        private CardPersistentSupport(string nameid, int initialusetimes, int maxusetimes, bool deletewhenusedup, PersistentTriggerDictionary triggerdic, Func<AbstractPersistent, int[]> infos)
        {
            NameID = nameid;
            InitialUseTimes = initialusetimes;
            MaxUseTimes = maxusetimes;
            DeleteWhenUsedUp = deletewhenusedup;
            TriggerDic = triggerdic;
            _infos = infos;
        }
    }
}
