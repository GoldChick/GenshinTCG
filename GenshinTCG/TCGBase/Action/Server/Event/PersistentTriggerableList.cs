﻿using System.Collections;
using System.Diagnostics.CodeAnalysis;
namespace TCGBase
{
    /// <param name="me">team me</param>
    /// <param name="p">this buff</param>
    /// <param name="s">the message sender</param>
    /// <param name="v">possible things to change</param>
    public delegate void EventPersistentHandler(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v);
    public class PersistentTriggerableList : IEnumerable<AbstractCustomTriggerable>
    {
        private readonly List<AbstractCustomTriggerable> _list;
        public PersistentTriggerableList(List<AbstractCustomTriggerable>? list = null)
        {
            _list = list ?? new();
        }
        internal void Add(SenderTagInner st, EventPersistentHandler h) => Add(new Triggerable(st.ToString(), h));
        public void Add(SenderTag st, EventPersistentHandler h) => Add(new Triggerable(st.ToString(), h));
        public void Add(string st, EventPersistentHandler h) => _list.Add(new Triggerable(st, h));
        public void Add(AbstractCustomTriggerable t) => _list.Add(t);
        /// <summary>
        /// 找到第index个满足要求的触发，自动clamp到0-(size-1)
        /// </summary>
        public bool TryGetValue(string st, [NotNullWhen(returnValue: true)] out AbstractCustomTriggerable? h, int index = 0)
        {
            var targets = _list.Where(p => p.Tag == st);
            bool any = targets.Any();
            h = any ? targets.ElementAt(int.Clamp(index, 0, targets.Count() - 1)) : null;
            return any;
        }
        public List<AbstractCustomTriggerable> this[string st] => GetAllValue(st);
        public List<AbstractCustomTriggerable> GetAllValue(string st) => _list.Where(p => p.Tag == st).ToList();
        /// <summary>
        /// 查看是否有count个tag满足要求的触发，自动count>=1
        /// </summary>
        public bool ContainsKey(string st, int count = 1) => _list.Where(p => p.Tag == st).Count() == int.Min(1, count);
        public bool Any() => _list.Any();
        public IEnumerator<AbstractCustomTriggerable> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}
