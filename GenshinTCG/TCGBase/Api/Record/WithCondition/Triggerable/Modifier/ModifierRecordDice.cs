using System.Text.Json.Serialization;

namespace TCGBase
{
    /*
        目前观察到的现象：
        作为技能的 x无色+x某元素：
        首先减费某元素，依次寻找对应元素、任意元素
        其次减费无色，依次寻找无色元素、有色元素、任意元素

        例
        铃铛+魔女+灼灼+烟熏鸡：普攻不消耗灼灼
        冰圣遗物+铃铛+火花+薯条：普攻不消耗薯条
     */
    /// <summary>
    /// 对于减费来说：<br/>
    /// consume为[非负]表示一次消耗[consume]个次数；consume为[负]表示一次最多消耗[减的费用]的次数
    /// </summary>
    public record class ModifierRecordDice : ModifierRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementCategory Element { get; }
        public bool If { get; }
        private DiceModifierType _demand => Element switch
        {
            ElementCategory.Trival => DiceModifierType.Any,
            ElementCategory.Void => DiceModifierType.Void,
            _ => DiceModifierType.Color
        };
        private static readonly ConditionRecordBase _whensourceme = new ConditionRecordBaseImplement(ConditionType.SourceMe, false);
        public ModifierRecordDice(ElementCategory element, int value = 1, bool adddata = false, bool iF = false, int consume = 1, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null) : base(ModifierType.Dice, value, adddata, consume, when, trigger)
        {
            Element = element;
            If = iF;
        }
        protected override string GetSenderName()
        {
            return (If ? DiceModifierType.If : _demand).ToString();
        }
        protected override EventPersistentHandler? Get()
        {
            return (me, p, s, v) =>
            {
                if (_whensourceme.Valid(me, p, s, v))
                {
                    //dms: void=>color=>any=>if
                    //scv: trival / color=>void
                    if (s is DiceModifierSender dms && v is SingleCostVariable scv)
                    {
                        bool elementFlag = _demand switch
                        {
                            DiceModifierType.Void => scv.Type == ElementCategory.Void,
                            DiceModifierType.Color => scv.Type == ElementCategory.Void || ((int)scv.Type > 0 && (int)scv.Type <= 7 && scv.Type == Element),
                            _ => true
                        };

                        if (elementFlag && scv.Count > 0)
                        {
                            int min = int.Min(scv.Count, p.AvailableTimes);

                            if (!If || scv.Count <= Value)
                            {
                                scv.Count -= Value;
                                if (dms.RealAction)
                                {
                                    p.AvailableTimes -= Consume >= 0 ? Consume : min;
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
