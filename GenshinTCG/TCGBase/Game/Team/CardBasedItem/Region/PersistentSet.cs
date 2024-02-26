using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace TCGBase
{
    public delegate void EventPersistentSetHandler(PlayerTeam me, AbstractSender s, AbstractVariable? v);
    public class PersistentSet<T> : AbstractPersistentSet, IEnumerable<Persistent> where T : AbstractCardBase
    {
        private readonly List<Persistent> _data;
        private readonly PlayerTeam _me;
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
        internal PersistentSet(int region, PlayerTeam team, int size = 0, bool multisame = false)
        {
            PersistentRegion = region;
            _data = new();
            _handlers = new();
            MaxSize = size;
            MultiSame = multisame;
            _me = team;
        }
        public Persistent this[int i] => _data[i];
        /// <summary>
        /// 无则加入（未满），有则刷新
        /// </summary>
        internal void Add([NotNull] Persistent input)
        {
            Update();
            if (MaxSize <= 0 || _data.Count < MaxSize)
            {
                int index = _data.FindIndex(p => p.CardBase.Namespace == input.CardBase.Namespace && p.CardBase.NameID == input.CardBase.NameID);
                if (!MultiSame && index >= 0)
                {
                    var t = _data[index];
                    if (t.CardBase is IEffect ef)
                    {
                        ef.Update(_me, t);
                    }
                    else
                    {
                        t.AvailableTimes = t.CardBase.InitialUseTimes;
                        t.Data.Clear();
                    }
                    _me.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.TriggerUpdate(_me.TeamIndex, PersistentRegion, index, t.AvailableTimes, t.Data));
                }
                else
                {
                    input.PersistentRegion = PersistentRegion;
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
        public bool Contains(string nameSpace, string nameID) => _data.Exists(e => e.CardBase.Namespace == nameSpace && e.CardBase.NameID == nameID);
        public bool Contains(string name) => _data.Exists(e => $"{e.CardBase.Namespace}:{e.CardBase.NameID}" == name);
        public bool Contains(Predicate<Persistent> condition) => _data.Exists(condition);
        public Persistent? Find(Predicate<Persistent> condition) => _data.Find(condition);
        public Persistent? Find(string nameSpace, string nameID) => _data.Find(e => e.CardBase.Namespace == nameSpace && e.CardBase.NameID == nameID);
        public Persistent? Find(string name) => _data.Find(e => $"{e.CardBase.Namespace}:{e.CardBase.NameID}" == name);
        public bool TryFind(Predicate<Persistent> condition, [NotNullWhen(true)] out Persistent? p)
        {
            p = _data.Find(condition);
            return p != null;
        }
        public bool TryFind(string nameSpace, string nameID, [NotNullWhen(true)] out Persistent? p)
        {
            p = _data.Find(e => e.CardBase.Namespace == nameSpace && e.CardBase.NameID == nameID);
            return p != null;
        }
        internal EventPersistentSetHandler? GetPersistentHandlers(AbstractSender sender)
        {
            EventPersistentSetHandler? acs = null;
            if (sender is not (ActionUseCardSender or ActionUseSkillSender))
            {
                if (_handlers.TryGetValue(sender.SenderName, out var hs))
                {
                    acs += hs;
                }
            }
            return acs;
        }
        public List<Persistent> Copy() => _data.ToList();
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
        internal void Clear(Func<Persistent, bool>? condition = null)
        {
            for (int i = _data.Count - 1; i >= 0; i--)
            {
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
        private EventPersistentSetHandler PersistentHandelerConvert(Persistent p, AbstractTriggerable value)
        {
            return Persistent.GetDelayedHandler((me, s, v) =>
            {
                if (p.Active)
                {
                    value.Trigger(me, p, s, v);
                    int index = _data.FindIndex(d => d == p);
                    if (index >= 0)
                    {
                        me.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.TriggerUpdate(me.TeamIndex, PersistentRegion, index, p.AvailableTimes, p.Data));
                    }
                }
            });
        }
        private void Register(Persistent p)
        {
            _data.Add(p);
            foreach (var trigger in p.CardBase.TriggerableList)
            {
                var h = PersistentHandelerConvert(p, trigger);
                if (!_handlers.ContainsKey(trigger.Tag))
                {
                    _handlers.Add(trigger.Tag, h);
                }
                else
                {
                    _handlers[trigger.Tag] += h;
                }
            }
            _me.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.ObtainUpdate(_me.TeamIndex, PersistentRegion, p.AvailableTimes, p.Data, p.CardBase.Namespace, p.CardBase.NameID));
        }
        private void Unregister(int index, Persistent p)
        {
            foreach (var trigger in p.CardBase.TriggerableList)
            {
                _handlers[trigger.Tag] -= PersistentHandelerConvert(p, trigger);
            }
            _me.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.LoseUpdate(_me.TeamIndex, PersistentRegion, index));
            _data.RemoveAt(index);
        }
        public void Destroy(int index)
        {
            var p = _data.ElementAtOrDefault(index);
            if (p != null)
            {
                Unregister(index, p);
                _me.Game.EffectTrigger(new PersistentDesperatedSender(_me.TeamIndex, p.PersistentRegion, p.CardBase), null);
            }
        }
        public void DestroyFirst(Predicate<Persistent> condition) => Destroy(_data.FindIndex(condition));
        public IEnumerator<Persistent> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();
    }
}
