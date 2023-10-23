using System.Collections;
namespace TCGBase
{
    public class PersistentTriggerDic : IEnumerable<KeyValuePair<string, EventPersistentHandler>>
    {
        private readonly Dictionary<string, EventPersistentHandler> _dic;
        public PersistentTriggerDic()
        {
            _dic = new();
        }
        public void Add(SenderTags st, EventPersistentHandler h) => Add(st.ToString(), h);
        public void Add(string st, EventPersistentHandler h) => _dic.Add(st, h);
        public IEnumerator<KeyValuePair<string, EventPersistentHandler>> GetEnumerator() => _dic.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dic.GetEnumerator();
    }
}
