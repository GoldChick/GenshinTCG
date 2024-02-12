namespace TCGBase
{
    public record TriggerableRecordAfterUseSkill : TriggerableRecordWithAction
    {
        /// <summary>
        /// 如果为true，则第一次不会触发<br/>
        /// 说的就是你！旋火轮！<br/>
        /// 会占用persistent的Data
        /// </summary>
        public bool NeedEnable { get; }
        /// <summary>
        /// 默认为Passive，表示所有技能均能触发
        /// </summary>
        public SkillCategory SkillType { get; }
        public TriggerableRecordAfterUseSkill(List<ActionRecordBase> action, SkillCategory skilltype = SkillCategory.P, bool needEnable = false) : base(TriggerableType.AfterUseSkill, action)
        {
            NeedEnable = needEnable;
            SkillType = skilltype;
        }
        public override AbstractTriggerable GetTriggerable()
        {
            var t = new Triggerable(SenderTag.AfterUseSkill.ToString());

            EventPersistentHandler? Handler = null;
            foreach (var item in Action)
            {
                Handler += item.GetHandler(t);
            }

            t.Action += (me, p, s, v) =>
            {
                if (me.TeamIndex == s.TeamID && s is AfterUseSkillSender ss && (SkillType == SkillCategory.P || (ss.Skill is ISkillable skill && skill.SkillCategory == SkillType)))
                {
                    if (NeedEnable && p.Data == null)
                    {
                        p.Data = 1;
                    }
                    else
                    {
                        Handler?.Invoke(me, p, s, v);
                    }
                }
            };
            return t;
        }
    }
}
