using TCGBase;
using TCGGame;

namespace GenshinTCG.TCGMod.Util.Persistent
{
    public static class PersistentFunc
    {
        /// <returns>当[对方角色]受到伤害时，是否为[我方角色]造成，最好用于Element_Enchant、Hurt_Add和Hurt_Mul</returns>
        public static bool IsCurrCharacterDamage(PlayerTeam me, AbstractPersistent p, AbstractSender s, AbstractVariable? v, out DamageVariable? dv)
         => (dv = v as DamageVariable) != null && s.TeamID == me.TeamIndex && dv.DirectSource == DamageSource.Character && me.CurrCharacter == p.PersistentRegion;
    }
}
