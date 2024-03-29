﻿namespace TCGBase
{
    public enum CardTag
    {
        /// <summary>
        /// 具有该Tag的状态，被视为能组织所附属角色使用技能，用于[冻结][石化]
        /// </summary>
        AntiSkill,
        /// <summary>
        /// 具有该Tag的状态，被标记为护盾，用于[双岩共鸣]、[贯虹]的检测
        /// </summary>
        YellowShield,
        /// <summary>
        /// 具有该Tag的卡牌，被标记为战斗行动，用于很多[天赋]、[下落斩]
        /// </summary>
        Slowly,
        /// <summary>
        /// 具有该Tag的卡牌，被视为装备，受到1人1张的限制
        /// </summary>
        Weapon,
        /// <summary>
        /// 具有该Tag的卡牌，被视为圣遗物，受到1人1张的限制
        /// </summary>
        Artifact,
        Talent,
    }
    public enum CardType
    {
        Character,
        Summon,
        Equipment,
        Support,
        Event,
        Effect
    }
    public abstract class AbstractCardBase : INameable, ICard, INameSetable
    {
        public virtual string Namespace { get; protected set; }
        public virtual string NameID { get; protected set; }
        public virtual int InitialUseTimes => 0;
        public bool Hidden { get; }
        public CardType CardType { get; }
        public int Variant { get; protected set; }
        public List<string> Tags { get; }
        public PersistentTriggerableList TriggerableList { get; }

        /// <summary>
        /// 通过[代码]方式创造卡牌时，需要自己维护tags和TriggerList
        /// </summary>
        protected AbstractCardBase(string nameID)
        {
            Namespace = (GetType().Namespace ?? "minecraft").ToLower();
            NameID = nameID;
            Tags = new();
            TriggerableList = new();
        }
        /// <summary>
        /// 通过[json]方式创建，可以参考现有的例子
        /// </summary>
        protected private AbstractCardBase(CardRecordBase record)
        {
            Namespace = (GetType().Namespace ?? "minecraft").ToLower();
            NameID = "name404";
            CardType = record.CardType;
            Hidden = record.Hidden;
            Tags = record.Tags;
            TriggerableList = new();
            foreach (var item in record.ModList)
            {
                TriggerableList.Add(item.GetTriggerable());
            }
            foreach (var item in record.SkillList)
            {
                TriggerableList.Add(item.GetTriggerable());
            }
        }
        void INameSetable.SetName(string @namespace, string nameid)
        {
            Namespace = @namespace;
            NameID = nameid;
        }
    }
}
