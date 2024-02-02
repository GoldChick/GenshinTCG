namespace TCGBase
{
    public static class TriggerPreset
    {

        public static ITriggerable DoSkill(params string[] args)
        {
            var skill = new SkillTrigger(Enum.Parse<SkillCategory>(args[1]), TriggerBasePreset.GetCost(args[2]));
            skill.Action = GetTrigger(args[3], skill);
            return skill;
        }
        public static EventPersistentHandler GetTrigger(string arg, ITriggerable triggerable)
        {
            var args = arg.Split(',');
            if (Enum.TryParse(args[0], out TriggerType type))
            {
                return type switch
                {
                    TriggerType.DoDamage => DoDamage(args[1], triggerable),
                    _ => throw new Exception("TriggerPreset.GetTrigger(): Unknown TriggerType Enum!")
                };
            }
            return null;
        }
        public static EventPersistentHandler DoDamage(string dmg, ITriggerable triggerable)
        {
            DamageVariable dv = new(TriggerBasePreset.GetDamage(dmg));
            return (me, p, s, v) =>
            {
                //TODO: check it?
                me.DoDamage(dv, triggerable);
            };
        }
        public static EventPersistentHandler DoDamageAorB(string dmgA, string condition, string dmgB, ITriggerable triggerable)
        {
            DamageVariable dvA = new(TriggerBasePreset.GetDamage(dmgA));
            DamageVariable dvB = new(TriggerBasePreset.GetDamage(dmgB));
            return (me, p, s, v) =>
            {
                //TODO: check it?
                me.DoDamage(dvA, triggerable);
            };
        }
    }

}
