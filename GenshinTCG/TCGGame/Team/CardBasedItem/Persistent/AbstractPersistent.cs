using TCGBase;
using TCGCard;

namespace TCGGame
{
    public abstract class AbstractPersistent
    {
        protected int _availableTimes;

        public abstract ICardBase CardBase { get; }
        public abstract int AvailableTimes { get; set; }
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
    public abstract class AbstractPersistent<T> : AbstractPersistent where T : IPersistent
    {
        public override ICardBase CardBase => Card;
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
            if (card == null)
            {
                throw new Exception($"AbstractPersistent<T>:找不到nameid={nameid}的类型为{typeof(T)}的ICardBase!");
            }
            Card = card;
            AvailableTimes = Card.MaxUseTimes;
        }
        public override void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            //TODO:弃置
            Card?.EffectTrigger(game.Teams[meIndex], game.Teams[1 - meIndex], this, sender, variable);
            game.Step();
        }
    }
}
