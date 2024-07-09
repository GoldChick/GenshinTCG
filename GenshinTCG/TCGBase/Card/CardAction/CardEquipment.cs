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
        /// 只允许给自己的角色装备
        /// </summary>
        public List<TargetDemand> TargetDemands { get; }
        public CardEquipment(CardRecordAction record) : base(record)
        {
            TargetDemands = new()
            {
            new(TargetTeam.Me,TargetType.Character,(me, oldps,newp) =>
            {
                if (record.Select.FirstOrDefault() is IWhenThenAction selectrecord)
                {
                    return  !oldps.Any() && selectrecord.IsConditionValid(me, newp, new ActionDuringUseCardSender(me.TeamID, oldps), null);
                }
                return false;
            })};
        }
    }
}
