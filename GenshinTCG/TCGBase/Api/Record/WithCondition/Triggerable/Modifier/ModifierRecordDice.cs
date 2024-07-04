using System.Text.Json.Serialization;

namespace TCGBase
{
    /*
        目前观察到的现象：
        作为技能的 x某元素+x无色：
        首先减费某元素，依次寻找对应元素、任意元素(Trivial)
        其次减费无色，依次寻找无色元素、有色元素、任意元素

        例
        铃铛+魔女+灼灼+烟熏鸡：普攻不消耗灼灼
        冰圣遗物+铃铛+火花+薯条：普攻不消耗薯条
     */
    //TODO:关于减费，仍然还难以下结论
    public record class ModifierRecordDice : ModifierRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementCategory Element { get; }
        /// <summary>
        /// NOTE:蒂玛乌斯、瓦格纳具有奇特的低优先级
        /// </summary>
        public bool Late { get; }
        private DiceModifierType _demand => Element switch
        {
            ElementCategory.Trivial => DiceModifierType.Any,
            ElementCategory.Void => DiceModifierType.Void,
            _ => DiceModifierType.Color
        };
        public ModifierRecordDice(ElementCategory element, List<string>? lua = null, bool luaID = false, bool late = false, List<ConditionRecordBase>? when = null, ActionRecordBase? afterSuccess = null) : base(ModifierType.Dice, lua, luaID, when, afterSuccess)
        {
            Element = element;
            Late = late;
        }
        protected override string GetSenderName()
        {
            return (Late ? DiceModifierType.Late : _demand).ToString();
        }
        protected override bool DefaultConditionCheck(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, AbstractTriggerable modTriggerable)
        {
            if (_whensourceme.Valid(me, p, s, v) && v is SingleCostVariable scv)
            {
                //dms: void=>color=>any=>if
                //scv: Trivial / color=>void
                bool elementFlag = _demand switch
                {
                    DiceModifierType.Void => scv.Type == ElementCategory.Void,
                    DiceModifierType.Color => scv.Type == ElementCategory.Void || ((int)scv.Type > 0 && (int)scv.Type <= 7 && scv.Type == Element),
                    _ => true
                };

                if (elementFlag && scv.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
