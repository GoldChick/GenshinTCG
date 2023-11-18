using System;
using System.Diagnostics.CodeAnalysis;

namespace TCGBase
{
    public class PersistentSet
    {
        public int PersistentRegion { get; init; }
    }
    public delegate void EventPersistentSetHandler(PlayerTeam me, AbstractSender s, AbstractVariable? v);
    public class PersistentSet<T> : PersistentSet where T : ICardPersistent
    {
        private readonly List<Persistent<T>> _data;
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
        internal PlayerTeam Team { get; }
        /// <param name="region">
        /// 用来表明persistent在谁身上，在加入PersistentSet时赋值:<br/>
        /// -1=团队 0-5=角色 11=召唤物 12=支援区
        /// </param>
        internal PersistentSet(int region, PlayerTeam team, int size = 0, bool multisame = false)
        {
            PersistentRegion = region;
            _data = new();
            _handlers = new();
            MaxSize = size;
            MultiSame = multisame;
            Team = team;
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
                int index = _data.FindIndex(p => p.Type == input.Type);
                if (!MultiSame && index >= 0 && _data[index] is Persistent<T> t && t.Card.NameID == input.Card.NameID && t.Card.Namespace == input.Card.Namespace)
                {
                    if (t.Active && t.Card.Variant == input.Card.Variant)
                    {
                        input.Card.Update(t);
                        Team.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.TriggerUpdate(Team.TeamIndex, PersistentRegion, index, t.AvailableTimes));
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
        public void Update()
        {
            while (_data.Any(p => !p.Active))
            {
                Clear(p => !p.Active);
            }
        }
        public bool Contains(Type type) => _data.Exists(e => e.Type == type);
        public bool Contains(string nameSpace, string nameID, int variant) => _data.Exists(e => e.Card.Namespace == nameSpace && e.Card.NameID == nameID && e.Card.Variant == variant);
        public bool Contains(string nameSpace, string nameID) => _data.Exists(e => e.Card.Namespace == nameSpace && e.Card.NameID == nameID);
        /// <summary>
        /// 通过nameID来比较，不过不比较nameSpace，因此可能会有重复
        /// </summary>
        public bool Contains(string nameID) => _data.Exists(e => e.Card.NameID == nameID);
        public Persistent<T>? Find(Type type) => _data.Find(e => e.Type == type);
        public Persistent<T>? Find(string nameSpace, string nameID) => _data.Find(e => e.Card.Namespace == nameSpace && e.Card.NameID == nameID);
        public Persistent<T>? Find(string nameSpace, string nameID, int variant) => _data.Find(e => e.Card.Namespace == nameSpace && e.Card.NameID == nameID && e.Card.Variant == variant);
        public Persistent<T>? Find(string nameID) => _data.Find(e => e.Card.NameID == nameID);
        public void EffectTrigger(PlayerTeam me, AbstractSender sender, AbstractVariable? variable)
        {
            if (_handlers.TryGetValue(sender.SenderName, out var hs))
            {
                hs?.Invoke(me, sender, variable);
            }
            //任意行动的主体必须是队伍
            if (sender.TeamID != -1 && _handlers.TryGetValue(SenderTag.AfterAnyAction.ToString(), out hs))
            {
                hs?.Invoke(me, sender, variable);
            }
        }
        public List<Persistent<T>> Copy() => _data.ToList();
        public void TryRemoveAt(int index)
        {
            if (_data.Count > index && index >= 0)
            {
                var d = _data[index];
                d.Active = false;
                Update();
            }
        }
        internal void TryRemove(Type type) => TryRemoveAt(_data.FindIndex(e => e.Type == type));
        //public void TryRemove(string textureNameID) => RemoveSingle(_data.Find(e => e.Card.NameID == textureNameID));
        //public void TryRemove(string textureNamespace, string textureNameID) => RemoveSingle(_data.Find(e => e.Card.Namespace == textureNamespace && e.Card.NameID == textureNameID));
        ///// <param name="p">确定存在的</param>
        //private void RemoveSingle(Persistent<T>? p)
        //{
        //    if (p != null)
        //    {
        //        p.Childs.ForEach(c => c.Active = false);
        //        p.Father?.Childs.Remove(p);
        //        Unregister(p);
        //        _data.Remove(p);
        //    }
        //}
        internal void Clear(Func<Persistent<T>, bool>? condition = null)
        {
            for (int i = _data.Count - 1; i >= 0; i--)
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
        private EventPersistentSetHandler PersistentHandelerConvert(Persistent<T> p, EventPersistentHandler value)
        {
            return (me, s, v) =>
            {
                if (p.Active)
                {
                    int index = _data.FindIndex(d => d.Equals(p));
                    value.Invoke(me, p, s, v);
                    if (index >= 0)
                    {
                        Team.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.TriggerUpdate(Team.TeamIndex, PersistentRegion, index, p.AvailableTimes));
                    }
                }
            };
        }
        private void Register(Persistent<T> p)
        {
            //TODO:是否需要在其他地方额外统一注册kvp.key
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
            Team.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.ObtainUpdate(Team.TeamIndex, PersistentRegion, p.Card.Variant, p.AvailableTimes, p.Card.Namespace, p.Card.NameID));
        }
        private void Unregister(int index, Persistent<T> p)
        {
            foreach (var kvp in p.Card.TriggerDic)
            {
                _handlers[kvp.Key] -= PersistentHandelerConvert(p, kvp.Value);
            }
            Team.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.LoseUpdate(Team.TeamIndex, PersistentRegion, index));
            _data.RemoveAt(index);
        }
    }
}
