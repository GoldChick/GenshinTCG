using System.Text.Json;
using TCGBase;
using TCGCard;
using TCGUtil;

namespace TCGGame
{
    public partial class PlayerTeam
    {
        protected override bool IsTargetValid(TargetEnum e, int arg) => e switch
        {
            TargetEnum.Card_Me => arg >= 0 && arg < CardsInHand.Count,
            TargetEnum.Character_Enemy => arg >= 0, //TODO: no enemy check now
            TargetEnum.Character_Me => arg >= 0 && arg < Characters.Length,
            //TargetEnum.Dice => arg >= 0 && arg < Characters.Length,
            TargetEnum.MultiCard => arg >= 0,
            //TargetEnum.MultiDice => arg >= 0 && arg < Characters.Length,
            TargetEnum.Summon_Enemy => arg >= 0 && arg < Summons.Count,
            TargetEnum.Support_Enemy => arg >= 0 && arg < Supports.Count,
            _ => false
        };
        public override bool IsEventValid(NetEvent evt)
        {
            //TODO:Action.Index没有做范围限制
            var require = GetEventRequirement(evt.Action);

            //Logger.Error(JsonSerializer.Serialize(evt));

            //Logger.Error((require.TargetEnums.Length == (evt.AdditionalTargetArgs?.Length ?? 0)).ToString());
            //Logger.Error((require.TargetEnums.Select((e, index) => IsTargetValid(e, evt.AdditionalTargetArgs[index])).All(e => e)).ToString());
            //Logger.Error((require.Cost.EqualTo(evt.CostArgs)).ToString());
            //Logger.Error((ContainsCost(evt.CostArgs)).ToString());

            return require.TargetEnums.Length == (evt.AdditionalTargetArgs?.Length ?? 0)
                && require.TargetEnums.Select((e, index) => IsTargetValid(e, evt.AdditionalTargetArgs[index])).All(e => e)
                && require.Cost.EqualTo(evt.CostArgs)
                && ContainsCost(evt.CostArgs);
        }
        protected override void GetTargetRequirement(NetAction action, List<TargetEnum> enums, out Cost defaultCost)
        {
            switch (action.Type)
            {
                case ActionType.Switch:
                case ActionType.SwitchForced:
                    var a = action.Index % Characters.Length;
                    if (a != CurrCharacter && Characters[a].Alive)
                    {
                        defaultCost = new(false, action.Type == ActionType.Switch ? 1 : 0);
                    }
                    else
                    {
                        defaultCost = new(false, 114514);
                    }
                    break;
                case ActionType.UseSKill:
                    //Assert CurrCharacter != -1
                    //TODO:冰冻、石化效果？
                    Character character = Characters[CurrCharacter];
                    ICardCharacter chaCard = Characters[CurrCharacter].Card;
                    ICardSkill skill = chaCard.Skills[action.Index % chaCard.Skills.Length];
                    if (skill.Tags.Contains(Tags.SkillTags.Q) && character.MP != chaCard.MaxMP)
                    {
                        defaultCost = new(false, 114514);
                    }
                    else
                    {
                        defaultCost = new(skill.CostSame, skill.Costs);
                        if (skill is ITargetSelector selector)
                            enums.AddRange(selector.TargetEnums);
                    }
                    break;
                case ActionType.UseCard:
                    if (CardsInHand.Count > 0)
                    {
                        ICardAction card = CardsInHand[action.Index % CardsInHand.Count].Card;
                        if (card.CanBeUsed(Game, TeamIndex))
                        {
                            defaultCost = new(card.CostSame, card.Costs);
                            if (card is ITargetSelector se1)
                            {
                                enums.AddRange(se1.TargetEnums);
                            }
                        }
                        else
                        {
                            defaultCost = new(false, 114514);
                        }
                    }
                    else
                    {
                        defaultCost = new(false, 114514);
                    }
                    break;
                default:
                    defaultCost = new(false);
                    break;
            }
        }
    }
}
