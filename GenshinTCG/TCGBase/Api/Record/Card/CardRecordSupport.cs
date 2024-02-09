﻿namespace TCGBase
{
    public record CardRecordSupport : CardRecordAction
    {
        public CardRecordSupport(string nameID, CardType cardType, List<TriggerableRecordBase> skillList, List<string> tags, List<CostRecord>? cost = null, bool hidden = false, int maxNumPermitted = 2) : base(nameID, cardType, skillList, tags, cost, hidden, maxNumPermitted)
        {
        }
        public override AbstractCardAction GetCard() => new CardSupport(this);
    }
}
