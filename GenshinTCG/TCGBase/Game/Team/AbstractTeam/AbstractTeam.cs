namespace TCGBase
{
    public abstract partial class AbstractTeam
    {
        public abstract AbstractGame Game { get; }
        public abstract PlayerTeam Enemy { get; }
        /// <summary>
        /// 在Game.Teams中的index
        /// </summary>
        public int TeamIndex { get; private init; }
        public abstract Character[] Characters { get; protected init; }
        /// <summary>
        /// 只允许使用队内的random
        /// </summary>
        public CounterRandom Random { get; init; }
        public TeamSpecialState SpecialState { get; init; }
        public abstract int CurrCharacter { get; internal set; }
        public PersistentSet<ICardPersistent> Supports { get; init; }
        public PersistentSet<AbstractCardSummon> Summons { get; init; }
        public PersistentSet<ICardPersistent> Effects { get; init; }
        protected private AbstractTeam(int teamindex)
        {
            TeamIndex = teamindex;
            Random = new();//TODO:SEED
            SpecialState = new();

            Summons = new(11, this, 4, false);
            Supports = new(12, this, 4, true);
            Effects = new(-1, this);
        }
        /// <summary>
        /// 强制切换到某一个[活]角色（可指定绝对坐标或相对坐标，默认绝对）<br/>
        /// </summary>
        public virtual void TrySwitchToIndex(int index, bool relative = false)
        { 
        }
        /// <summary>
        /// 找到失去生命值最多的角色，默认值为当前出战<br/>
        /// except可以排除某个index的角色
        /// </summary>
        public virtual int FindHPLostMost(int except = -1) => CurrCharacter;
    }
}
