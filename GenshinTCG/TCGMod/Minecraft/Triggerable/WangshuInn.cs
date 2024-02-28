using System.Text.Json.Serialization;
using TCGBase;

namespace Minecraft
{
    /// <summary>
    /// [满足条件的角色]中失去生命值最多的角色[Value]点
    /// </summary>
    public class WangshuInn : AbstractTriggerable, IWhenThenAction
    {
        public override string NameID { get => "wangshuinn"; protected set { } }

        public override string Tag => SenderTag.RoundOver.ToString();

        public int Value { get; }
        public int Consume { get; }
        public List<ConditionRecordBase> When { get; }

        [JsonConstructor]
        public WangshuInn(int value = 2, int consume = 1, List<ConditionRecordBase>? when = null)
        {
            Value = value;
            Consume = consume;
            When = when ?? new();
        }
        /// <summary>
        /// 不考虑CurrCharacter为-1
        /// </summary>
        public override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (me.CurrCharacter >= 0)
            {
                var targets = me.Characters.Where(c => (this as IWhenThenAction).IsConditionValid(me, c, sender, variable));

                if (targets.Any())
                {
                    Character? curr = null;
                    int currhplost = 0;
                    for (int i = 0; i < me.Characters.Length; i++)
                    {
                        var c = me.Characters[(me.CurrCharacter + i) % me.Characters.Length];
                        if (targets.Contains(c))
                        {
                            int hplost = c.Card.MaxHP - c.HP;
                            if (hplost > currhplost)
                            {
                                curr = c;
                                currhplost = hplost;
                            }
                        }
                    }
                    if (curr != null)
                    {
                        me.Heal(persitent, this, Value, curr.PersistentRegion, false);
                        persitent.AvailableTimes -= Consume;
                    }
                }
            }
        }
    }
}
