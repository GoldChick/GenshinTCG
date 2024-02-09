namespace TCGBase
{
    public enum WeaponCategory
    {
        OtherWeapon,
        Sword,
        Claymore,
        Longweapon,
        Catalyst,
        Bow
    }
    public abstract class AbstractCardEquipment : AbstractCardAction, ITargetSelector
    {
        /// <summary>
        /// 默认给自己的角色装备（可修改，但是修改了的Q天赋要实现IEnergyConsumer来额外指定消耗谁的能量，或者不消耗）
        /// </summary>
        public List<TargetDemand> TargetDemands => new()
        {
            new(DamageTargetTeam.Me,SelectType.Character,EquipmentCanBeUsed)
        };
        /// <summary>
        /// 对于装备，targets为长度1的list，唯一的值为我方目标角色的index
        /// </summary>
        public virtual bool EquipmentCanBeUsed(PlayerTeam me, List<Persistent> targets)
            => targets.ElementAtOrDefault(0) is Character c && c.Alive;
        protected AbstractCardEquipment()
        {
        }
        protected private AbstractCardEquipment(CardRecordEquipment record) : base(record)
        {
        }
    }
}
