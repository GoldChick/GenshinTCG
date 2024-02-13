namespace TCGBase
{
    internal sealed class Triggerable : AbstractTriggerable
    {
        public Triggerable(TriggerableRecordBaseImplement record)
        {
            NameID = "default";//never set in this case
            Action = null;
            foreach (var item in record.Action)
            {
                Action += item.GetHandler(this);
            }
            Tag = record.Type.ToString();
        }
        public Triggerable(string tag, EventPersistentHandler? action = null)
        {
            NameID = "default";//never set in this case
            Action = action;
            Tag = tag;
        }
        public EventPersistentHandler? Action { get; internal set; }
        public override string NameID { get; protected set; }
        public override string Tag { get; }
        public override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable) => Action?.Invoke(me, persitent, sender, variable);
    }
}
