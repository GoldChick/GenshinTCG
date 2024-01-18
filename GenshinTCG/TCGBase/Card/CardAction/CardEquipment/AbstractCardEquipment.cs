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
    public abstract class AbstractCardEquipment : AbstractCardAction, ICardPersistent, ITargetSelector
    {
        /// <summary>
        /// 默认给自己的角色装备（可修改，但是修改了的Q天赋要实现IEnergyConsumer来额外指定消耗谁的能量，或者不消耗）
        /// </summary>
        public virtual TargetDemand[] TargetDemands => new TargetDemand[]
        {
            new(TargetEnum.Character_Me,CanBeUsed)
        };
        public override bool CustomDesperated => true;

        public override DamageSource DamageSource => DamageSource.Addition;

        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs) => me.Characters[targetArgs[0]].Alive;

        public override void Update<T>(PlayerTeam me, Persistent<T> persistent) => persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);

        protected AbstractCardEquipment()
        {
        }
    }
    public abstract class AbstractCardWeapon : AbstractCardEquipment
    {
        public abstract WeaponCategory WeaponCategory { get; }
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddEquipment(this, targetArgs[0]);
        }
        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs)
        {
            var c = me.Characters[targetArgs[0]];
            return c.Alive && c.Card.WeaponCategory == WeaponCategory;
        }
        protected AbstractCardWeapon()
        {
            Variant = -1;
        }
    }
    public abstract class AbstractCardArtifact : AbstractCardEquipment
    {
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddEquipment(this, targetArgs[0]);
        }
        protected AbstractCardArtifact()
        {
            Variant = -2;
        }
    }
}
