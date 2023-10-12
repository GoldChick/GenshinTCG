using TCGBase;
using TCGCard;
using TCGClient;
using TCGUtil;

namespace TCGGame
{
    public abstract partial class AbstractTeam : IPrintable
    {
        internal AbstractGame Game { get; private init; }
        /// <summary>
        /// 在Game.Teams中的index
        /// </summary>
        internal int TeamIndex { get; private init; }
        internal AbstractTeam Enemy => Game.Teams[1 - TeamIndex];
        internal AbstractClient Client { get => Game.Clients[TeamIndex]; }
        /// <summary>
        /// TODO:用于:TODO
        /// </summary>
        public bool IsPreviewMode { get; }
        /// <summary>
        /// 为True则为骰子模式,需要消耗骰子;为False则为行动模式,不需要骰子(NOTE:很远的将来)
        /// </summary>
        public bool UseDice { get; protected init; }
        /// <summary>
        /// 只允许使用队内的random
        /// </summary>
        internal CounterRandom Random { get; init; }
        /// <summary>
        /// 用于pvp模式仅限4个角色(NOTE:pve-很远的将来)
        /// </summary>
        public Character[] Characters { get; protected init; }


        public PersistentSet<AbstractCardPersistentSupport> Supports { get; init; }
        public PersistentSet<AbstractCardPersistentSummon> Summons { get; init; }
        public PersistentSet<AbstractCardPersistentEffect> Effects { get; init; }

        public int CurrCharacter { get; internal set; }
        public bool Pass { get; internal set; }

        public AbstractTeam(AbstractGame game, int index)
        {
            Effects = new();
            CurrCharacter = -1;
            Pass = false;
            Random = new();//TODO:SEED

            Supports = new(4, true);
            Summons = new(4);
            Effects = new();

            Game = game;
            TeamIndex = index;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ReplaceAssist()
        {
            return false;
        }
        public virtual int GetDiceNum(int type = 0) => 0;
        /// <summary>
        /// 回合开始时最先调用，如扔骰子等
        /// </summary>
        public abstract void RoundStart();
        /// <summary>
        /// 回合结束时最后调用，如清理骰子等
        /// </summary>
        public abstract void RoundEnd();
        public abstract void Print();
    }
}
