//################################################################
//Effect分为[个人效果]和[团队效果]两种
//################################################################
using TCGInfo.InfoInterface;

namespace TCGCard
{
    public enum EffectType
    {
        Character,
        Team
    }
    public enum EffectTriggerType
    {
        Instant = 0,
        OnAttack = 1,
        OnHurt = 2,
        OnDead = 4,
        Custom = 8
    }
    public enum EffectDurationType
    {
        Instant = 0,
        Frequency = 1,
        Round = 2,
    }
    public interface ICardEffect:ICardBase
    {
        string GetEffectName();
        string GetEffectDescription();
        EffectType GetEffectType();
        EffectTriggerType GetEffectTriggerType();
        EffectDurationType GetEffectDurationType();
        int GetMaxUseTimes();
        void Action(EffectTriggerType triggerType, IInfoBase[] infos);//TODO
    }
}
