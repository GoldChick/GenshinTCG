namespace TCGBase
{
    public record CardRecordAction : CardRecordBase
    {
        public CardRecordAction(string nameID, bool hidden, CardType cardType, List<string> skillList, List<string> tags, int maxNumPermitted = 2) : base(nameID, hidden, cardType, skillList, tags)
        {
            MaxNumPermitted = maxNumPermitted;
        }
        public int MaxNumPermitted { get; }
        //TODO: choose condition & select condition
    }
}
