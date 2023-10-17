using TCGBase;
using TCGCard;

namespace TCGGame
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
        /// 经过namespace修正的、在服务端中确定的nameid，如"genshin3_3:paimon"
        /// </summary>
        public string NameID { get; protected init; }
        /// <summary>
        /// 用于[已知class的]persistent储存更加详细的数据
        /// 如[桓纳兰那]
        /// </summary>
        public object? Data { get; set; }
        public AbstractPersistent(string nameid)
        {
            NameID = nameid;
            Active = true;
        }
        public abstract void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable);
    }
    public abstract class AbstractPersistent<T> : AbstractPersistent where T : AbstractCardPersistent
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
        protected AbstractPersistent(string nameid, T card) : base(nameid)
        {
            Card = card ?? throw new Exception($"AbstractPersistent<T>:找不到nameid={nameid}的类型为{typeof(T)}的ICardBase!");
            //TODO:不好看，以后改改
            if (Card is AbstractCardPersistentSummon cs)
            {
                AvailableTimes = cs.InitialUseTimes;
            }
            else
            {
                AvailableTimes = Card.MaxUseTimes;
            }
        }
        public override void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            if (Card != null)
            {
                if (Card.TriggerDic.TryGetValue(sender.SenderName, out var trigger))
                {
                    trigger?.Trigger(game.Teams[meIndex], this, sender, variable);
                }
                else if (Card.TriggerDic.TryGetValue(Tags.SenderTags.AFTER_ANY_ACTION, out var trigger_any))
                {
                    trigger_any?.Trigger(game.Teams[meIndex], this, sender, variable);
                }
                //TODO:game.Step(), such as shining?
            }
        }
    }
}
