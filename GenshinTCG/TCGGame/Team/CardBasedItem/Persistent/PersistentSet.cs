using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using TCGBase;
using TCGCard;
using TCGUtil;

namespace TCGGame
{
    public class PersistentSet
    {
    }
    public class PersistentSet<T> : PersistentSet, IPrintable where T : IPersistent
    {
        /// <summary>
        /// 为正代表最多x个，为负或0代表无限制
        /// </summary>
        public int MaxSize { get; init; }
        /// <summary>
        /// 是否可以存在相同id的东西
        /// </summary>
        public bool MultiSame { get; init; }

        private readonly List<AbstractPersistent<T>> _data;
        public int Count => _data.Count;
        public bool Full => MaxSize > 0 && MaxSize <= Count;

        public PersistentSet(int size = 0, bool multisame = false, List<AbstractPersistent<T>>? data = null)
        {
            _data = data ?? new();
            MaxSize = size;
            MultiSame = multisame;
        }
        public AbstractPersistent<T> this[int i] => _data[i];
        public void Add([NotNull] AbstractPersistent<T> input)
        {
            if (MaxSize <= 0 || _data.Count < MaxSize)
            {
                if (!MultiSame && _data.Find(p => p.NameID == input.NameID) is AbstractPersistent<T> t)
                {
                    //NOTE:按照最初的设计，在触发effect的时候不会改变effect（日后可能会导致一些bug）
                    if (t.Active)
                    {
                        t.AvailableTimes = t.Card.MaxUseTimes;
                        t.Data = null;
                    }
                    else
                    {
                        throw new NotImplementedException("PersistentSet.Add():更新了已经存在，但并非active的effect!");
                    }
                }
                else
                {
                    _data.Add(input);
                }
            }
        }
        public void RemoveAt(int index) => _data.RemoveAt(index);
        public int Update() => _data.RemoveAll(p => !p.Active);
        public void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            foreach (var e in _data)
            {
                //TODO:UNKNOWN GAME STEP
                e.EffectTrigger(game, meIndex, sender, variable);
                game.Step();
            }
        }

        public void Print()
        {
            foreach (var e in _data)
            {
                Logger.Print($"{e.NameID} 可用次数{e.AvailableTimes}/{e.Card.MaxUseTimes} {JsonSerializer.Serialize(e.Card.Tags)}");
            }
        }
    }
}
