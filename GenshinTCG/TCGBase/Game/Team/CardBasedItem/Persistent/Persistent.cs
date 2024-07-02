using System.Text.Json;
using System.Text.Json.Serialization;

namespace TCGBase
{
    public class Persistent
    {
        [JsonIgnore]
        internal PersistentSet? Owner { get; set; }
        /// <summary>
        /// 整局游戏的状态自增ID，场上的每个状态的ID各不相同
        /// </summary>
        public int ID { get; internal set; }
        protected int _availableTimes;
        public int AvailableTimes
        {
            get => _availableTimes;
            set
            {
                _availableTimes = int.Max(0, value);
                if (CardBase is IEffect ef)
                {
                    Active = ef.CustomDesperated | _availableTimes != 0;
                }
            }
        }

        public AbstractCardBase CardBase { get; }
        /// <summary>
        /// 用来表明persistent在谁身上，在加入PersistentSet时赋值:<br/>
        /// -1=团队 0-5=角色 11=召唤物 12=支援区 -20~-11=手牌
        /// </summary>
        public int PersistentRegion { get; internal set; }
        /// <summary>
        /// 是否处于激活状态，默认为True，当某次行动结算结束之后会清除未激活的effect
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// 用于[已知class的]persistent储存更加详细的数据，自行维护<br/>
        /// 如[桓纳兰那][立本]
        /// </summary>
        public List<int> Data { get; internal set; }
        /// <summary>
        /// 依赖于自己的persistent们，此状态清除时将把list中的其他状态也清除
        /// </summary>
        public List<int> Childs { get; }
        public Persistent(AbstractCardBase card, Persistent? bind = null)
        {
            CardBase = card;
            Active = true;
            Childs = new();
            Data = new();
            AvailableTimes = card.InitialUseTimes;

            //TODO: lazy add
            //bind?.Childs?.Add(this);
        }
    }
    public class Persistent<T> : Persistent where T : AbstractCardBase
    {
        public T Card { get; }
        public Persistent(T card, Persistent? bind = null) : base(card, bind)
        {
            Card = card;
        }
    }
    public class JsonConverterPersistent : JsonConverter<Persistent>
    {
        public override Persistent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (root.TryGetProperty("Type", out JsonElement typeElement))
            {
                if (Enum.TryParse(typeElement.GetString(), out TriggerType type))
                {
                    return type switch
                    {
                        TriggerType.AorB => JsonSerializer.Deserialize<Persistent>(root.GetRawText(), options),

                        _ => throw new JsonException($"Unimplemented ActionRecord 'Type' property: {typeElement}."),
                    };
                }
            }
            throw new JsonException($"JsonConverterAction.Read() : Missing or invalid 'Type' property:(NOT Ignore Case)Json: \n{root}");
        }

        public override void Write(Utf8JsonWriter writer, Persistent value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }

}
