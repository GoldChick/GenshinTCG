namespace TCGBase
{
    public enum CardType
    {
        Character,
        Summon,
        Support,
        Event,
        Effect
    }
    public abstract class AbstractCardBase
    {
        public string Namespace => (GetType().Namespace ?? "minecraft").ToLower();
        public string NameID { get; }
        public CardType CardType { get; }
        public List<string> Tags { get; }
        public PersistentTriggerList TriggerList { get; }
        /// <summary>
        /// 通过[代码]方式创造卡牌时，需要自己维护tags和TriggerList
        /// </summary>
        protected AbstractCardBase(string nameID)
        {
            NameID = nameID;
            Tags = new();
            TriggerList = new();
        }
        /// <summary>
        /// 通过[json]方式创建，可以参考现有的例子
        /// </summary>
        protected private AbstractCardBase(BaseCardRecord record)
        {
            CardType = record.CardType;
            NameID = record.NameID;
            Tags = record.Tags;
            TriggerList = new(record.SkillList.Select(Trigger.Convert).ToList());
        }
    }
}
