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
        public ModifierRecordFast(ModifierActionMode mode, List<string>? lua = null, bool luaID = false, List<ConditionRecordBase>? when = null, ActionRecordBase? afterSuccess = null) : base(ModifierType.Fast, lua, luaID, when, afterSuccess)
        {
            Mode = mode;
        }
        protected override bool DefaultConditionCheck(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, AbstractTriggerable modTriggerable)
        {
            if (_whensourceme.Valid(me, p, s, v) && s is AfterOperationSender aos && v is FastActionVariable fav)
            {
                return Mode switch
                {
                    ModifierActionMode.Card => aos.ActionType == OperationType.UseCard,
                    ModifierActionMode.Skill => aos.ActionType == OperationType.UseSKill,
                    ModifierActionMode.Switch => aos.ActionType == OperationType.Switch,
                    _ => true
                };
            }
            return false;
        }
    }
}
