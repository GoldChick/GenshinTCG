using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TCGBase
{
    public enum CardTag
    {
        /// <summary>
        /// 具有该Tag的状态，被视为能组织所附属角色使用技能，用于[冻结][石化]
        /// </summary>
        AntiSkill,
        /// <summary>
        /// 具有该Tag的状态，被标记为护盾，用于[双岩共鸣]、[贯虹]的检测
        /// </summary>
        YellowShield,
        /// <summary>
        /// 具有该Tag的卡牌，被标记为战斗行动，用于很多[天赋]、[下落斩]
        /// </summary>
        Slowly,
        /// <summary>
        /// 具有该Tag的卡牌，被视为装备，受到1人1张的限制
        /// </summary>
        Weapon,
        /// <summary>
        /// 具有该Tag的卡牌，被视为圣遗物，受到1人1张的限制
        /// </summary>
        Artifact,
        Talent,
    }
    public enum CardType
    {
        Character,
        Summon,
        Equipment,
        Support,
        Event,
        Effect
    }
    public abstract partial class AbstractCardBase : INameable, ICard, INameSetable
    {
        public virtual string Namespace { get; protected set; }
        public virtual string NameID { get; protected set; }
        public virtual int InitialUseTimes => 0;
        public bool Hidden { get; }
        public CardType CardType { get; }
        public int Variant { get; protected set; }
        public List<string> Tags { get; }
        public PersistentTriggerableList TriggerableList { get; }

        /// <summary>
        /// 通过[代码]方式创造卡牌时，需要自己维护tags和TriggerList
        /// </summary>
        protected AbstractCardBase(string nameID)
        {
            Namespace = (GetType().Namespace ?? "minecraft").ToLower();
            NameID = nameID;
            Tags = new();
            TriggerableList = new();
        }
        /// <summary>
        /// 通过[json]方式创建，可以参考现有的例子
        /// </summary>
        protected private AbstractCardBase(CardRecordBase record)
        {
            Namespace = (GetType().Namespace ?? "minecraft").ToLower();
            NameID = "name404";
            CardType = record.CardType;
            Hidden = record.Hidden;
            Tags = record.Tags;
            TriggerableList = new();
            foreach (var item in record.ModList)
            {
                TriggerableList.Add(item.GetTriggerable());
            }
            foreach (var item in record.SkillList)
            {
                TriggerableList.Add(item.GetTriggerable());
            }
        }
        /// <summary>
        /// 提供一些可供展示的值，可以在文本里以"{0}"、"{4}"的形式提现(见于FillDescription())
        /// </summary>
        protected virtual List<int> GetSpecialValues(Persistent p) => new() { p.AvailableTimes };
        public string FillDescription(Persistent p, string description)
        {
            var values = GetSpecialValues(p);
            // TODO:正则匹配还没搞好
            return FillDescriptionRegex().Replace(description, m => int.TryParse(m.Value, out int index) ? values.ElementAtOrDefault(index).ToString() : "0");
        }
        void INameSetable.SetName(string @namespace, string nameid)
        {
            Namespace = @namespace;
            NameID = nameid;
        }

        [GeneratedRegex(@"{(\d+)}")]
        private static partial Regex FillDescriptionRegex();
    }
    //public class JsonConverterAbstractCardBase : JsonConverter<AbstractCardBase>
    //{
    //    public override AbstractCardBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
    //        var root = doc.RootElement;
    //        if (root.TryGetProperty("Type", out JsonElement typeElement) && root.TryGetProperty("Name", out JsonElement nameElement))
    //        {
    //            if (Enum.TryParse(typeElement.GetString(), out CardType type))
    //            {
    //                return type switch
    //                {
    //                    //TODO:check it
    //                    CardType.Character => Registry.Instance.CharacterCards.GetValueOrDefault(nameElement.GetString()),
    //                    _ => throw new JsonException($"Unimplemented ActionRecord 'Type' property: {typeElement}."),
    //                };
    //            }
    //        }
    //        throw new JsonException($"JsonConverterAction.Read() : Missing or invalid 'Type' property:(NOT Ignore Case)Json: \n{root}");
    //    }
    //    public override void Write(Utf8JsonWriter writer, AbstractCardBase value, JsonSerializerOptions options)
    //    {
    //        writer.WriteStartObject();
    //        writer.WriteString("Type", value.CardType.ToString());
    //        writer.WriteString("Name", $"{value.Namespace}:{value.NameID}");
    //        writer.WriteEndObject();
    //    }
    //}
}
