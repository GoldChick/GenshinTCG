﻿using TCGBase;
using TCGCard;
using TCGUtil;

namespace TCGGame
{
    public partial class PlayerTeam
    {
        protected override bool IsTargetValid(TargetEnum e, int arg) => e switch
        {
            TargetEnum.Card_Me => arg >= 0 && arg < CardsInHand.Count,
            TargetEnum.Character_Me => arg >= 0 && arg < Characters.Length,
            TargetEnum.Character_Enemy => arg >= 0 && arg < Enemy.Characters.Length,
            //TargetEnum.Dice => arg >= 0 && arg < Characters.Length,
            //TargetEnum.MultiCard => arg >= 0,
            //TargetEnum.MultiDice => arg >= 0 && arg < Characters.Length,
            TargetEnum.Summon_Me => arg >= 0 && arg < Summons.Count,
            TargetEnum.Summon_Enemy => arg >= 0 && arg < Enemy.Summons.Count,
            TargetEnum.Support_Me => arg >= 0 && arg < Supports.Count,
            TargetEnum.Support_Enemy => arg >= 0 && arg < Enemy.Supports.Count,
            _ => false
        };
        public override bool IsEventValid(NetEvent evt) => IsLimitValid(evt.Action) && IsTargetValid(evt) && IsDiceValid(evt);

        public bool IsLimitValid(NetAction action) => action.Index >= 0 && action.Type switch
        {
            ActionType.Switch or ActionType.SwitchForced =>
                action.Index < Characters.Length && action.Index != CurrCharacter && Characters[action.Index].Alive,
            ActionType.UseSKill => Characters[CurrCharacter].Active && action.Index < Characters[CurrCharacter].Card.Skills.Length,
            ActionType.UseCard => action.Index < CardsInHand.Count,
            //ActionType.Blend =>,
            ActionType.Pass => true,
            _ => false
        };
        public bool IsTargetValid(NetEvent evt)
        {
            List<TargetEnum> enums = GetTargetEnums(evt.Action);

            //Logger.Error($"enums={enums}");
            //Logger.Error($"0={evt.AdditionalTargetArgs}");
            //Logger.Error($"1={enums.Count == (evt.AdditionalTargetArgs?.Length ?? 0)}");
            //Logger.Error($"2={enums.Select((e, index) => IsTargetValid(e, evt.AdditionalTargetArgs[index])).All(e => e)}");

            return enums.Count == (evt.AdditionalTargetArgs?.Length ?? 0)
            && enums.Select((e, index) => IsTargetValid(e, evt.AdditionalTargetArgs[index])).All(e => e)
            && (!(evt.Action.Type == ActionType.UseCard) || CardsInHand[evt.Action.Index].Card.CanBeUsed(this, evt.AdditionalTargetArgs));
        }
        public bool IsDiceValid(NetEvent evt) => GetEventFinalDiceRequirement(evt.Action).Cost.EqualTo(evt.CostArgs) && ContainsCost(evt.CostArgs);
        /// <summary>
        /// 返回经过处理的targetenums们<br/>
        /// 不过有效的处理似乎只有场地满了
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public List<TargetEnum> GetTargetEnums(NetAction action)
        {
            List<TargetEnum> enums = new();
            switch (action.Type)
            {
                case ActionType.UseSKill:
                    if (Characters[CurrCharacter].Card.Skills[action.Index] is ITargetSelector selector)
                    {
                        enums.AddRange(selector.TargetEnums);
                    }
                    break;
                case ActionType.UseCard:
                    var actioncard = CardsInHand[action.Index].Card;
                    if (actioncard is ITargetSelector se1)
                    {
                        enums.AddRange(se1.TargetEnums);
                    }
                    if (actioncard is AbstractCardSupport && Supports.Full)
                    {
                        enums.Add(TargetEnum.Support_Me);
                    }
                    break;
            }
            return enums;
        }
        protected override void GetEventInitialDiceRequirement(NetAction action, out Cost defaultCost)
        {
            switch (action.Type)
            {
                case ActionType.Switch:
                case ActionType.SwitchForced:
                    defaultCost = new(false, action.Type == ActionType.Switch ? 1 : 0);
                    break;
                case ActionType.UseSKill:
                    Character character = Characters[CurrCharacter];
                    AbstractCardCharacter chaCard = Characters[CurrCharacter].Card;
                    AbstractCardSkill skill = chaCard.Skills[action.Index % chaCard.Skills.Length];
                    if (skill.Tags.Contains(Tags.SkillTags.Q) && character.MP != chaCard.MaxMP)
                    {
                        defaultCost = new(false, 114514);
                    }
                    else
                    {
                        defaultCost = new(skill.CostSame, skill.Costs);
                    }
                    break;
                case ActionType.UseCard:
                    AbstractCardAction card = CardsInHand[action.Index % CardsInHand.Count].Card;
                    defaultCost = new(card.CostSame, card.Costs);
                    break;
                case ActionType.Pass:
                    defaultCost = new(false);
                    break;
                default:
                    throw new NotImplementedException($"PlayerTeam.NetEvent.GetEventInitialDiceRequirement():还没有实现{action.Type}的情况！");
            }
        }
    }
}
