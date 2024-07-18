using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace TCGBase
{
    public delegate void EventPersistentSetHandler(SimpleSender s, AbstractVariable? v);
    public abstract class AbstractPersistentSet
    {
        public int PersistentRegion { get; init; }
        protected readonly PlayerTeam _me;
        protected private AbstractPersistentSet(PlayerTeam me)
        {
            _me = me;
        }
    }
    public class PersistentSet : AbstractPersistentSet, IEnumerable<Persistent>
    {
        private readonly List<Persistent> _data;
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
        internal PersistentSet(int region, PlayerTeam me, int size = 0, bool multisame = false) : base(me)
        {
            PersistentRegion = region;
            _data = new();
            _handlers = new();
            MaxSize = size;
            MultiSame = multisame;
        }
        public Persistent this[int i] => _data[i];
        /// <summary>
        /// 返回是否加入/刷新成功
        /// </summary>
        internal bool Add([NotNull] Persistent input)
        {
            Update();
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
                _me.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.TriggerUpdate(_me.TeamID, PersistentRegion, index, t.AvailableTimes, t.Data));
                return true;
            }
            else if (MaxSize <= 0 || _data.Count < MaxSize)
            {
                input.Owner = this;
                input.PersistentRegion = PersistentRegion;
                _me.Game.RegisterPersistent(input);
                Register(input);
                return true;
            }
            return false;
        }
        internal void Update()
        {
            while (_data.Any(p => !p.Active))
            {
                Clear(p => !p.Active);
            }
        }
        public bool Contains(INameable nameable) => _data.Exists(e => e.CardBase.Namespace == nameable.Namespace && e.CardBase.NameID == nameable.NameID);
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
        internal List<EventPersistentSetHandler> GetPersistentHandlers(string sendertag, SimpleSender sender)
        {
            List<EventPersistentSetHandler> acs = new();
            if (sender is not (ActionUseSkillSender))
            {
                foreach (var h in _handlers)
                {
                    if (h.Key == sendertag && h.Value != null)
                    {
                        acs.Add(h.Value);
                    }
                }
            }
            return acs;
        }
        public List<Persistent> Copy() => _data.ToList();
        internal void Clear(Func<Persistent, bool>? condition = null)
        {
            for (int i = _data.Count - 1; i >= 0; i--)
            {
                if (_data.Count > i)
                {
                    var d = _data[i];
                    if (condition == null || condition(d))
                    {
                        d.Childs.ForEach(c => _me.Game.GlobalPersistents[c].Active = false);
                        Unregister(i, d);
                    }
                }
            }
        }
        private IEnumerable<KeyValuePair<string, EventPersistentSetHandler>> HandlerToPersistentSet(Persistent p)
        {
            return p.CardBase.TriggerableList.GroupBy(it => it.Tag).Select(ig => new KeyValuePair<string, EventPersistentSetHandler>(ig.Key, (s, v) =>
            {
                foreach (var it in ig)
                {
                    if (!p.Active)
                    {
                        break;
                    }
                    it.Trigger(_me, p, s, v);
                }
                int index = _data.FindIndex(d => d == p);
                if (index >= 0)
                {
                    _me.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.TriggerUpdate(_me.TeamID, PersistentRegion, index, p.AvailableTimes, p.Data));
                }
                Update();
            }));
        }
        private void Register(Persistent p)
        {
            _data.Add(p);
            var handlersDic = HandlerToPersistentSet(p);
            foreach (var kvp in handlersDic)
            {
                if (!_handlers.ContainsKey(kvp.Key))
                {
                    _handlers.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    _handlers[kvp.Key] += kvp.Value;
                }
            }
            _me.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.ObtainUpdate(_me.TeamID, p));
        }
        private void Unregister(int index, Persistent p)
        {
            var handlersDic = HandlerToPersistentSet(p);
            foreach (var kvp in handlersDic)
            {
                _handlers[kvp.Key] -= kvp.Value;
            }
            _me.Game.BroadCast(ClientUpdateCreate.PersistentUpdate.LoseUpdate(_me.TeamID, PersistentRegion, index));
            _data.RemoveAt(index);
        }
        public void PopTo(Persistent p, AbstractPersistentSet destination)
        {
            int index = _data.FindIndex(pe => pe == p);
            if (index > 0)
            {
                if (destination is CardsInHand inhand)
                {
                    if (p.CardBase is AbstractCardAction action)
                    {
                        Unregister(index, p);
                        inhand.Add(action);
                    }
                }
                else if (destination is PersistentSet set && !set.Full)
                {
                    Unregister(index, p);
                    set.Add(p);
                }
            }
        }
        public void Destroy(int index)
        {
            var p = _data.ElementAtOrDefault(index);
            if (p != null)
            {
                Unregister(index, p);
                _me.Game.EffectTrigger(SenderTag.AfterEffectDesperated, new PersistentDesperatedSender(_me.TeamID, PersistentRegion, p.CardBase));
            }
        }
        public void DestroyFirst(Predicate<Persistent> condition) => Destroy(_data.FindIndex(condition));
        public IEnumerator<Persistent> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();
    }
}
