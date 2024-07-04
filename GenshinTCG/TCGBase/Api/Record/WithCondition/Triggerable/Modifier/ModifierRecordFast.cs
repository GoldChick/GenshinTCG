using NLua;
using System;
using System.Text.Json.Serialization;

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
    /// <summary>
    /// 参数见于<see cref="AfterOperationSender"/> and <seealso cref="FastActionVariable"/>
    /// </summary>
    public record class ModifierRecordFast : ModifierRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierActionMode Mode { get; }
        public ModifierRecordFast(ModifierActionMode mode, List<string>? lua = null, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null) : base(ModifierType.Dice, lua, when, trigger)
        {
            Mode = mode;
        }
        protected override string GetSenderName() => SenderTag.AfterOperation.ToString();
        protected override bool DefaultConditionCheck(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, AbstractTriggerable modTriggerable)
        {
            if (_whensourceme.Valid(me, p, s, v) && s is AfterOperationSender aos && v is FastActionVariable fav)
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
                    return true;
                }
            }
            return false;
        }
        protected override void Modify(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, Lua lua)
        {
            if (v is FastActionVariable fav)
            {
                fav.Fast = true;
                base.Modify(me, p, s, v, lua);
            }
        }
    }
}
