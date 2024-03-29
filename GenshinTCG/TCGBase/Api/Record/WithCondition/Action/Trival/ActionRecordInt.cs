﻿namespace TCGBase
{
    public record class ActionRecordInt : ActionRecordBaseWithTarget
    {
        public int Value { get; }
        public ActionRecordInt(TriggerType type, int value = 0, TargetRecord? target = null, List<ConditionRecordBase>? when = null) : base(type, target, when)
        {
            Value = int.Max(0, value);
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
                            team.Game.EffectTrigger(new ActionUseSkillSender(team.TeamIndex, c.PersistentRegion, Value));
                            break;
                        case TriggerType.Prepare:
                            team.Game.EffectTrigger(new ActionUsePrepareSender(team.TeamIndex, c.PersistentRegion, Value));
                            break;
                        case TriggerType.Heal:
                            team.Heal(p, triggerable, Value, c.PersistentRegion, false);
                            break;
                        case TriggerType.Revive:
                            team.Heal(p, triggerable, Value, c.PersistentRegion, false, true);
                            break;
                    }
                }
            });
        }
    }
}
