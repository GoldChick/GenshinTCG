using GenshinTCG.TCGGame.Interface;
using TCGBase;
using TCGUtil;

namespace TCGGame
{
    public abstract partial class AbstractTeam : IEffectTrigger,IPrintable
    {
        /// <summary>
        /// 为True则为骰子模式,需要消耗骰子;为False则为行动模式,不需要骰子(NOTE:很远的将来)
        /// </summary>
        public bool UseDice { get; protected init; }

        /// <summary>
        /// 用于pvp模式仅限4个角色(NOTE:pve-很远的将来)
        /// </summary>
        public Character[] Characters { get; protected init; }


        public Support?[] Supports { get; } = { null, null, null, null };
        public Summon?[] Summons { get; } = { null, null, null, null };

        public int CurrCharacter { get; internal set; }
        public List<Effect> Effects { get; private init; }

        public AbstractTeam()
        {
            Effects = new();
            CurrCharacter = -1;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ReplaceAssist()
        {
            return false;
        }
        public bool AddPersistent()
        {
            return false;
        }

        public void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            Array.ForEach(Characters, c => c.EffectTrigger(game, meIndex, sender, variable));
            Effects.ForEach(e => e.EffectTrigger(game, meIndex, sender, variable));
            Array.ForEach(Summons, s => s?.EffectTrigger(game, meIndex, sender, variable));
            Array.ForEach(Supports, s => s?.EffectTrigger(game, meIndex, sender, variable));
        }

        public abstract void Print();
    }
}
