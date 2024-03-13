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
        Owner,
    }
    public record class ActionRecordDice : ActionRecordBaseWithTeam
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DiceGainType Element { get; }
        /// <summary>
        /// target不为空（且为角色）时，将代替Element
        /// </summary>
        public TargetRecord? Target { get; }
        public int Count { get; }
        public bool Gain { get; }

        public ActionRecordDice(DiceGainType element, int count, bool gain = true, TargetRecord? target = null, TargetTeam team = TargetTeam.Me, List<ConditionRecordBase>? when = null) : base(TriggerType.Dice, team, when)
        {
            Element = element;
            Count = count;
            Gain = gain;
            Target = target;
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            var team = Team == TargetTeam.Enemy ? me.Enemy : me;
            var targets = Target?.GetTargets(me, p, s, v, out _);
            if (Gain)
            {
                if (targets == null)
                {
                    switch (Element)
                    {
                        case DiceGainType.Random:
                            throw new NotImplementedException("ActionRecordDice:获得随机基础元素骰还没做");
                            break;
                        case DiceGainType.Owner:
                            if (me.Characters.ElementAtOrDefault(p.PersistentRegion) is Character cc1)
                            {
                                team.GainDice(cc1.Card.CharacterElement, Count);
                            }
                            break;
                        default:
                            team.GainDice((ElementCategory)Element, Count);
                            break;
                    }
                }
                else
                {
                    foreach (var pe in targets)
                    {
                        if (pe is Character c)
                        {
                            team.GainDice(c.Card.CharacterElement, Count);
                        }
                    }
                }
            }
            else
            {
                throw new NotImplementedException("ActionRecordDice:失去骰子还没做");
            }
        }
    }
}
