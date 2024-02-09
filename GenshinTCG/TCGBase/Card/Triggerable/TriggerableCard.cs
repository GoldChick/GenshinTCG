namespace TCGBase
{
    internal class TriggerableCard : ITriggerable
    {
        public TriggerableCard(TriggerableRecordCard card)
        {
            EventPersistentHandler? inner = null;
            foreach (var item in card.Action)
            {
                inner += item.GetHandler(this);
            }
            Action = inner;
        }
        public string Tag => SenderTagInner.UseCard.ToString();
        public EventPersistentHandler? Action { get; internal set; }
        public void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable) => throw new Exception("Triggerable Card test");
        //Action?.Invoke(me, persitent, sender, variable)

    }
}
