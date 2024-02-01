using System.Collections;
using System.Diagnostics.CodeAnalysis;
namespace TCGBase
{
    /// <param name="me">team me</param>
    /// <param name="p">this buff</param>
    /// <param name="s">the message sender</param>
    /// <param name="v">possible things to change</param>
    public delegate void EventPersistentHandler(AbstractTeam me, AbstractPersistent p, AbstractSender s, AbstractVariable? v);


    public class PersistentTriggerList : IEnumerable<IPersistentTrigger>
    {
        private readonly List<IPersistentTrigger> _list;
        public PersistentTriggerList()
        {
            _list = new();
        }
        internal class PersistentTrigger : IPersistentTrigger
        {
            public EventPersistentHandler Handler;
            public string Tag { get; }
            public PersistentTrigger(string tag, EventPersistentHandler h)
            {
                Tag = tag;
                Handler = h;
            }
            public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable) => Handler.Invoke(me, persitent, sender, variable);
        }
        internal void Add(SenderTagInner st, EventPersistentHandler h) => Add(new PersistentTrigger(st.ToString(), h));
        public void Add(SenderTag st, EventPersistentHandler h) => Add(new PersistentTrigger(st.ToString(), h));
        public void Add(string st, EventPersistentHandler h) => _list.Add(new PersistentTrigger(st, h));
        public void Add(IPersistentTrigger t) => _list.Add(t);
        //public IPersistentTrigger this[string st] { get => _list[st]; internal set => _list[st] = value; }
        public bool TryGetValue(string st, [NotNullWhen(returnValue: true)] out IPersistentTrigger? h) => (h = _list.Find(p => p.Tag == st)) != null;
        public bool ContainsKey(string st) => _list.Any(p => p.Tag == st);
        public bool Any() => _list.Any();
        public IEnumerator<IPersistentTrigger> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}
