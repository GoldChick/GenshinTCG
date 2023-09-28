using TCGGame;

namespace TCGCard
{
    public interface ICardSkill : ICardServer, IUsable<AbstractTeam>
    {
        /// <summary>
        /// 是否是[准备行动]<br/>
        /// [准备行动]可以触发[增伤]，但无法触发[使用技能后]的各种效果
        /// </summary>
        //public bool Prepare { get; }
    }
}