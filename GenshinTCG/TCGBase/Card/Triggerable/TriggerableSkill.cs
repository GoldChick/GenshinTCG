namespace TCGBase
{
    internal sealed class TriggerableSkill : AbstractTriggerable, ICostable, ISkillable
    {
        public TriggerableSkill(TriggerableRecordSkill skill)
        {
            NameID = skill.CardName;//set in INameSetable
            SkillCategory = skill.Category;
            Cost = new CostCreate(skill.Cost).ToCostInit();

            EventPersistentHandler? inner = null;
            foreach (var item in skill.Action)
            {
                inner += item.GetHandler(this);
            }
            inner += (me, p, s, v) =>
            {
                if (p is Character c)
                {
                    c.MP += skill.MP;
                }
            };
            Action = TriggerablePreset.GetSkillHandler(inner);
        }
        public SkillCategory SkillCategory { get; }
        public CostInit Cost { get; }
        public EventPersistentHandler? Action { get; internal set; }
        public override string NameID { get; protected set; }
        public override string Tag => SenderTagInner.UseSkill.ToString();
        public override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable) => Action?.Invoke(me, persitent, sender, variable);
    }
}
