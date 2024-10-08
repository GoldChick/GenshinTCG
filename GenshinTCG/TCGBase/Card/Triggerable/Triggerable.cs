﻿namespace TCGBase
{
    internal sealed class Triggerable : AbstractTriggerable
    {
        public Triggerable(string tag, EventPersistentHandler? action = null)
        {
            NameID = "default";//never set in this case
            Action = action;
            Tag = tag;
        }
        public Triggerable(string tag, Func<AbstractTriggerable, EventPersistentHandler>? func)
        {
            NameID = "default";//never set in this case
            Action = func?.Invoke(this);
            Tag = tag;
        }
        public EventPersistentHandler? Action { get; internal set; }
        public override string NameID { get; protected set; }
        public override string Tag { get; }
        public override void Trigger(PlayerTeam me, Persistent persitent, SimpleSender sender, AbstractVariable? variable) => Action?.Invoke(me, persitent, sender, variable);
    }
}
