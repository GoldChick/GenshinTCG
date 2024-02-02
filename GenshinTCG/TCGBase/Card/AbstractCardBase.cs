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
        /// <summary>
        /// 对于[角色牌][行动牌]，hidden的不能被选择；<br/>
        /// 对于[角色/出战状态]，hidden的前台不显示
        /// 对于[召唤物]，hidden无用
        /// </summary>
        public bool Hidden { get; }
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
            Hidden = record.Hidden;
            NameID = record.NameID;
            Tags = record.Tags;
            TriggerList = new(record.SkillList.Select(Trigger.Convert).ToList());
        }
    }
}
