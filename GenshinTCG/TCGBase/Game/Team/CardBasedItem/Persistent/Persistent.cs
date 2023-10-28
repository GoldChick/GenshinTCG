using TCGBase;
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
        /// 经过namespace修正的、在服务端中确定的nameid，如"genshin3_3:paimon"<br/>
        /// TODO:并不准确，namespace还没有正确完成
        /// </summary>
        public string NameID { get; protected init; }
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
        public AbstractPersistent(string nameid)
        {
            NameID = nameid;
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
        protected Persistent(string nameid, T card, AbstractPersistent? bind = null) : base(nameid)
        {
            Card = card ?? throw new Exception($"AbstractPersistent<T>:找不到nameid={nameid}的类型为{typeof(T)}的ICardBase!");
            //TODO:不好看，以后改改
            AvailableTimes = card.InitialUseTimes;
            Father = bind;
            bind?.Childs.Add(this);
        }
        public Persistent(T card, AbstractPersistent? bind = null) : this(card.NameID, card, bind) { }
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
