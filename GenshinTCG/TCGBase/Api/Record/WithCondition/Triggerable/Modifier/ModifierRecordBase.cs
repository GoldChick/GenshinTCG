﻿using System.Text.Json.Serialization;

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
        /// 如果成功触发，减少多少AvailableTime，默认1
        /// </summary>
        public int Consume { get; }
        public List<ConditionRecordBase> When { get; }
        public ActionRecordTrigger? Trigger { get; }
        public ModifierRecordBase(ModifierType type, int value = 1,int consume = 1, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null)
        {
            Type = type;
            Value = int.Max(value, 1);
            Consume = consume;
            When = when ?? new();
            Trigger = trigger;
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
                    Get()?.Invoke(me, p, s, v);
                    Trigger?.GetHandler(modTriggerable)?.Invoke(me, p, s, v);
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
        protected virtual EventPersistentHandler? Get()
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
                            }
                            break;
                        case ModifierType.Shield:
                            if (targetthis.Valid(me, p, s, v))
                            {
                                var min = int.Min(p.AvailableTimes, dv.Amount);
                                dv.Amount -= min;
                                p.AvailableTimes -= min;
                            }
                            break;
                        case ModifierType.Barrier:
                            if (targetthis.Valid(me, p, s, v) && dv.Amount > 0)
                            {
                                dv.Amount -= Value;
                                p.AvailableTimes -= Consume;
                            }
                            break;
                    }
                }
            };
        }
    }
    public record class ModifierRecordBaseImplement : ModifierRecordBase
    {
        public ModifierRecordBaseImplement(ModifierType type, int value, int consume = 1, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null) : base(type, value, consume, when, trigger)
        {
        }
    }
}
