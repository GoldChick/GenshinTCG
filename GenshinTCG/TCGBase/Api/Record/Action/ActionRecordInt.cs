namespace TCGBase
{
    public record class ActionRecordInt : ActionRecordBaseWithTarget
    {
        public int Value { get; }
        public ActionRecordInt(TriggerType type, int value = 0, TargetRecord? target = null, List<TargetRecord>? whenwith = null) : base(type, target, whenwith)
        {
            Value = value;
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            Target.GetTargets(me, p, s, v, out var team).ForEach(pe =>
            {
                if (pe is Character c)
                {
                    switch (Type)
                    {
                        case TriggerType.MP:
                            c.MP += Value;
                            break;
                        case TriggerType.Skill:
                            team.EffectTrigger(new ActionUseSkillSender(team.TeamIndex, c.PersistentRegion, Value));
                            break;
                    }
                }
            });
        }
    }
}
