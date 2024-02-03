namespace TCGBase
{
    public static class TriggerablePreset
    {
        public static ITriggerable DoSkill(params string[] args)
        {
            // eg:  "skill[a[pyro=1,void=2[dodamageaorb,pyro-3,count=3,pyro-5[mp,1" 迪卢克e
            var skill = new SkillTriggerable(Enum.Parse<SkillCategory>(args[1]), TriggerBasePreset.GetCost(args[2]));
            skill.Action = (me, p, s, v) =>
            {
                //TODO:检测冻结、石化
                if (p.PersistentRegion == me.CurrCharacter)
                {
                    GetTrigger(args.ElementAtOrDefault(3), skill)?.Invoke(me, p, s, v);
                    GetTrigger(args.ElementAtOrDefault(4), skill)?.Invoke(me, p, s, v);
                    if (p is Character c && s is ActionUseSkillSender ss)
                    {
                        c.SkillCounter[ss.Skill]++;
                    }
                }
            };
            return skill;
        }
        public static EventPersistentHandler? GetTrigger(string? arg, ITriggerable triggerable)
        {
            if (arg != null)
            {
                var args = arg.Split(',');
                if (Enum.TryParse(args[0], out TriggerType type))
                {
                    return type switch
                    {
                        TriggerType.DoDamage => DoDamage(args[1], triggerable),
                        TriggerType.MP => MP(args[1]),
                        _ => throw new Exception("TriggerPreset.GetTrigger(): Unknown TriggerType Enum!")
                    };
                }
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
        public static EventPersistentHandler MP(string mp, string? target = null)
        {
            //TODO:能不能其他人(
            return (me, p, s, v) =>
            {
                if (p is Character c)
                {
                    if (int.TryParse(mp, out int mpnum))
                    {
                        c.MP += mpnum;
                    }
                }
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
