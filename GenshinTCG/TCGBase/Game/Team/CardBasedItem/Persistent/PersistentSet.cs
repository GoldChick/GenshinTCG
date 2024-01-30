using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace TCGBase
{
    public class PersistentSet
    {
        public int PersistentRegion { get; init; }
    }
    public delegate void EventPersistentSetHandler(AbstractTeam me, AbstractSender s, AbstractVariable? v);
    public class PersistentSet<T> : PersistentSet, IEnumerable<Persistent<T>> where T : ICardPersistent
    {
        private readonly List<Persistent<T>> _data;
        private readonly PlayerTeam? _me;
        private readonly Dictionary<string, EventPersistentSetHandler?> _handlers;
        /// <summary>
        /// 为正代表最多x个，为负或0代表无限制
        /// </summary>
        public int MaxSize { get; init; }
        /// <summary>
        /// 是否可以存在相同id的东西
        /// </summary>
        public bool MultiSame { get; init; }
        public int Count => _data.Count;
        public bool Full => MaxSize > 0 && MaxSize <= Count;
        /// <param name="region">
        /// 用来表明persistent在谁身上，在加入PersistentSet时赋值:<br/>
        /// -1=团队 0-5=角色 11=召唤物 12=支援区
        /// </param>
        internal PersistentSet(int region, AbstractTeam team, int size = 0, bool multisame = false)
        {
            PersistentRegion = region;
            _data = new();
            _handlers = new();
            MaxSize = size;
            MultiSame = multisame;
            //TODO: check it :可能会引起什么报错，不过暂时不关心
            if (team is PlayerTeam pt)
            {
                _me = pt;
            }
        }
        public Persistent<T> this[int i] => _data[i];
        /// <summary>
        /// 无则加入（未满），有则刷新
        /// </summary>
        internal void Add([NotNull] Persistent<T> input)
        {
            input.PersistentRegion = PersistentRegion;
            Update();
            if (MaxSize <= 0 || _data.Count < MaxSize)
            {
                int index = _data.FindIndex(p => p.Type == input.Type);
                if (!MultiSame && index >= 0 && _data[index] is Persistent<T> t && t.Card.NameID == input.Card.NameID && t.Card.Namespace == input.Card.Namespace)
                {
                    if (t.Card.Variant % 10 == input.Card.Variant % 10)
                    {
                        input.Card.Update(_me, t);
                        _me.RealGame.BroadCast(ClientUpdateCreate.PersistentUpdate.TriggerUpdate(_me.TeamIndex, PersistentRegion, index, t.AvailableTimes));
                    }
                    else
                    {
                        TryRemoveAt(index);
                        Register(input);
                    }
                }
                else
                {
                    Register(input);
                }
            }
        }
        internal void Update()
        {
            while (_data.Any(p => !p.Active))
            {
                Clear(p => !p.Active);
            }
        }
        /// <summary>
        /// 是否包含某type的子类
        /// </summary>
        public bool Contains(Type type) => _data.Exists(e => e.Type.IsAssignableTo(type));
        public bool Contains(int variant) => _data.Exists(e => (e.Card.Variant % 10) == variant);
        public bool Contains(string nameSpace, string nameID, int variant) => _data.Exists(e => e.Card.Namespace == nameSpace && e.Card.NameID == nameID && (e.Card.Variant % 10) == variant);
        public bool Contains(string nameSpace, string nameID) => _data.Exists(e => e.Card.Namespace == nameSpace && e.Card.NameID == nameID);
        public Persistent<T>? Find(Predicate<Persistent<T>> condition) => _data.Find(condition);
        /// <summary>
        /// 找到第一个某type的子类
        /// </summary>
        public Persistent<T>? Find(Type type) => _data.Find(e => e.Type.IsAssignableTo(type));
        public Persistent<T>? Find(string nameSpace, string nameID) => _data.Find(e => e.Card.Namespace == nameSpace && e.Card.NameID == nameID);
        public Persistent<T>? Find(string nameSpace, string nameID, int variant) => _data.Find(e => e.Card.Namespace == nameSpace && e.Card.NameID == nameID && (e.Card.Variant % 10) == variant);
        public Persistent<T>? Find(int variant) => _data.Find(e => (e.Card.Variant % 10) == variant);
        public bool TryFind(Predicate<Persistent<T>> condition, [NotNullWhen(true)] out Persistent<T>? p)
        {
            p = _data.Find(condition);
            return p != null;
        }
        /// <summary>
        /// 找到第一个某type的子类
        /// </summary>
        public bool TryFind(Type type, [NotNullWhen(true)] out Persistent<T>? p)
        {
            p = _data.Find(e => e.Type.IsAssignableTo(type));
            return p != null;
        }
        public bool TryFind(string nameSpace, string nameID, [NotNullWhen(true)] out Persistent<T>? p)
        {
            p = _data.Find(e => e.Card.Namespace == nameSpace && e.Card.NameID == nameID);
            return p != null;
        }
        public bool TryFind(string nameSpace, string nameID, int variant, [NotNullWhen(true)] out Persistent<T>? p)
        {
            p = _data.Find(e => e.Card.Namespace == nameSpace && e.Card.NameID == nameID && (e.Card.Variant % 10) == variant);
            return p != null;
        }
        public bool TryFind(int variant, [NotNullWhen(true)] out Persistent<T>? p)
        {
            p = _data.Find(e => (e.Card.Variant % 10) == variant);
            return p != null;
        }
        internal EventPersistentSetHandler? GetPersistentHandlers(AbstractSender sender)
        {
            EventPersistentSetHandler? acs = null;
            if (_handlers.TryGetValue(sender.SenderName, out var hs))
            {
                acs += hs;
            }
            //任意行动的主体必须是队伍
            if (sender.TeamID != -1 && _handlers.TryGetValue(SenderTag.AfterAnyAction.ToString(), out hs))
            {
                acs += hs;
            }
            return acs;
        }
        public List<Persistent<T>> Copy() => _data.ToList();
        public void TryRemoveAt(int index)
        {
            //TODO:弃置状态..
            if (_data.Count > index && index >= 0)
            {
                var d = _data[index];
                d.Active = false;
                Update();
            }
        }
        /// <summary>
        /// 尝试清除第一个指定Type及其子类
        /// </summary>
        /// <param name="type"></param>
        public void TryRemove(Type type) => TryRemoveAt(_data.FindIndex(e => e.Type.IsAssignableTo(type)));
        /// <summary>
        /// 用来清理第一个[武器][装备][圣遗物]
        /// </summary>
        public void TryRemove(int variant) => TryRemoveAt(_data.FindIndex(e => (e.Card.Variant % 10) == variant));
        internal void Clear(Func<Persistent<T>, bool>? condition = null)
        {
            for (int i = _data.Count - 1; i >= 0; i--)
            {
                //TODO:似乎有很多的循环调用，比如Ondesperated
                if (_data.Count > i)
                {
                    var d = _data[i];
                    if (condition == null || condition(d))
                    {
                        d.Childs.ForEach(c => c.Active = false);
                        d.Father?.Childs.Remove(d);
                        Unregister(i, d);
                    }
                }
            }
        }
        private EventPersistentSetHandler PersistentHandelerConvert(Persistent<T> p, IPersistentTrigger value)
        {
            return (me, s, v) =>
            {
                if (p.Active)
                {
                    value.Trigger(me, p, s, v);
                    int index = _data.FindIndex(d => d == p);
                    if (index >= 0)
                    {
                        _me.RealGame.BroadCast(ClientUpdateCreate.PersistentUpdate.TriggerUpdate(_me.TeamIndex, PersistentRegion, index, p.AvailableTimes));
                    }
                }
            };
        }
        private void Register(Persistent<T> p)
        {
            _data.Add(p);
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
            _me.RealGame.BroadCast(ClientUpdateCreate.PersistentUpdate.ObtainUpdate(_me.TeamIndex, PersistentRegion, p.Card.Variant, p.AvailableTimes, p.Card.Namespace, p.Card.NameID));
        }
        private void Unregister(int index, Persistent<T> p)
        {
            foreach (var kvp in p.Card.TriggerDic)
            {
                _handlers[kvp.Key] -= PersistentHandelerConvert(p, kvp.Value);
            }
            _me.RealGame.BroadCast(ClientUpdateCreate.PersistentUpdate.LoseUpdate(_me.TeamIndex, PersistentRegion, index));
            _data.RemoveAt(index);
            //TODO: unregister here
            _me.RealGame.EffectTrigger(new PersistentDesperatedSender(_me.TeamIndex, p.PersistentRegion, p.Card), null);
        }

        public IEnumerator<Persistent<T>> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();
    }
}
