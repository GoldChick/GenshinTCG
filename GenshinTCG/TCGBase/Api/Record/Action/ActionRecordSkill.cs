namespace TCGBase
{
    public record class ActionRecordSkill : ActionRecordBaseWithTarget
    {
        public int Skill { get; }
        public ActionRecordSkill(int skill = 0, TargetRecord? target = null, List<TargetRecord>? when = null) : base(TriggerType.Skill, target, when)
        {
            Skill = skill;
        }
        public override EventPersistentHandler? GetHandler(AbstractTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                Target.GetTargets(me, p, s, out var team).ForEach(pe =>
                {
                    if (pe is Character c)
                    {
                        team.EffectTrigger(new ActionUseSkillSender(team.TeamIndex, c.PersistentRegion, Skill));
                    }
                });
            };
        }
    }
}
