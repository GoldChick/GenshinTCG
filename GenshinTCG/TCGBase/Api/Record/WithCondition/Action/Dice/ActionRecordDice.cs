using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum DiceGainType
    {
        Trival,
        Cryo,
        Hydro,
        Pyro,
        Electro,
        Geo,
        Dendro,
        Anemo,
        Void,
        Random,
        Owner
    }
    public record class ActionRecordDice : ActionRecordBaseWithTeam
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DiceGainType Element { get; }
        public int Count { get; }
        public bool Gain { get; }
        public ActionRecordDice(DiceGainType element, int count, bool gain = true, TargetTeam team = TargetTeam.Me, List<ConditionRecordBase>? when = null) : base(TriggerType.Dice, team, when)
        {
            Element = element;
            Count = count;
            Gain = gain;
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            var team = Team == TargetTeam.Enemy ? me.Enemy : me;
            if (Gain)
            {
                switch (Element)
                {
                    case DiceGainType.Random:
                        throw new NotImplementedException("ActionRecordDice:获得随机基础元素骰还没做");
                        break;
                    case DiceGainType.Owner:
                        if (me.Characters.ElementAtOrDefault(p.PersistentRegion) is Character c && c.CardBase is CardCharacter cc)
                        {
                            team.GainDice(cc.CharacterElement, Count);
                        }
                        break;
                    default:
                        team.GainDice((ElementCategory)Element, Count);
                        break;
                }
            }
            else
            {
                throw new NotImplementedException("ActionRecordDice:失去骰子还没做");
            }
        }
    }
}
