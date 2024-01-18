using System.Collections;
using System.Diagnostics.CodeAnalysis;
namespace TCGBase
{
    /// <param name="me">team me</param>
    /// <param name="p">this buff</param>
    /// <param name="s">the message sender</param>
    /// <param name="v">possible things to change</param>
    public delegate void EventPersistentHandler(PlayerTeam me, AbstractPersistent p, AbstractSender s, AbstractVariable? v);


    public class PersistentTriggerDictionary : IEnumerable<KeyValuePair<string, IPersistentTrigger>>
    {
        public static PersistentTriggerDictionary EmptyTriggerDic => _emptytriggerdic;
        private static PersistentTriggerDictionary _emptytriggerdic;
        private readonly Dictionary<string, IPersistentTrigger> _dic;
        public PersistentTriggerDictionary()
        {
            _dic = new();
            _emptytriggerdic ??= new();
        }
        private class PersistentTrigger : IPersistentTrigger
        {
            private readonly EventPersistentHandler _h;
            public string Tag { get; }
            public PersistentTrigger(string tag, EventPersistentHandler h)
            {
                Tag = tag;
                _h = h;
            }
            public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable) => _h.Invoke(me, persitent, sender, variable);
        }
        internal void Add(SenderTagInner st, EventPersistentHandler h) => Add(new PersistentTrigger(st.ToString(), h));
        public void Add(SenderTag st, EventPersistentHandler h) => Add(new PersistentTrigger(st.ToString(), h));
        public void Add(string st, EventPersistentHandler h) => _dic.Add(st, new PersistentTrigger(st, h));
        public void Add(IPersistentTrigger t) => _dic.Add(t.Tag.ToString(), t);

        public IPersistentTrigger this[string st] => _dic[st];
        public bool TryGetValue(string st, [NotNullWhen(returnValue: true)] out IPersistentTrigger? h) => _dic.TryGetValue(st, out h);
        public bool ContainsKey(string st) => _dic.ContainsKey(st);
        public bool Any() => _dic.Any();
        public IEnumerator<KeyValuePair<string, IPersistentTrigger>> GetEnumerator() => _dic.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dic.GetEnumerator();
    }
}
