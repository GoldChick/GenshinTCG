using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using TCGBase;
using TCGCard;
using TCGUtil;

namespace TCGGame
{
    public class PersistentSet
    {
        public int PersistentRegion { get; init; }
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
        /// <summary>
        /// 正在遍历中的缓存
        /// </summary>
        private List<AbstractPersistent<T>> _cash;
        public int Count => _data.Count;
        public bool Full => MaxSize > 0 && MaxSize <= Count;

        /// <param name="region">
        /// 用来表明persistent在谁身上，在加入PersistentSet时赋值:<br/>
        /// -1=团队 0-5=角色 11=召唤物 12=支援区
        /// </param>
        public PersistentSet(int region, int size = 0, bool multisame = false, List<AbstractPersistent<T>>? data = null)
        {
            PersistentRegion = region;
            _data = data ?? new();
            _cash = new();
            MaxSize = size;
            MultiSame = multisame;
        }
        public AbstractPersistent<T> this[int i] => _data[i];
        /// <summary>
        /// 无则加入（未满），有则刷新
        /// </summary>
        public void Add([NotNull] AbstractPersistent<T> input)
        {
            if (_during)
            {
                _cash.Add(input);
            }
            else
            {
                input.PersistentRegion = PersistentRegion;
                if (MaxSize <= 0 || _data.Count < MaxSize)
                {
                    if (!MultiSame && _data.Find(p => p.NameID == input.NameID) is AbstractPersistent<T> t)
                    {
                        if (t.Active)
                        {
                            //TODO:不好看，以后改
                            if (t is AbstractPersistent<AbstractCardPersistentSummon> cs)
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
                    }
                }
            }
        }
        public void RemoveAt(int index) => _data.RemoveAt(index);
        public int Update() => _during ? 0 : _data.RemoveAll(p => !p.Active);
        public bool Contains(string nameid) => _data.Exists(e => e.NameID == nameid);
        public AbstractPersistent<T>? TryGet(string nameid) => _data.Find(e => e.NameID == nameid);
        public void EffectTrigger(PlayerTeam me, AbstractSender sender, AbstractVariable? variable)
        {
            Action a = () =>
            {
                foreach (var e in _data)
                {
                    //TODO:UNKNOWN GAME STEP
                    if (e.Active)
                    {
                        e.EffectTrigger(me, sender, variable);
                    }
                }
            };
            if (_during)
            {
                a.Invoke();
            }
            else
            {
                _during = true;
                a.Invoke();
                _during = false;
                _cash.ForEach(Add);
                _cash.Clear();
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
