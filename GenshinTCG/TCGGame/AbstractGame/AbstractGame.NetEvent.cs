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
            var t = Teams[currTeam];

            switch (evt.Action.Type)
            {
                case ActionType.Switch:
                    var initial = t.CurrCharacter;
                    t.CurrCharacter = evt.Action.Index % t.Characters.Length;
                    EffectTrigger(new SwitchSender(initial, t.CurrCharacter));
                    //TODO:Check Alive!
                    break;
                case ActionType.UseSKill:
                    t.Characters[t.CurrCharacter].Card.Skills[evt.Action.Index].AfterUseAction(this, currTeam);
                    break;

                default://空过
                    Logger.Warning($"玩家{currTeam}选择了空过！");
                    t.Pass = true;
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
