using TCGBase;
using TCGCard;

namespace TCGGame
{
    public abstract partial class AbstractTeam
    {
        //2023/8/14: IMPORTANT TODO:
        //返回Target是有意义的吗?是不是都应该交给客户端???

        /// <summary>
        /// 当action不合法时，返回需要dice和target都为empty的NetEventRequire
        /// </summary>
        public NetEventRequire CheckEventRequire(NetEvent evt)
        {
            ActionType type = evt.Action.Type;
            string? tag = Tags.SenderTags.ActionTypeToSenderTag(type);
            if (tag == null)
            {
                //顺从你了
                return new();
            }
            Cost? cost = null;
            List<TargetEnum> targets = new();

            switch (type)
            {
                case ActionType.ReRollDice:
                    targets.Add(TargetEnum.MultiDice);
                    break;
                case ActionType.ReRollCard:
                    targets.Add(TargetEnum.MultiCard);
                    break;
                case ActionType.ReplaceSupport:
                    targets.Add(TargetEnum.Support);
                    break;
                case ActionType.UseSKill:
                    ICardSkill skill = Characters[CurrCharacter].Card.Skills[evt.Action.Index % Characters.Length];
                    cost = new(skill.CostSame, skill.Costs);

                    if (skill is ITargetSelector selector)
                    {
                        //Special Target
                        return new(cost, selector.TargetEnums);
                    }
                    break;
                case ActionType.UseCard:
                    //TODO: abstract没有Card呃呃呃呃

                    //ICardAction card=
                    break;
                case ActionType.Blend:
                    cost = new(false, 1);
                    targets.Add(TargetEnum.Card);
                    break;
                default:
                    break;
            }
            if (tag != null)
            {
                var c = new DiceCostVariable(cost);
                //TODO: WRONG SENDER
                EffectTrigger(Game, TeamIndex, new SimpleSender(tag), c);
                cost = c.Cost;
            }
            //TODO:Effect Trigger
            return new(cost, targets);
        }

    }
}
