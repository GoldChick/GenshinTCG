using TCGBase;

namespace TCGCard
{
    /// <summary>
    /// 表示伤害的根本来源，现在包括:
    /// Skill Summon Effect
    /// </summary>
    public interface IDamageSource
    {
        public DamageSource DamageSource { get; }
    }
}
