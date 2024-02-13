﻿using System.Text.Json;

namespace TCGBase
{
    internal class TriggerableCard : AbstractTriggerable
    {
        public EventPersistentHandler? Handler;
        public TriggerableCard(TriggerableRecordBase card) : base()
        {
            NameID = "usecard";//set in INameSetable
            Handler = null;
            foreach (var item in card.Action)
            {
                Handler += item.GetHandler(this);
            }
        }
        public override string NameID { get; protected set; }

        public override string Tag => SenderTagInner.UseCard.ToString();

        public override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            Handler?.Invoke(me, persitent, sender, variable);
        }
    }
}
