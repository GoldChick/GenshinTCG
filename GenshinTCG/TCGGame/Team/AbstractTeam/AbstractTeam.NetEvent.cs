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
        public NetEventRequire GetEventRequirement(NetAction action)
        {
            List<TargetEnum> targets = new();
            //TargetEnum & DefaultCost 
            GetTargetRequirement(action, targets, out Cost defaultCost);

            DiceCostVariable c = new(defaultCost);

            string? tag = Tags.SenderTags.ActionTypeToSenderTag(action.Type, true);

            if (tag != null)
                EffectTrigger(Game, TeamIndex, new SimpleSender(tag), c);

            //NOTE: Forced ActionType
            if (CurrCharacter < 0 || CurrCharacter >= Characters.Length || !Characters[CurrCharacter].Alive)
            {
                //TODO:自动？
                if (action.Type == ActionType.Switch)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        c.Cost.Costs[i] = 0;
                    }
                }
                else
                {
                    c.Cost.Costs[0] = 114514;
                }
            }
            return new(c.Cost, targets);
        }
        public bool IsEventValid(NetEvent evt)
        {
            //TODO:Action.Index没有做范围限制
            var require = GetEventRequirement(evt.Action);
            return require.TargetEnums.Length == (evt.AdditionalTargetArgs?.Length ?? 0)
                && require.TargetEnums.Select((e, index) => IsTargetValid(e, evt.AdditionalTargetArgs[index])).All(e => e)
                && require.Cost.EqualTo(evt.CostArgs);
        }


        protected virtual void GetTargetRequirement(NetAction action, List<TargetEnum> enums, out Cost defaultCost)
        {
            switch (action.Type)
            {
                case ActionType.Switch:
                    defaultCost = new(false, 0);
                    enums.Add(TargetEnum.Character_Me);
                    break;
                case ActionType.UseSKill:
                    //TODO: bug: when CurrCharacter=-1
                    ICardCharacter character = Characters[CurrCharacter].Card;
                    ICardSkill skill = character.Skills[action.Index % character.Skills.Length];
                    defaultCost = new(skill.CostSame, skill.Costs);
                    if (skill is ITargetSelector selector)
                        enums.AddRange(selector.TargetEnums);
                    break;
                default:
                    defaultCost = new(false);
                    break;
            }
        }
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
