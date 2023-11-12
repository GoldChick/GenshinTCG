namespace TCGBase
{
    public partial class PlayerTeam
    {
        internal bool IsEventValid(NetEvent evt) => IsLimitValid(evt.Action) && IsTargetValid(evt) && IsDiceValid(evt);
        /// <summary>
        /// 判断action.Index是否在合适范围
        /// </summary>
        private bool IsLimitValid(NetAction action) => action.Index >= 0 && action.Type switch
        {
            ActionType.ReRollDice or ActionType.ReRollCard => true,
            ActionType.Switch or ActionType.SwitchForced =>
                action.Index < Characters.Length && action.Index != CurrCharacter && Characters[action.Index].HP > 0,
            ActionType.UseSKill => Characters[CurrCharacter].Active && action.Index < Characters[CurrCharacter].Card.Skills.Length && (Characters[CurrCharacter].Card.Skills[action.Index].Category != SkillCategory.Q || Characters[CurrCharacter].MP == Characters[CurrCharacter].Card.MaxMP),
            ActionType.UseCard => action.Index < CardsInHand.Count,
            ActionType.Blend => action.Index < CardsInHand.Count,
            ActionType.Pass => true,
            _ => false
        };
        private bool IsTargetValid(NetEvent evt)
        {
            List<TargetEnum> enums = GetTargetEnums(evt.Action);
            return enums.Count == (evt.AdditionalTargetArgs.Length)
            && enums.Select((e, index) => IsTargetValid(e, evt.AdditionalTargetArgs[index])).All(e => e)
            && evt.Action.Type switch
            {
                ActionType.UseCard => CardsInHand[evt.Action.Index].CanBeUsed(this, evt.AdditionalTargetArgs),
                _ => true
            };
        }
        private bool IsDiceValid(NetEvent evt)
        {
            if (evt.Action.Type == ActionType.Blend)
            {
                return evt.CostArgs != null && evt.CostArgs.Sum() == 1 && evt.CostArgs[(int)Characters[CurrCharacter].Card.CharacterElement] == 0 && ContainsCost(evt.CostArgs);
            }
            else
            {
                return GetEventFinalDiceRequirement(evt.Action).EqualTo(evt.CostArgs) && ContainsCost(evt.CostArgs);
            }
        }/// <summary>
         /// 返回经过处理的targetenums们<br/>
         /// 不过有效的处理似乎只有场地满了
         /// </summary>
         /// <param name="evt"></param>
         /// <returns></returns>
        internal List<TargetEnum> GetTargetEnums(NetAction action)
        {
            List<TargetEnum> enums = new();
            switch (action.Type)
            {
                case ActionType.ReRollDice:
                    for (int i = 0; i < Dices.Count; i++)
                    {
                        enums.Add(TargetEnum.Dice_Optional);
                    }
                    break;
                case ActionType.ReRollCard:
                    for (int i = 0; i < CardsInHand.Count; i++)
                    {
                        enums.Add(TargetEnum.Card_Optional);
                    }
                    break;
                case ActionType.UseSKill:
                    if (Characters[CurrCharacter].Card.Skills[action.Index] is ITargetSelector selector)
                    {
                        enums.AddRange(selector.TargetEnums);
                    }
                    break;
                case ActionType.UseCard:
                    var actioncard = CardsInHand[action.Index];
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

        /// <summary>
        /// 返回经过各种减费结算的
        /// </summary>
        internal virtual DiceCostVariable GetEventFinalDiceRequirement(NetAction action, bool realAction = false)
        {
            DiceCostVariable c;
            switch (action.Type)
            {
                case ActionType.Switch:
                    c = new(false, 1);
                    Game.EffectTrigger(new UseDiceFromSwitchSender(TeamIndex, CurrCharacter, action.Index % Characters.Length, realAction), c, false);
                    break;
                case ActionType.UseSKill:
                    AbstractCardCharacter chaCard = Characters[CurrCharacter].Card;
                    AbstractCardSkill skill = chaCard.Skills[action.Index % chaCard.Skills.Length];
                    c = new(skill.CostSame, skill.Costs);
                    Game.EffectTrigger(new UseDiceFromSkillSender(TeamIndex, CurrCharacter, action.Index % chaCard.Skills.Length, realAction), c, false);
                    break;
                case ActionType.UseCard:
                    var card = CardsInHand[action.Index % CardsInHand.Count];
                    c = new(card.CostSame, card.Costs);
                    Game.EffectTrigger(new UseDiceFromCardSender(TeamIndex, action.Index % CardsInHand.Count, realAction), c, false);
                    break;
                case ActionType.Blend:
                    int[] ints = new int[8];
                    if (CurrCharacter == -1)
                    {
                        ints[0] = 114514;
                    }
                    else
                    {
                        ints[(int)Characters[CurrCharacter].Card.CharacterElement] = 1;
                    }
                    //对于Blend并不是需要该种元素，而是不能是该种元素
                    c = new(false, ints);
                    break;
                default:
                    c = new(false);
                    break;
            }
            return c;
        }

        /// <summary>
        /// 判断targetenum所需要的targetarg是否合理
        /// </summary>
        protected virtual bool IsTargetValid(TargetEnum e, int arg) => e switch
        {
            TargetEnum.Card_Enemy => throw new Exception("PlayerTeam.IsTargetValid:Card_Enemy根本没做"),
            TargetEnum.Card_Me => arg >= 0 && arg < CardsInHand.Count,
            TargetEnum.Character_Enemy => arg >= 0 && arg < Enemy.Characters.Length,
            TargetEnum.Character_Me => arg >= 0 && arg < Characters.Length,
            TargetEnum.Dice_Optional or TargetEnum.Card_Optional => arg == 0 || arg == 1,
            TargetEnum.Summon_Enemy => arg >= 0 && arg < Enemy.Summons.Count,
            TargetEnum.Summon_Me => arg >= 0 && arg < Summons.Count,
            TargetEnum.Support_Enemy => arg >= 0 && arg < Enemy.Supports.Count,
            TargetEnum.Support_Me => arg >= 0 && arg < Supports.Count,
            _ => false
        };
    }
}
