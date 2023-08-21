using TCGBase;
using TCGUtil;

namespace TCGGame
{
    public abstract partial class AbstractGame
    {
        /// <param name="evt">已经证明是valid的NetEvent</param>
        /// <returns>是否是战斗行动</returns>
        public virtual bool HandleEvent(NetEvent evt, int currTeam)
        {
            switch (evt.Action.Type)
            {
                case ActionType.UseSKill:
                    var t = Teams[currTeam];
                    t.Characters[t.CurrCharacter].Card.Skills[evt.Action.Index].AfterUseAction(this, currTeam);
                    break;

                default://空过
                    Logger.Warning($"玩家{currTeam}选择了空过！");
                    Teams[currTeam].Pass = true;
                    break;
            }
            //TODO:蒙德共鸣这种
            return true;
        }
        public virtual bool IsFastAction(NetAction action)
        {
            //蒙德共鸣怎样实现?
            return true;
        }
    }
}
