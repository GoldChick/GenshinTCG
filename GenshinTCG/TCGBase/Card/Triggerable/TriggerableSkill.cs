namespace TCGBase
{
    internal sealed class TriggerableSkill : AbstractTriggerable, ICostable, ISkillable
    {
        public TriggerableSkill(TriggerableRecordSkill skill)
        {
            NameID = "useskill";//set in INameSetable
            SkillCategory = skill.Category;

            CostCreate cost = new();
            foreach (var item in skill.Cost)
            {
                cost.Add(item.Type, item.Count);
            }
            Cost = cost.ToCostInit();

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
