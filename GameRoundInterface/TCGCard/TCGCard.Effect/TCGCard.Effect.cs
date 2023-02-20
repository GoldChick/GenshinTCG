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
        Instant = 0,
        OnAttack = 1,
        OnHurt = 2,
        OnDead = 4,
        OnRoundStart = 8,
        OnRoundEnd = 16,
        Custom = 32
    }
    public interface ICardEffect : ICardBase
    {
        public bool Stackable { get; }
        public int MaxUseTimes { get; }
        public EffectType EffectType { get; }
        public EffectTriggerType TriggerType { get; }
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
