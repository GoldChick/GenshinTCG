using TCGBase;

namespace TCGBase
{
    public static class WeaponTags
    {
        public static string GetWeaponCategory(WeaponCategory cate) => cate switch
        {
            WeaponCategory.SWORD => "minecraft:sword",
            WeaponCategory.CLAYMORE => "minecraft:claymore",
            WeaponCategory.LONGWEAPON => "minecraft:longweapon",
            WeaponCategory.CATALYST => "minecraft:catalyst",
            WeaponCategory.BOW => "minecraft:bow",
            _ => "minecraft:other"
        };
    }
    public enum WeaponCategory
    {
        OTHER,
        SWORD,
        CLAYMORE,
        LONGWEAPON,
        CATALYST,
        BOW
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
