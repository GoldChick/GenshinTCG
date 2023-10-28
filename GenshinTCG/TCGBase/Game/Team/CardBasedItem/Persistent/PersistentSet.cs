using System.Diagnostics.CodeAnalysis;
using TCGBase;
 

namespace TCGBase
{
    public class PersistentSet
    {
        public int PersistentRegion { get; init; }
    }
    public delegate void EventPersistentSetHandler(PlayerTeam me, AbstractSender s, AbstractVariable? v);
    public class PersistentSet<T> : PersistentSet where T : AbstractCardPersistent
    {
        /// <summary>
        /// 为正代表最多x个，为负或0代表无限制
        /// </summary>
        public int MaxSize { get; init; }
        /// <summary>
        /// 是否可以存在相同id的东西
        /// </summary>
        public bool MultiSame { get; init; }

        private readonly List<Persistent<T>> _data;

        private readonly Dictionary<string, EventPersistentSetHandler?> _handlers;
        public int Count => _data.Count;
        public bool Full => MaxSize > 0 && MaxSize <= Count;

        /// <param name="region">
        /// 用来表明persistent在谁身上，在加入PersistentSet时赋值:<br/>
        /// -1=团队 0-5=角色 11=召唤物 12=支援区
        /// </param>
        public PersistentSet(int region, int size = 0, bool multisame = false, List<Persistent<T>>? data = null)
        {
            PersistentRegion = region;
            _data = data ?? new();
            _handlers = new();
            MaxSize = size;
            MultiSame = multisame;
        }
        public Persistent<T> this[int i] => _data[i];
        /// <summary>
        /// 无则加入（未满），有则刷新
        /// </summary>
        public void Add([NotNull] Persistent<T> input)
        {
            input.PersistentRegion = PersistentRegion;
            if (MaxSize <= 0 || _data.Count < MaxSize)
            {
                if (!MultiSame && _data.Find(p => p.NameID == input.NameID) is Persistent<T> t)
                {
                    if (t.Active)
                    {
                        //TODO:不好看，以后改
                        if (t is Persistent<AbstractCardPersistentSummon> cs)
                        {
                            cs.Card.Update(cs);
                        }
                        else
                        {
                            t.AvailableTimes = t.Card.MaxUseTimes;
                            t.Data = null;
                        }
                    }
                    else
                    {
                        throw new NotImplementedException($"PersistentSet.Add():更新了已经存在，但并非active的effect:{input.NameID}!");
                    }
                }
                else
                {
                    _data.Add(input);
                    Register(input);
                }
            }
        }
        public void RemoveAt(int index) => _data.RemoveAt(index);
        public void Update()
        {
            while (true)
            {
                var disposes = _data.Where(p => !p.Active).ToList();
                if (disposes.Count == 0)
                {
                    break;
                }
                disposes.ForEach(d =>
                {
                    Unregister(d);
                    d.Childs.ForEach(c => c.Active = false);
                    d.Father?.Childs.Remove(d);
                });
                _data.RemoveAll(p => !p.Active);
            }
        }
        public bool Contains(string nameid) => _data.Exists(e => e.NameID == nameid);
        public Persistent<T>? TryGet(string nameid) => _data.Find(e => e.NameID == nameid);
        public void EffectTrigger(PlayerTeam me, AbstractSender sender, AbstractVariable? variable)
        {
            if (_handlers.TryGetValue(sender.SenderName, out var hs))
            {
                hs?.Invoke(me, sender, variable);
            }
            if (_handlers.TryGetValue(SenderTag.AfterAnyAction.ToString(), out hs))
            {
                hs?.Invoke(me, sender, variable);
            }
        }
        public List<Persistent<T>> Copy() => _data.ToList();
        internal void Clear()
        {
            _data.ForEach(d =>
            {
                Unregister(d);
                d.Childs.ForEach(c => c.Active = false);
                d.Father?.Childs.Remove(d);
            });
            _data.Clear();
            _handlers.Clear();
        }
        private EventPersistentSetHandler PersistentHandelerConvert(Persistent<T> p, EventPersistentHandler value)
        {
            return (me, s, v) =>
            {
                if (p.Active)
                {
                    value.Invoke(me, p, s, v);
                }
            };
        }
        private void Register(Persistent<T> p)
        {
            //TODO:是否需要在其他地方额外统一注册kvp.key
            foreach (var kvp in p.Card.TriggerDic)
            {
                var h = PersistentHandelerConvert(p, kvp.Value);
                if (!_handlers.ContainsKey(kvp.Key))
                {
                    _handlers.Add(kvp.Key, h);
                }
                else
                {
                    _handlers[kvp.Key] += h;
                }
            }
        }
        private void Unregister(Persistent<T> p)
        {
            foreach (var kvp in p.Card.TriggerDic)
            {
                _handlers[kvp.Key] -= PersistentHandelerConvert(p, kvp.Value);
            }
        }
    }
}
