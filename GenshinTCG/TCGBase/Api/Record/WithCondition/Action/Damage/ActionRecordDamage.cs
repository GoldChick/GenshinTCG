﻿namespace TCGBase
{
    public record class ActionRecordDamage : ActionRecordBase
    {
        /// <summary>
        /// 不为null并且有效时，对且仅对其中的**第一个**应用damage<br/>
        /// targetexcept依然有效，而targetindexoffset、targetteam无效<br/>
        /// 上述效果也会应用于subdamage
        /// </summary>
        public TargetSupplyRecord? Target { get; }
        public DamageRecord Damage { get; }
        public List<ActionRecordBase> With { get; }
        public ActionRecordDamage(DamageRecord damage, List<ActionRecordBase>? with = null, TargetSupplyRecord? target = null, List<ConditionRecordBase>? when = null) : base(TriggerType.Damage, when)
        {
            Damage = damage;
            With = with ?? new();
            Target = target;
        }
        private static DamageRecord? GetDamageRecordWithTarget(int targetindexoffset, TargetTeam team, DamageRecord? curr)
        {
            return curr == null ? null : new(curr.Element, curr.Amount, targetindexoffset, curr.TargetArea, team, GetDamageRecordWithTarget(targetindexoffset, team, curr.SubDamage));
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, SimpleSender s, AbstractVariable? v)
        {
            EventPersistentHandler? subhandler = null;
            foreach (ActionRecordBase action in With)
            {
                subhandler += action.GetHandler(triggerable);
            }
            me.DoDamage(Target != null && Target.GetTargets(me, p, s, v).FirstOrDefault() is Character cha ?
                GetDamageRecordWithTarget(cha.PersistentRegion - me.CurrCharacter, (TargetTeam)(1 - cha._t.TeamID ^ me.TeamID), Damage)
                : Damage, p, triggerable, () =>
                {
                    subhandler?.Invoke(me, p, s, v);
                });
        }
    }
}
