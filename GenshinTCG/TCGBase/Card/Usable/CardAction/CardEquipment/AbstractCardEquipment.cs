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
    public abstract class AbstractCardEquipment : AbstractCardAction, ICardPersistnet, ITargetSelector,IDamageSource
    {
        string ICardPersistnet.Namespace => "equipment";
        /// <summary>
        /// 默认给自己的角色装备（可修改）
        /// </summary>
        public virtual TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };

        public virtual int InitialUseTimes => MaxUseTimes;

        public abstract int MaxUseTimes { get; }

        public int Variant => -1;

        public bool CustomDesperated => true;

        public DamageSource DamageSource => DamageSource.Addition;

        public abstract PersistentTriggerDictionary TriggerDic { get; }

        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddEquipment(this, targetArgs[0]);
        }
        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs) => me.Characters[targetArgs[0]].Alive;

        public virtual int[] Info(AbstractPersistent p) => new int[] { p.AvailableTimes };

        public void Update<T1>(Persistent<T1> persistent) where T1 : ICardPersistnet => persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
    }
    public abstract class AbstractCardWeapon : AbstractCardEquipment
    {
        public abstract WeaponCategory WeaponCategory { get; }
        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs)
        {
            var c = me.Characters[targetArgs[0]];
            return c.Alive && c.Card.WeaponCategory == WeaponCategory;
        }
    }
    public abstract class AbstractCardArtifact : AbstractCardEquipment
    {
    }
}
