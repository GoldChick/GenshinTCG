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


        public PersistentSet<AbstractCardSupport> Supports { get; init; }
        public PersistentSet<AbstractCardSummon> Summons { get; init; }
        public PersistentSet<AbstractCardEffect> Effects { get; init; }

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

        //TODO:找到地方调用这个东西

        public void TryAddSummon(ISummonProvider provider)
        {
            var left = provider.PersistentPool.Where(s => !Summons.Contains(s.NameID)).ToList();
            if (left.Count == 0)//全都召唤了，刷新
            {
                var pool = provider.PersistentPool.Select(p => p).ToList();
                if (provider.PersistentOrdered)
                {
                    for (int i = 0; i < int.Min(provider.PersistentNum, pool.Count); i++)
                    {
                        Summons.Add(new Summon(pool[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < provider.PersistentNum && pool.Count > 0; i++)
                    {
                        int j = Random.Next(pool.Count);
                        pool.RemoveAt(j);
                        Summons.Add(new Summon(pool[j]));
                    }
                }
            }
            else if (!Summons.Full)
            {
                Summons.Add(new Summon(left[Random.Next(left.Count)]));
            }
        }
        /// <summary>
        /// 增加一个persistent类型的effect
        /// IEffect -1:团队 0-(characters.count-1):个人
        /// ISummon
        /// ISupport: 0-3顶掉的场地
        /// </summary>
        /// <param name="per"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool AddPersistent(AbstractCardPersistent per, int target = -1)
        {
            if (per is AbstractCardEffect ef)
            {
                if (target == -1)
                {
                    Effects.Add(new Effect(ef));
                }
                else
                {
                    int i = int.Clamp(target, 0, Characters.Length - 1);
                    var cha = Characters[i];
                    if (cha.Alive)
                    {
                        cha.Effects.Add(new PersonalEffect(i, ef));
                    }
                    else
                    {
                        Logger.Warning($"AbstractTeam.AddPersistent():添加名为{ef.NameID}的effect时出现问题：角色已经被击倒！");
                    }
                }
            }
            else if (per is AbstractCardSupport sp)
            {
                //TODO:场地弃置
                if (!Supports.Full)
                {
                    Supports.Add(new Support(sp));
                }
            }
            else if (per is AbstractCardSummon sm)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new ArgumentException("AbstractTeam.AddPersistent():添加的Persistent类型不支持！");
            }
            //TODO:replace
            return false;
        }

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
