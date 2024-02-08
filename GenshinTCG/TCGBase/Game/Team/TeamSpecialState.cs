﻿namespace TCGBase
{
    /// <summary>
    /// update see the last of <see cref="Game.EventProcess(NetEvent, int, out AbstractSender, out FastActionVariable?)"/>
    /// </summary>
    public class TeamSpecialState
    {
        /// <summary>
        /// 是否满足重击条件，即行动前骰子数为偶数
        /// </summary>
        public bool HeavyStrike { get; internal set; }
        /// <summary>
        /// 是否满足下落攻击条件，即上次行动为切换角色时，造成普通攻击伤害
        /// </summary>
        public bool DownStrike { get; internal set; }
        /// <summary>
        /// 是否满足本大爷还没有输条件，即本回合我方有角色被击倒
        /// </summary>
        public bool HaventLost { get; internal set; }
        /// <summary>
        /// 我方受到的元素伤害种类数
        /// </summary>
        public List<int> ElementTypeSufferNum { get; }
        /// <summary>
        /// 我方支援区弃置的卡牌数量
        /// </summary>
        public int SupportDesperatedNum { get; internal set; }
        public TeamSpecialState()
        {
            ElementTypeSufferNum = new();
        }
    }
}
