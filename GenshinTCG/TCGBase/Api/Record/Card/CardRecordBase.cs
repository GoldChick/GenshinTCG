using System.Text.Json.Serialization;

namespace TCGBase
{
    public record BaseGameRecord
    {
        public bool Hidden { get; }

        protected BaseGameRecord(bool hidden)
        {
            Hidden = hidden;
        }
    }
    public record CardRecordBase : BaseGameRecord
    {
        /// <summary>
        /// 卡牌一共分两大类别：角色牌，行动牌<br/>
        /// 行动牌又有五大类别：装备牌，支援牌，事件牌，状态牌，召唤物牌
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CardType CardType { get; }
        public CardRecordBase(bool hidden, CardType cardType, List<TriggerableRecordBase> skillList, List<string>? tags) : base(hidden)
        {
            CardType = cardType;
            SkillList = skillList;
            Tags = tags ?? new();
        }
        public List<TriggerableRecordBase> SkillList { get; }
        public List<string> Tags { get; }
    }
}
