using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ModifierType
    {
        Damage,
        Enchant,
        //下面一个When SourceMe
        Dice,
        Fast,
        //下面两个内置 When TargetThis
        Shield,
        Barrier
    }
    public record class ModifierRecordBase : IWhenThenAction
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierType Type { get; }

        public int Value { get; }
        /// <summary>
        /// 如果[成功触发]，减少多少AvailableTime，默认1
        /// </summary>
        public int Consume { get; }
        /// <summary>
        /// 如果大于等于0并[成功触发]，会向persistent.data中加入<br/>
        /// </summary>
        public int AddData { get; }
        public List<ConditionRecordBase> When { get; }
        public ActionRecordTrigger? Trigger { get; }
        public ModifierRecordBase(ModifierType type, int value = 1, int consume = 1, int adddata = -1, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null)
        {
            Type = type;
            Value = int.Max(value, 1);
            Consume = consume;
            When = when ?? new();
            Trigger = trigger;
            AddData = adddata;
            switch (Type)
            {
                case ModifierType.Shield:
                    Consume = 0;
                    break;
            }
        }
        public AbstractTriggerable GetTriggerable()
        {
            return new Triggerable(GetSenderName(), GetHandler);
        }
        private EventPersistentHandler GetHandler(AbstractTriggerable modTriggerable)
        {
            return (me, p, s, v) =>
            {
                if ((this as IWhenThenAction).IsConditionValid(me, p, s, v))
                {
                    Get(() =>
                    {
                        Trigger?.GetHandler(modTriggerable)?.Invoke(me, p, s, v);
                        if (AddData >= 0)
                        {
                            p.Data.Add(AddData);
                        }
                    })?.Invoke(me, p, s, v);
                }
            };
        }
        protected virtual string GetSenderName()
        {
            return (Type switch
            {
                ModifierType.Enchant => SenderTag.ElementEnchant,
                ModifierType.Shield or ModifierType.Barrier => SenderTag.HurtDecrease,
                _ => throw new NotImplementedException($"UnImplemented Modifier Record Type: {Type}")
            }).ToString();
        }
        protected virtual EventPersistentHandler? Get(Action whensuccess)
        {
            return (me, p, s, v) =>
            {
                ConditionRecordBase targetthis = new(ConditionType.TargetThis, false, null);

                if (v is DamageVariable dv)
                {
                    switch (Type)
                    {
                        case ModifierType.Enchant:
                            if (dv.Element == DamageElement.Trival)
                            {
                                dv.Element = (DamageElement)Value;
                                p.AvailableTimes -= Consume;
                                whensuccess.Invoke();
                            }
                            break;
                        case ModifierType.Shield:
                            if (targetthis.Valid(me, p, s, v))
                            {
                                var min = int.Min(p.AvailableTimes, dv.Amount);
                                dv.Amount -= min;
                                p.AvailableTimes -= min;
                                whensuccess.Invoke();
                            }
                            break;
                        case ModifierType.Barrier:
                            if (targetthis.Valid(me, p, s, v) && dv.Amount > 0)
                            {
                                dv.Amount -= Value;
                                p.AvailableTimes -= Consume;
                                whensuccess.Invoke();
                            }
                            break;
                    }
                }
            };
        }
    }
    public record class ModifierRecordBaseImplement : ModifierRecordBase
    {
        public ModifierRecordBaseImplement(ModifierType type, int value, int consume = 1, int adddata = -1, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null) : base(type, value, consume, adddata, when, trigger)
        {
        }
    }
}
