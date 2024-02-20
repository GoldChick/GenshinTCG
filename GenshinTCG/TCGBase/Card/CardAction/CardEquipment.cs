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
        /// 默认给自己的角色装备（可修改，但是修改了的Q天赋要实现IEnergyConsumer来额外指定消耗谁的能量，或者不消耗）
        /// </summary>
        public List<TargetDemand> TargetDemands => new()
        {
            new(TargetTeam.Me,TargetType.Character,_predicate)
        };

        private readonly Func<PlayerTeam, List<Persistent>, bool> _predicate;
        public CardEquipment(CardRecordAction record) : base(record)
        {
            if (record.Select.FirstOrDefault() is TargetRecord tr)
            {
                _predicate = (me, targets) =>
                {
                    if (targets.Count == 1)
                    {
                        //TODO: use card sender ....
                        return tr.When.TrueForAll(condition => condition.Valid(me, targets[0], null, null));
                    }
                    return false;
                };
            }
            else
            {
                _predicate = (me, targets) => false;
            }
        }
    }
}
