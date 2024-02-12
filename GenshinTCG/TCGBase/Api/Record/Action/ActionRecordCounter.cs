namespace TCGBase
{
    /// <summary>
    /// NameID的状态的<see cref="Persistent.AvailableTimes"/>增加Add(可以为负)<br/>
    /// 如果NameID为null，并且调用该Handler的Persistent不为Character，那就直接增加给他
    /// </summary>
    public record class ActionRecordCounter : ActionRecordBaseWithTeam
    {
        public int Add { get; }
        public string? Name { get; }
        public ActionRecordCounter(int add, string? name = null, DamageTargetTeam team = DamageTargetTeam.Me, List<TargetRecord>? when = null) : base(TriggerType.Effect, team, when)
        {
            Add = add;
            Name = name;
        }
        public override EventPersistentHandler? GetHandler(AbstractCustomTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                if (Name != null)
                {
                    Queue<PersistentSet<AbstractCardBase>> queue = new(me.Characters.Select(c => c.Effects));
                    queue.Enqueue(me.Effects);
                    queue.Enqueue(me.Summons);
                    queue.Enqueue(me.Supports);
                    bool flag = false;
                    while (!flag && queue.TryDequeue(out var set))
                    {
                        flag = TryAdd(set);
                    }
                }
                else if (p is not Character)
                {
                    p.AvailableTimes += Add;
                }
            };
        }
        private bool TryAdd(PersistentSet<AbstractCardBase> set)
        {
            if (set.TryFind(ef => $"{ef.CardBase.Namespace}:{ef.CardBase.NameID}" == Name, out var target))
            {
                target.AvailableTimes += Add;
                return true;
            }
            return false;
        }
    }
}
