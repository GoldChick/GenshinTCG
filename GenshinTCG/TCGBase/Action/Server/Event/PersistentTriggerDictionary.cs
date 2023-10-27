using System.Collections;
using TCGGame;

namespace TCGBase
{
    public delegate void EventPersistentHandler(PlayerTeam me, AbstractPersistent p, AbstractSender s, AbstractVariable? v);
    public class PersistentTriggerDictionary : IEnumerable<KeyValuePair<string, EventPersistentHandler>>
    {
        private readonly Dictionary<string, EventPersistentHandler> _dic;
        public PersistentTriggerDictionary()
        {
            _dic = new();
        }
        public PersistentTriggerDictionary(Dictionary<string, EventPersistentHandler> dic) { _dic = dic; }
        public void Add(SenderTag st, EventPersistentHandler h) => Add(st.ToString(), h);
        public void Add(SenderTag st, PersistentTrigger t) => Add(st.ToString(), t);
        public void Add(string st, EventPersistentHandler h) => _dic.Add(st, h);
        public void Add(string st, PersistentTrigger t) => _dic.Add(st, t.Trigger);
        public EventPersistentHandler this[string st] => _dic[st];
        public bool TryGetValue(string st,out EventPersistentHandler? h) => _dic.TryGetValue(st, out h);
        public bool ContainsKey(string st)=> _dic.ContainsKey(st);
        public IEnumerator<KeyValuePair<string, EventPersistentHandler>> GetEnumerator() => _dic.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dic.GetEnumerator();
    }
}
