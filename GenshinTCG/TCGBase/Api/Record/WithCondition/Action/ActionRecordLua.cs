namespace TCGBase
{
    public record class ActionRecordLua : ActionRecordBase, ILuaable
    {
        public List<string> Lua { get; }

        public bool LuaID { get; }

        public ActionRecordLua(List<string>? lua = null, bool luaID = false, List<ConditionRecordBase>? when = null) : base(TriggerType.Lua, when)
        {
            Lua = lua ?? new();
            LuaID = luaID;
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            (this as ILuaable).DoLua(me, p, s, v, triggerable);
        }
    }
}
