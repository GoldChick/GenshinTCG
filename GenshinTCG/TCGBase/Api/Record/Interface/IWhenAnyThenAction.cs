namespace TCGBase
{
    public interface IWhenAnyThenAction
    {
        public List<List<ConditionRecordBase>> WhenAny { get; }
        public bool IsConditionValid(PlayerTeam me, Persistent? p, AbstractSender? s, AbstractVariable? v)
        {
            return WhenAny.Count == 0 || WhenAny.Any(list => list.TrueForAll(condition => condition.Valid(me, p, s, v)));
        }
    }
}
