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
    public abstract class AbstractCardEquipment : AbstractCardAction, ITargetSelector
    {
        /// <summary>
        /// 默认给自己的角色装备（可修改，但是修改了的Q天赋要实现IEnergyConsumer来额外指定消耗谁的能量，或者不消耗）
        /// </summary>
        public TargetDemand[] TargetDemands => new TargetDemand[]
        {
            new(TargetEnum.Character_Me,CanBeUsed)
        };
        /// <summary>
        /// 对于装备，这里targetArgs为长度1的数组，唯一的值为我方目标角色的index
        /// </summary>
        public override bool CanBeUsed(AbstractTeam me, int[] targetArgs) => me.Characters[targetArgs[0]].Alive;
        protected AbstractCardEquipment() 
        {
        }
        protected private AbstractCardEquipment(CardRecordEquipment record) : base(record)
        {
        }
    }
}
