using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 主动技能
    /// </summary>
    public record TriggerableRecordPrepare : TriggerableRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SkillCategory Category { get; }
        public string CardName { get; }
        public TriggerableRecordPrepare(SkillCategory category, List<ActionRecordBase> action, string? cardname = null, List<ConditionRecordBase>? when = null) : base(TriggerableType.Prepare, action, when)
        {
            Category = category;
            CardName = cardname ?? "prepareskill";
        }
        public override AbstractTriggerable GetTriggerable() => new PrepareSkillTriggerable(this);
        private sealed class PrepareSkillTriggerable : AbstractTriggerable, ISkillable
        {
            public PrepareSkillTriggerable(TriggerableRecordPrepare record)
            {
                NameID = record.CardName;
                SkillCategory = record.Category;
                EventPersistentHandler? inner = null;
                foreach (var item in record.Action)
                {
                    Action += item.GetHandler(this);
                }
                Action = inner;
            }
            public override string NameID { get; protected set; }
            public override string Tag => SenderTagInner.Prepare.ToString();
            public EventPersistentHandler? Action { get; }
            public SkillCategory SkillCategory { get; }

            public override void Trigger(PlayerTeam me, Persistent persitent, SimpleSender sender, AbstractVariable? variable)
            {
                if (me.TeamID == sender.TeamID && persitent.PersistentRegion == me.CurrCharacter)
                {
                    if (persitent is Character c && c.Effects.All(ef => !ef.CardBase.Tags.Contains(CardTag.AntiSkill.ToString())))
                    {
                        Action?.Invoke(me, persitent, sender, variable);
                        if (me.Game.CurrTeam == me.TeamID)
                        {
                            me.Game.HandleNetEvent(new NetEvent(new NetOperation(OperationType.Break)), me.TeamID);
                        }
                    }
                }
            }
        }
    }
}
