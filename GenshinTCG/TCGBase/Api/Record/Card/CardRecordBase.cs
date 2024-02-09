using System.Text.Json.Serialization;

namespace TCGBase
{
    public record BaseGameRecord
    {
        public string NameID { get; }
        public bool Hidden { get; }

        protected BaseGameRecord(string nameID, bool hidden)
        {
            NameID = nameID;
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
        public CardRecordBase(string nameID, bool hidden, CardType cardType, List<TriggerableRecordBase> skillList, List<string>? tags) : base(nameID, hidden)
        {
            CardType = cardType;
            SkillList = skillList;
            Tags = tags ?? new();
        }
        public List<TriggerableRecordBase> SkillList { get; }
        public List<string> Tags { get; }
    }
}
