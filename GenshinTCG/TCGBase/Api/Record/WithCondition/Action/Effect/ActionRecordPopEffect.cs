namespace TCGBase
{
    /// <summary>
    /// 将target的persistent转移到Destination对应的PersistentSet
    /// </summary>
    public record class ActionRecordPopEffect : ActionRecordBaseWithTarget
    {
        /// <summary>
        /// 如果Type为Summon或Support或Effect，转移到对应位置<br/>
        /// 如果Type为Hand，则返回手中（仅限CardAction）<br/>
        /// 其他情况下，则选择第一个目标的Owner（如果有），如果没有就转移到队伍Effect<br/>
        /// 如果满了或无可用destination，则不能转移
        /// </summary>
        public TargetRecord Destination { get; }
        public ActionRecordPopEffect(TargetRecord? destination = null, TargetRecord? target = null, List<ConditionRecordBase>? when = null) : base(TriggerType.PopEffect, target, when)
        {
            Destination = destination ?? new();
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            var dests = Destination.GetTargets(me, p, s, v, out var team);
            AbstractPersistentSet destination = team.CardsInHand;
            switch (Destination.Type)
            {
                case TargetType.Summon:
                    destination = team.Summons;
                    break;
                case TargetType.Support:
                    destination = team.Supports;
                    break;
                case TargetType.Hand:
                    destination = team.CardsInHand;
                    break;
                default:
                    if (dests.FirstOrDefault() is Persistent per && per.Owner != null)
                    {
                        destination = per.Owner;
                    }
                    else
                    {
                        destination = team.Effects;
                    }
                    break;
            }
            Target.GetTargets(me, p, s, v, out _).ForEach(pe =>
            {
                pe.Owner?.PopTo(pe, destination);
            });
        }
    }
}
