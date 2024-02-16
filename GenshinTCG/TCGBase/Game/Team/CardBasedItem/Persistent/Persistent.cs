namespace TCGBase
{
    public class Persistent
    {
        protected int _availableTimes;

        public int AvailableTimes
        {
            get => _availableTimes;
            set
            {
                _availableTimes = int.Max(0, value);
                if (CardBase is IEffect ef)
                {
                    Active = ef.CustomDesperated | _availableTimes != 0;
                }
            }
        }

        public AbstractCardBase CardBase { get; }
        /// <summary>
        /// 用来表明persistent在谁身上，在加入PersistentSet时赋值:<br/>
        /// -1=团队 0-5=角色 11=召唤物 12=支援区
        /// </summary>
        public int PersistentRegion { get; internal set; }
        /// <summary>
        /// 是否处于激活状态，默认为True，当某次行动结算结束之后会清除未激活的effect
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// 偷懒所使用的，不过确实有用捏
        /// </summary>
        public Type Type { get; protected init; }
        /// <summary>
        /// 用于[已知class的]persistent储存更加详细的数据
        /// 如[桓纳兰那]
        /// </summary>
        public object? Data { get; set; }
        /// <summary>
        /// 依赖于的另一个persistent，在自己的persistent中编写检测机制
        /// </summary>
        public Persistent? Father { get; set; }
        /// <summary>
        /// 依赖于自己的persistent们，此状态清除时将把list中的其他状态也清除
        /// </summary>
        public List<Persistent> Childs { get; }
        public Persistent(AbstractCardBase card, Persistent? bind = null)
        {
            CardBase = card;
            Type = card.GetType();
            Active = true;
            Childs = new();
            AvailableTimes = card.InitialUseTimes;

            Father = bind;
            bind?.Childs?.Add(this);
        }
        internal static EventPersistentSetHandler GetDelayedHandler(EventPersistentSetHandler? input)
        {
            return (me, s, v) =>
            {
                if (me.Game.TempDelayedTriggerQueue == null)
                {
                    Queue<Action> queue = new();
                    me.Game.TempDelayedTriggerQueue = queue;
                    input?.Invoke(me, s, v);
                    me.Game.TempDelayedTriggerQueue = null;
                    while (queue.TryDequeue(out var action))
                    {
                        action.Invoke();
                    }
                }else
                {
                    input?.Invoke(me, s, v);
                }
            };
        }
    }
}
