using NLua;

namespace TCGBase
{
    public record class ActionRecordLua : ActionRecordBase
    {
        public List<string> Value { get; }
        public ActionRecordLua(List<string>? value = null, List<ConditionRecordBase>? when = null) : base(TriggerType.Lua, when)
        {
            Value = value ?? new();
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            try
            {
                using Lua lua = new();
                lua.LoadCLRPackage();
                lua.DoString("import('TCGBase')");
                lua.DoString("import('System.Linq')");
                lua["this"] = triggerable;
                lua["me"] = me;
                lua["p"] = p;
                lua["s"] = s;
                lua["v"] = v;
                lua.DoString(string.Join('\n', Value));
            }
            catch (Exception)
            {
            }
        }
    }
}
