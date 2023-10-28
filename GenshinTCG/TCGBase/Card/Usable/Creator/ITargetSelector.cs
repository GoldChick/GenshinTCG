﻿namespace TCGBase
{
    public enum TargetEnum
    {
        Card_Enemy,
        Card_Me,
        Character_Enemy,
        Character_Me,

        Dice_Optional,
        Card_Optional,

        Summon_Enemy,
        Summon_Me,
        Support_Enemy,
        Support_Me
    }
    /// <summary>
    /// 实现这个接口的卡牌使用时除了骰子，还必须需要选择另外的目标
    /// 支援牌不需要实现这种东西，检测将在内部进行
    /// </summary>
    public interface ITargetSelector
    {
        /// <summary>
        /// 将按照顺序依次选取<br/>
        /// 如:[诸武精通]:{Character_Me,Character_Me}<br/>
        /// [送你一程]:{Summon}
        /// </summary>
        public TargetEnum[] TargetEnums { get; }
    }
}