namespace TCGBase
{
    public record class ActionRecordMP : ActionRecordBaseWithTarget
    {
        public int Amount { get; }
        public ActionRecordMP(int amount, TargetRecord? target = null, List<TargetRecord>? when = null) : base(TriggerType.MP, target, when)
        {
            Amount = amount;
        }
        public override EventPersistentHandler? GetHandler(AbstractCustomTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                Target.GetTargets(me, p, s, out var _).ForEach(pe =>
                {
                    if (pe is Character c)
                    {
                        c.MP += Amount;
                    }
                });
            };
        }
    }
}
