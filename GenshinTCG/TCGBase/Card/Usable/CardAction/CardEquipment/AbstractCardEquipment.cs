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
        /// <summary>
        /// 绑定在角色身上的effect
        /// </summary>
        public abstract AbstractCardPersistentEffect Effect { get; }
    }
    public abstract class AbstractCardWeapon : AbstractCardEquipment, ITargetSelector
    {
        public abstract WeaponCategory WeaponCategory { get; }
        /// <summary>
        /// 默认给自己的角色装备（可修改）
        /// </summary>
        public virtual TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };
        public override bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null) => me.Characters[targetArgs[0]].Card.WeaponCategory == WeaponCategory;
        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            int target = targetArgs[0];
            var cha = me.Characters[target];
            if (cha.Weapon != null)
            {
                cha.Weapon.Active = false;
                me.EffectUpdate();
            }
            cha.Weapon = new Persistent<AbstractCardPersistentEffect>(Effect);
        }
    }
    public abstract class AbstractCardArtifact : AbstractCardEquipment
    {
        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            int target = targetArgs[0];
            var cha = me.Characters[target];
            if (cha.Artifact != null)
            {
                cha.Artifact.Active = false;
                me.EffectUpdate();
            }
            cha.Artifact = new Persistent<AbstractCardPersistentEffect>(Effect);
        }
    }
}
