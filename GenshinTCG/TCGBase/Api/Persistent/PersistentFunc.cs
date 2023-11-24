using System.Diagnostics.CodeAnalysis;

namespace TCGBase
{
    /// <summary>
    /// 提供一些可能会用到的预制方法？<br/>
    /// 不一定是良好的
    /// </summary>
    public static class PersistentFunc
    {
        /// <returns>当[对方角色]受到伤害时，是否为[我方角色]造成，最好用于Element_Enchant、Hurt_Add和Hurt_Mul</returns>
        public static bool IsCurrCharacterDamage(PlayerTeam me, AbstractPersistent p, AbstractSender s, AbstractVariable? v, [NotNullWhen(returnValue: true)] out DamageVariable? dv)
         => (dv = v as DamageVariable) != null && s.TeamID == me.TeamIndex && dv.DirectSource == DamageSource.Character
                && (p.PersistentRegion < 0 || p.PersistentRegion > 10 || me.CurrCharacter == p.PersistentRegion);
    }
}
