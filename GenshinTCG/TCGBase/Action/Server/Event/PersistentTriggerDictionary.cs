using System.Collections;
using System.Diagnostics.CodeAnalysis;
namespace TCGBase
{
    /// <param name="me">team me</param>
    /// <param name="p">this buff</param>
    /// <param name="s">the message sender</param>
    /// <param name="v">possible things to change</param>
    public delegate void EventPersistentHandler(AbstractTeam me, AbstractPersistent p, AbstractSender s, AbstractVariable? v);


    public class PersistentTriggerDictionary : IEnumerable<KeyValuePair<string, IPersistentTrigger>>
    {
        private readonly Dictionary<string, IPersistentTrigger> _dic;
        public PersistentTriggerDictionary()
        {
            _dic = new();
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
        public void Add(string st, EventPersistentHandler h) => _dic.Add(st, new PersistentTrigger(st, h));
        public void Add(IPersistentTrigger t) => _dic.Add(t.Tag.ToString(), t);

        public IPersistentTrigger this[string st] { get => _dic[st]; internal set => _dic[st] = value; }
        public bool TryGetValue(string st, [NotNullWhen(returnValue: true)] out IPersistentTrigger? h) => _dic.TryGetValue(st, out h);
        public bool ContainsKey(string st) => _dic.ContainsKey(st);
        public bool Any() => _dic.Any();
        public IEnumerator<KeyValuePair<string, IPersistentTrigger>> GetEnumerator() => _dic.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dic.GetEnumerator();
    }
}
