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
    public abstract class AbstractCardEquipment : AbstractCardAction, ICardPersistent, ITargetSelector, IDamageSource
    {
        /// <summary>
        /// 默认给自己的角色装备（可修改，但是修改了的Q天赋要实现IEnergyConsumer来额外指定消耗谁的能量，或者不消耗）
        /// </summary>
        public virtual TargetDemand[] TargetDemands => new TargetDemand[]
        {
            new(TargetEnum.Character_Me,CanBeUsed)
        };

        public virtual int InitialUseTimes => MaxUseTimes;

        public abstract int MaxUseTimes { get; }

        public abstract int Variant { get; }

        public bool CustomDesperated => true;

        public DamageSource DamageSource => DamageSource.Addition;

        public abstract PersistentTriggerDictionary TriggerDic { get; }

        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs) => me.Characters[targetArgs[0]].Alive;

        public void Update<T>(PlayerTeam me, Persistent<T> persistent) where T : ICardPersistent => persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);

        public virtual void OnDesperated(PlayerTeam me, int region)
        {
        }

        protected AbstractCardEquipment()
        {
        }
    }
    public abstract class AbstractCardWeapon : AbstractCardEquipment
    {
        public abstract WeaponCategory WeaponCategory { get; }
        public override sealed int Variant => -1;
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddEquipment(this, targetArgs[0]);
        }
        public override bool CanBeUsed(PlayerTeam me, int[] targetArgs)
        {
            var c = me.Characters[targetArgs[0]];
            return c.Alive && c.Card.WeaponCategory == WeaponCategory;
        }
    }
    public abstract class AbstractCardArtifact : AbstractCardEquipment
    {
        public override sealed int Variant => -2;
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddEquipment(this, targetArgs[0]);
        }
    }
}
