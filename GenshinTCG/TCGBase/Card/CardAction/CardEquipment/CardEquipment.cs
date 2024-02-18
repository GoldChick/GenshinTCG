
namespace TCGBase
{
    internal sealed class CardEquipment : AbstractCardEquipment
    {
        public override CostInit Cost { get; }
        public CardEquipment(CardRecordAction record) : base(record)
        {
            Cost = new CostCreate(record.Cost).ToCostInit();

            if (record.Select.FirstOrDefault() is TargetRecord tr)
            {
                _predicate = (me, targets) =>
                {
                    if (targets.Count == 1)
                    {
                        return tr.When.TrueForAll(condition => condition.Valid(me, targets[0], null, null));
                    }
                    return false;
                };
            }
        }
        private Func<PlayerTeam, List<Persistent>, bool>? _predicate;
        public override bool EquipmentCanBeUsed(PlayerTeam me, List<Persistent> targets)
        {
            return _predicate?.Invoke(me, targets) ?? true;
        }
    }
}
