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
    public class CardEquipment : AbstractCardAction, ITargetSelector
    {
        /// <summary>
        /// 默认给自己的角色装备
        /// </summary>
        public List<TargetDemand> TargetDemands { get; }
        public CardEquipment(CardRecordAction record) : base(record)
        {
            TargetDemands = new()
            {
            new(TargetTeam.Me,TargetType.Character,(me, oldps,newp) => record.Select.FirstOrDefault() is TargetRecord tr && !oldps.Any() &&
                    tr.When.TrueForAll(condition => condition.Valid(me, newp, new ActionDuringUseCardSender(me.TeamID, oldps), null)))
            };
        }
    }
}
