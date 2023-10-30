using TCGBase;

namespace TCGBase
{
    public enum WeaponCategory
    {
        Other,
        Sword,
        Claymore,
        Longweapon,
        Catalyst,
        Bow
    }
    public abstract class AbstractCardEquipment : AbstractCardAction
    {
    }
    public abstract class AbstractCardWeapon : AbstractCardEquipment, ITargetSelector
    {
        public abstract WeaponCategory WeaponCategory { get; }
        /// <summary>
        /// 默认给自己的角色装备（可修改）
        /// </summary>
        public virtual TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };
        public override bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null) => me.Characters[targetArgs[0]].Card.WeaponCategory == WeaponCategory;
    }
    public abstract class AbstractCardArtifact : AbstractCardEquipment
    {

    }
}
