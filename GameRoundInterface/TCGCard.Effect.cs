//################################################################
//Effect分为[个人效果]和[团队效果]两种
//################################################################
namespace TCGCard
{
    public enum EffectType
    {
        Character,
        Team
    }
    public interface IEffect
    {
        string GetEffectName();
        EffectType GetEffectType();
        int GetMaxUseTimes();
        void BeHurtEvent();
        void AttackEvent();
    }
}
