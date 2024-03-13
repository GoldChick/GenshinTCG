using System.Text.Json.Serialization;

namespace TCGBase
{
    public record class ModifierRecordEnchant : ModifierRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageElement Element { get; }
        /// <summary>
        /// 为true通过从data中第一个数字是1到4来染色
        /// </summary>
        public bool FromData { get; }
        public ModifierRecordEnchant(DamageElement element = DamageElement.Trival, bool fromdata = true, int consume = 1, int adddata = -1, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null) : base(ModifierType.Enchant, 0, consume, adddata, when, trigger)
        {
            Element = element;
            FromData = fromdata;
        }
        protected override EventPersistentHandler? Get(Action whensuccess)
        {
            return (me, p, s, v) =>
            {
                if (v is DamageVariable dv)
                {
                    if (dv.Element == DamageElement.Trival)
                    {
                        int absorb = p.Data.First();
                        if (FromData && absorb > 0 && absorb < 5)
                        {
                            dv.Element = (DamageElement)absorb;
                        }
                        else
                        {
                            dv.Element = Element;
                        }
                        p.AvailableTimes -= Consume;
                        whensuccess.Invoke();
                    }
                }
            };
        }
    }
}
