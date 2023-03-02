//################################################################
//额外制作的卡牌dll实现这些接口
//################################################################
using System.Collections.Generic;
using System;

namespace TCGCard
{
    /// <summary>
    /// 卡片类型
    ///总体分为[角色卡]和[辅助卡]
    ///<para></para>
    ///[辅助卡]包括[天赋卡][场地卡][武器卡][圣遗物卡][食物卡][事件卡][召唤物卡]等
    /// </summary>
    public enum CardType
    {
        Character,

        Nature,
        Weapon,
        Artifact,
        Place,
        Food,
        Event,

        Summon,

        Effect,
        Skill
    }

    public interface ICardServer : ICardBase
    {
        /// <summary>
        /// 卡片的类型
        /// </summary>
        public CardType CardType { get; }
        /// <summary>
        /// 默认携带的属性
        /// </summary>
        public Enum[] Attributes { get; }
    }
}