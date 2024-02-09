namespace TCGBase
{
    public static class TriggerablePreset
    {
        public static EventPersistentHandler GetSkillHandler(EventPersistentHandler? handler) => (me, p, s, v) =>
        {
            if (p.PersistentRegion == me.CurrCharacter)
            {
                if (p is Character c && c.Effects.All(ef => !ef.CardBase.Tags.Contains(CardTag.AntiSkill.ToString())) && s is ActionUseSkillSender ss)
                {
                    if (c.CardBase.TriggerableList.TryGetValue(SenderTagInner.UseSkill.ToString(), out var h, ss.Skill) && h is ITriggerable skill)
                    {
                        me.Game.EffectTrigger(new SimpleSender(me.TeamIndex, SenderTag.BeforeUseSkill));
                        handler?.Invoke(me, p, s, v);
                        c.SkillCounter[ss.Skill]++;
                        me.Game.EffectTrigger(new AfterUseSkillSender(me.TeamIndex, c, skill));
                        me.SpecialState.DownStrike = true;
                    }
                }
            }
        };
    }
}
