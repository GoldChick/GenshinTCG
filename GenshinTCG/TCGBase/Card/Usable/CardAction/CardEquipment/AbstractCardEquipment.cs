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
    public abstract class AbstractCardEquipment<T> : AbstractCardAction, ITargetSelector where T: AbstractCardPersistentEquipment
    {
        /// <summary>
        /// 绑定在角色身上的effect<br/>
        /// 对于talent可以覆写角色技能
        /// </summary>
        public abstract T Effect { get; }
        /// <summary>
        /// 默认给自己的角色装备（可修改）
        /// </summary>
        public virtual TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };
        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            me.AddEquipment(Effect, targetArgs[0]);
        }
    }
    public abstract class AbstractCardWeapon : AbstractCardEquipment<AbstractCardPersistentWeapon>
    {
        public abstract WeaponCategory WeaponCategory { get; }
        public override bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null) => me.Characters[targetArgs[0]].Card.WeaponCategory == WeaponCategory;
    }
    public abstract class AbstractCardArtifact : AbstractCardEquipment<AbstractCardPersistentArtifact>
    {
    }
}
