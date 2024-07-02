using NLua;

namespace TCGBase
{
    public record class ConditionRecordLua : ConditionRecordBase
    {
        public List<string> Value { get; }
        public ConditionRecordLua(List<string>? value = null, bool not = false, ConditionRecordBase? or = null) : base(ConditionType.Lua, not, or)
        {
            Value = value ?? new();
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            try
            {
                using Lua lua = new();
                lua.LoadCLRPackage();
                lua.DoString("import('TCGBase')");
                lua.DoString("import('System.Linq')");
                lua["me"] = me;
                lua["p"] = p;
                lua["s"] = s;
                lua["v"] = v;
                lua.DoString(string.Join('\n', Value));
                if (lua["result"] is bool r)
                {
                    return r;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}