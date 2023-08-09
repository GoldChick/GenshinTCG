using TCGAI;
namespace TCGGame
{
    public abstract partial class AbstractTeam
    {
        /// <summary>
        /// 为True则为骰子模式,需要消耗骰子;为False则为行动模式,不需要骰子(NOTE:很远的将来)
        /// </summary>
        public bool UseDice { get; init; }
        
        public AbstractAI AI { get; init; }
        /// <summary>
        /// 用于pvp模式仅限4个角色(NOTE:pve-很远的将来)
        /// </summary>
        public Character[] Characters { get; protected set; }


        public Assist?[] Assists { get;  } = { null, null, null, null };
        public Summon?[] Summons { get;  } = { null, null, null, null };

        /// <summary>
        /// 
        /// </summary>
        public bool ReplaceAssist()
        {
            return false;
        }
        public bool AddPersistent()
        {

        }
        
    }
}
