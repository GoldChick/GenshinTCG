using System.Diagnostics.CodeAnalysis;
using TCGBase;
using TCGCard;
using TCGUtil;

namespace TCGGame
{
    public class PersistentSet
    {
    }
    public class PersistentSet<T> : PersistentSet, IPrintable where T : AbstractCardPersistent
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
        /// <summary>
        /// 正在遍历
        /// </summary>
        private bool _during;
        public int Count => _data.Count;
        public bool Full => MaxSize > 0 && MaxSize <= Count;

        public PersistentSet(int size = 0, bool multisame = false, List<AbstractPersistent<T>>? data = null)
        {
            _data = data ?? new();
            MaxSize = size;
            MultiSame = multisame;
        }
        public AbstractPersistent<T> this[int i] => _data[i];
        /// <summary>
        /// 无则加入（未满），有则刷新
        /// </summary>
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
        public int Update() => _during ? 0 : _data.RemoveAll(p => !p.Active);
        public bool Contains(string nameid) => _data.Exists(e => e.NameID == nameid);
        public void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            if (_during)
            {
                Logger.Error($"PersistentSet.EffectTrigger():结算sendertype={sender.SenderName}进入了嵌套的buff结算！");
                foreach (var e in _data)
                {
                    //TODO:UNKNOWN GAME STEP
                    if (e.Active)
                    {
                        e.EffectTrigger(game, meIndex, sender, variable);
                    }
                }
            }
            else
            {
                _during = true;
                foreach (var e in _data)
                {
                    //TODO:UNKNOWN GAME STEP
                    if (e.Active)
                    {
                        e.EffectTrigger(game, meIndex, sender, variable);
                    }
                }
                _during = false;
            }
        }

        public void Print()
        {
            foreach (var e in _data)
            {
                Logger.Print($"{e.NameID} 可用次数{e.AvailableTimes}/{e.Card.MaxUseTimes}");
            }
        }
    }
}
