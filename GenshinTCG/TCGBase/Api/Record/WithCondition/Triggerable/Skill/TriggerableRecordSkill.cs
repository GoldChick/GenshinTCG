using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 主动技能
    /// </summary>
    public record TriggerableRecordSkill : TriggerableRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SkillCategory Category { get; }
        public List<SingleCostVariable> Cost { get; }
        public int MP { get; }
        public string CardName { get; }
        public TriggerableRecordSkill(SkillCategory category, List<SingleCostVariable> cost, List<ActionRecordBase> action, string? cardname = null, int mp = 1, List<ConditionRecordBase>? when = null) : base(TriggerableType.Skill, action, when)
        {
            Category = category;
            Cost = cost;
            MP = mp;
            CardName = cardname ?? "skill";
        }
        public override AbstractTriggerable GetTriggerable() => new TriggerableSkill(this);
        internal sealed class TriggerableSkill : AbstractTriggerable, ICostable, ISkillable
        {
            public TriggerableSkill(TriggerableRecordSkill skill)
            {
                NameID = skill.CardName;
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
                Action = inner;
            }
            public SkillCategory SkillCategory { get; }
            public CostInit Cost { get; }
            public EventPersistentHandler? Action { get; internal set; }
            public override string NameID { get; protected set; }
            public override string Tag => SenderTagInner.UseSkill.ToString();
            public override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                if (me.TeamIndex == sender.TeamID && persitent.PersistentRegion == me.CurrCharacter)
                {
                    if (persitent is Character c && c.Effects.All(ef => !ef.CardBase.Tags.Contains(CardTag.AntiSkill.ToString())) && sender is ActionUseSkillSender ss)
                    {
                        if (c.CardBase.TriggerableList.TryGetValue(SenderTagInner.UseSkill.ToString(), out var h, ss.Skill) && h is AbstractTriggerable skill)
                        {
                            Action?.Invoke(me, persitent, sender, variable);
                            c.SkillCounter[skill.NameID]++;
                            me.Game.EffectTrigger(new AfterUseSkillSender(me.TeamIndex, c, skill));
                            me.SpecialState.DownStrike = false;
                        }
                    }
                }
            }
        }

    }
}
