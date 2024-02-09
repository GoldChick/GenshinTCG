namespace TCGBase
{
    public record CardRecordEquipment : CardRecordAction
    {
        public CardRecordEquipment(string nameID, CardType cardType, List<string> skillList, List<string> tags, List<CostRecord>? cost = null, bool hidden = false, int maxNumPermitted = 2) : base(nameID, cardType, skillList, tags, cost, hidden, maxNumPermitted)
        {
        }
        public override AbstractCardAction GetCard(string @namespace) => new CardEquipment(this,@namespace);
    }
}
