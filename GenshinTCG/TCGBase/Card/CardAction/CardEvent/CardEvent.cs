﻿namespace TCGBase
{
    internal sealed class CardEvent : AbstractCardEvent
    {
        public override TargetDemand[] TargetDemands { get; }
        public CardEvent(CardRecordEvent record) : base(record)
        {
        }
    }
}
