using System.Collections.Generic;
using TCGBase;
using TCGInfo;
using TCGInfo.InfoInterface;
//################################################################
//Effect分为[个人效果]和[团队效果]两种
//广义的Effect还包括左侧的辅助牌和右侧的召唤物牌
//这里提供了一些预设的接口
//注意：这里的Effect往往并不以卡牌的形式出现（尽管被划分到TCGCard模块，但这是为了开发方便）
//################################################################
namespace TCGCard
{
    public enum EffectType
    {
        Character,
        Team,
        Assist,
        Summon
    }
    /// <summary>
    /// 一个Effect可以有多个触发方式
    /// 通过检测触发方式以实现对应的不同效果
    /// </summary>
    public enum EffectTriggerType
    {
        Before = 0,
        On = 1,
        After = 2
    }
    public interface ICardEffect : ICardBase
    {
        public bool Stackable { get; }
        public int MaxUseTimes { get; }
        public int Prior { get; }//值越大，越靠后触发，默认为0
        public EffectType EffectType { get; }
        public Dictionary<EffectTriggerType, ActionType> EffectTriggers { get; }

        public EffectTriggerType TriggerType { get; }
        public ActionType ActionType { get; }

        //public void OnGain();
        //public void OnLose();

        /// <summary>
        /// 当type为OnAttack和OnHurt时infos为IInfo<IDamage>
        /// </summary>
        /// <param name="infos"></param>
        void Work(EffectTriggerType triggerType, params IInfo[] infos);//TODO
    }
    public interface ICardEffect<T> : ICardEffect
    {
        T GetAdditionalValue();
    }

    public interface ICardSummon : ICardEffect<(ElementType element, int damage)>
    {

    }
}
