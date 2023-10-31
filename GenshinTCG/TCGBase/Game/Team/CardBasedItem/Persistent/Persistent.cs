namespace TCGBase
{
    public abstract class AbstractPersistent
    {
        protected int _availableTimes;

        public abstract AbstractCardPersistent CardBase { get; }
        public abstract int AvailableTimes { get; set; }
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
        public AbstractPersistent? Father { get; set; }
        /// <summary>
        /// 依赖于自己的persistent们，此状态清除时将把list中的其他状态也清除
        /// </summary>
        public List<AbstractPersistent> Childs { get; }
        protected AbstractPersistent(Type type)
        {
            Type = type;
            Active = true;
            Childs = new();
        }
        public abstract void EffectTrigger(PlayerTeam me, AbstractSender sender, AbstractVariable? variable);
    }
    public class Persistent<T> : AbstractPersistent where T : AbstractCardPersistent
    {
        public override AbstractCardPersistent CardBase => Card;
        public T Card { get; protected set; }
        public override int AvailableTimes
        {
            get => _availableTimes;
            set
            {
                if (Card.DeleteWhenUsedUp)
                {
                    if (value <= 0)
                    {
                        _availableTimes = 0;
                        Active = false;
                    }
                    else
                    {
                        _availableTimes = value;
                        Active = true;
                    }
                }
                else
                {
                    _availableTimes = int.Max(0, value);
                }
            }
        }
        protected Persistent(Type type, T card, AbstractPersistent? bind = null) : base(type)
        {
            Card = card;
            AvailableTimes = card.InitialUseTimes;
            Father = bind;
            bind?.Childs.Add(this);
        }
        public Persistent(T card, AbstractPersistent? bind = null) : this(card.GetType(), card, bind) { }
        public override void EffectTrigger(PlayerTeam me, AbstractSender sender, AbstractVariable? variable)
        {
            if (Card.TriggerDic.TryGetValue(sender.SenderName, out var trigger))
            {
                trigger?.Invoke(me, this, sender, variable);
            }
            if (Card.TriggerDic.TryGetValue(SenderTag.AfterAnyAction.ToString(), out var trigger_any))
            {
                trigger_any?.Invoke(me, this, sender, variable);
            }
            //TODO:game.Step(), such as shining?
        }
    }
}
