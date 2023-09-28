using System.Text.Json;
using TCGBase;
using TCGCard;
using TCGUtil;

namespace TCGGame
{
    public abstract partial class AbstractTeam
    {
        /// <summary>
        /// 当action不合法时，返回需要Cost非常多的NetEventRequire
        /// </summary>
        public virtual NetEventRequire GetEventFinalDiceRequirement(NetAction action, bool forced = false)
        {
            // DefaultCost 
            GetEventInitialDiceRequirement(action, out Cost defaultCost);

            DiceCostVariable c = new(defaultCost);

            string tag = Tags.SenderTags.ActionTypeToSenderTag(action.Type, true);

            if (action.Type != ActionType.SwitchForced)
            {
                EffectTrigger(Game, TeamIndex, new SimpleSender(TeamIndex, tag), c);
            }
            return new(c.Cost);
        }
        public virtual bool IsEventValid(NetEvent evt) => true;

        /// <param name="defaultCost">其实只是这个action最初需要的骰子，不经过任何的减费加费</param>
        protected abstract void GetEventInitialDiceRequirement(NetAction action, out Cost defaultCost);

        /// <summary>
        /// 判断targetenum所需要的targetarg是否合理
        /// </summary>
        protected virtual bool IsTargetValid(TargetEnum e, int arg) => e switch
        {
            //No Card In AbstractTeam
            TargetEnum.Character_Enemy => true,//TODO: no enemy check now
            TargetEnum.Character_Me => arg >= 0 && arg < Characters.Length,
            _ => false
        };
    }
}
