﻿using System.Text.Json.Serialization;

namespace TCGBase
{
    /*
        快速行动有两轮：
        第一轮是[可以]使用的，就是普通的换班、风域
        第二轮是不能在json中使用的，用于自由的新风增加行动轮
     */
    public enum ModifierActionMode
    {
        Any,
        Skill,
        Card,
        Switch
    }
    public record class ModifierRecordFast : ModifierRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierActionMode Mode { get; }
        private static readonly ConditionRecordBase _whensourceme = new ConditionRecordBaseImplement(ConditionType.SourceMe, false);
        public ModifierRecordFast(ModifierActionMode mode, int value = 1, bool adddata = false, int consume = 1, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null) : base(ModifierType.Dice, value, adddata, consume, when, trigger)
        {
            Mode = mode;
        }
        protected override string GetSenderName()
        {
            return SenderTag.AfterOperation.ToString();
        }
        protected override EventPersistentHandler? Get()
        {
            return (me, p, s, v) =>
            {
                if (_whensourceme.Valid(me, p, s, v))
                {
                    if (s is AfterOperationSender aos && v is FastActionVariable fav)
                    {
                        bool modeFlag = Mode switch
                        {
                            ModifierActionMode.Card => aos.ActionType == OperationType.UseCard,
                            ModifierActionMode.Skill => aos.ActionType == OperationType.UseSKill,
                            ModifierActionMode.Switch => aos.ActionType == OperationType.Switch,
                            _ => true
                        };
                        if (modeFlag && !fav.Fast)
                        {
                            fav.Fast = true;
                            if (Consume > 0)
                            {
                                p.AvailableTimes -= Consume;
                            }
                        }
                    }
                }
            };
        }
    }
}