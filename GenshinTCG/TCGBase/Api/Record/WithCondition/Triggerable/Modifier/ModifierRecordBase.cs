using NLua;
using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ModifierType
    {
        Damage,
        /// <summary>
        /// 自带要求：伤害是物理伤害
        /// </summary>
        Enchant,
        //下面一个When SourceMe
        Dice,
        /// <summary>
        /// 自带要求：When SourceMe
        /// </summary>
        Fast,
        /// <summary>
        /// 自带要求：When TargetThis
        /// </summary>
        Shield,
        /// <summary>
        /// 自带要求：When TargetThis
        /// </summary>
        Barrier
    }
    public record class ModifierRecordBase : IWhenThenAction
    {
        protected static readonly ConditionRecordBase _whensourceme = new(ConditionType.SourceMe, false);
        protected static readonly ConditionRecordBase _targetThis = new(ConditionType.TargetThis, false);
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierType Type { get; }

        public List<string> Lua { get; }
        public List<ConditionRecordBase> When { get; }
        public ActionRecordTrigger? WhenSuccess { get; }
        public ModifierRecordBase(ModifierType type, List<string>? lua = null, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null)
        {
            Type = type;
            Lua = lua ?? new();
            When = when ?? new();
            WhenSuccess = trigger;
        }
        public AbstractTriggerable GetTriggerable()
        {
            return new Triggerable(GetSenderName(), GetHandler);
        }
        private EventPersistentHandler GetHandler(AbstractTriggerable modTriggerable)
        {
            return (me, p, s, v) =>
            {
                if (DefaultConditionCheck(me, p, s, v, modTriggerable) && (this as IWhenThenAction).IsConditionValid(me, p, s, v))
                {
                    using Lua lua = new();
                    lua.LoadCLRPackage();
                    lua.DoString("import('TCGBase')");
                    lua.DoString("import('System.Linq')");
                    lua["this"] = modTriggerable;
                    lua["me"] = me;
                    lua["p"] = p;
                    lua["s"] = s;
                    lua["v"] = v;
                    Modify(me, p, s, v, lua);
                    if (v is IModifier im && im.RealAction)
                    {
                        WhenSuccess?.GetHandler(modTriggerable)?.Invoke(me, p, s, v);
                    }
                }
            };
        }
        protected virtual bool DefaultConditionCheck(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, AbstractTriggerable modTriggerable)
        {
            return Type switch
            {
                ModifierType.Enchant => v is DamageVariable dv && dv.Element == DamageElement.Trival,
                ModifierType.Shield or ModifierType.Barrier => v is DamageVariable dv && dv.Amount > 0 && _targetThis.Valid(me, p, s, v),
                _ => true
            };
        }
        protected virtual string GetSenderName()
        {
            return (Type switch
            {
                ModifierType.Enchant => SenderTag.ElementEnchant,
                ModifierType.Damage => SenderTag.DamageIncrease,
                ModifierType.Shield or ModifierType.Barrier => SenderTag.HurtDecrease,
                _ => throw new NotImplementedException($"UnImplemented Modifier Record Type: {Type}")
            }).ToString();
        }
        /// <summary>
        /// 当预设条件都通过时，调用这里；默认为执行lua脚本<br/>
        /// <list type="number">
        /// <item>对于Shield，已经提前写好了扣盾方式，但之后仍然会执行lua脚本</item>
        /// <item>1</item>
        /// </list>
        /// </summary>
        protected virtual void Modify(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, Lua lua)
        {
            switch (Type)
            {
                case ModifierType.Shield:
                    if (v is DamageVariable dv)
                    {
                        var min = int.Min(p.AvailableTimes, dv.Amount);
                        dv.Amount -= min;
                        p.AvailableTimes -= min;
                    }
                    break;
            }
            //lua文件格式: "minecraft:enchant.cryo"
            foreach (var name in Lua)
            {
                try
                {
                    var array = name.Split('.');
                    if (Registry.Instance.LuaScripts.TryGetValue(array[0], out var script))
                    {
                        lua.DoString(script);
                        (lua[array.ElementAtOrDefault(1) ?? "main"] as LuaFunction)?.Call(me, p, s, v);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"执行lua脚本{name}出现问题，详细原因：{e.Message}");
                }
            }
        }
    }
}
