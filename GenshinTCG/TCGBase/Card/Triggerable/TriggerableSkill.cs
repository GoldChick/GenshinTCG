namespace TCGBase
{
    internal sealed class TriggerableSkill : AbstractTriggerableSkill
    {
        public TriggerableSkill(TriggerableRecordSkill skill)
        {
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
            Action = TriggerablePreset.GetSkillHandler(inner);
        }
        public override SkillCategory SkillCategory { get; }
        public override CostInit Cost { get; }
        public EventPersistentHandler? Action { get; internal set; }
        public override void Trigger(AbstractTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable) => Action?.Invoke(me, persitent, sender, variable);
    }
}
