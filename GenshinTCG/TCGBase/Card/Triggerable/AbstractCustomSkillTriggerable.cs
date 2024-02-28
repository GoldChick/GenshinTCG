//namespace TCGBase
//{
//    public abstract class AbstractCustomSkillTriggerable : AbstractTriggerable, ICostable, ISkillable
//    {
//        public sealed override string Tag => SenderTagInner.UseSkill.ToString();
//        public abstract SkillCategory SkillCategory { get; }
//        public abstract CostInit Cost { get; }
//        /// <summary>
//        /// 这里的persistent其实是character，需要的话可以自行取用
//        /// </summary>
//        public abstract void AfterUseAction(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable);
//        /// <summary>
//        /// CustomSkillTriggerable已经自动封装Trigger，只需要在AfterUseAction中写出需要发生的事情即可
//        /// </summary>
//        public sealed override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable)
//            => GetSkillHandler(AfterUseAction)?.Invoke(me, persitent, sender, variable);

//        public static EventPersistentHandler GetSkillHandler(EventPersistentHandler? handler) => (me, p, s, v) =>
//        {
//            if (me.TeamIndex == s.TeamID && p.PersistentRegion == me.CurrCharacter)
//            {
//                if (p is Character c && c.Effects.All(ef => !ef.CardBase.Tags.Contains(CardTag.AntiSkill.ToString())) && s is ActionUseSkillSender ss)
//                {
//                    if (c.CardBase.TriggerableList.TryGetValue(SenderTagInner.UseSkill.ToString(), out var h, ss.Skill) && h is AbstractTriggerable skill)
//                    {
//                        handler?.Invoke(me, p, s, v);
//                        c.SkillCounter[skill.NameID]++;
//                        me.Game.EffectTrigger(new AfterUseSkillSender(me.TeamIndex, c, skill));
//                        me.SpecialState.DownStrike = false;
//                    }
//                }
//            }
//        };
//    }
//}
