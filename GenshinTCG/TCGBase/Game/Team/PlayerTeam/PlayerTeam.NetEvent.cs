using TCGBase;

namespace TCGBase
{
    public partial class PlayerTeam
    {
        public bool IsEventValid(NetEvent evt) => IsLimitValid(evt.Action) && IsTargetValid(evt) && IsDiceValid(evt);
        /// <summary>
        /// 判断action.Index是否在合适范围
        /// </summary>
        public bool IsLimitValid(NetAction action) => action.Index >= 0 && action.Type switch
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
        public bool IsTargetValid(NetEvent evt)
        {
            List<TargetEnum> enums = GetTargetEnums(evt.Action);

            //Logger.Error($"enums={enums}");
            //Logger.Error($"0={evt.AdditionalTargetArgs}");
            //Logger.Error($"1={enums.Count == (evt.AdditionalTargetArgs?.Length ?? 0)}");
            //Logger.Error($"2={enums.Select((e, index) => IsTargetValid(e, evt.AdditionalTargetArgs[index])).All(e => e)}");

            return enums.Count == (evt.AdditionalTargetArgs.Length)
            && enums.Select((e, index) => IsTargetValid(e, evt.AdditionalTargetArgs[index])).All(e => e)
            && evt.Action.Type switch
            {
                ActionType.UseCard => CardsInHand[evt.Action.Index].Card.CanBeUsed(this, evt.AdditionalTargetArgs),
                _ => true
            };
        }
        public bool IsDiceValid(NetEvent evt)
        {
            if (evt.Action.Type == ActionType.Blend)
            {
                return evt.CostArgs != null && evt.CostArgs.Sum() == 1 && evt.CostArgs[(int)Characters[CurrCharacter].Card.CharacterElement] == 0 && ContainsCost(evt.CostArgs);
            }
            else
            {
                return GetEventFinalDiceRequirement(evt.Action).Cost.EqualTo(evt.CostArgs) && ContainsCost(evt.CostArgs);
            }
        }/// <summary>
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

        /// <summary>
        /// 当action不合法时，返回需要Cost非常多的NetEventRequire
        /// </summary>
        public virtual NetEventRequire GetEventFinalDiceRequirement(NetAction action)
        {
            // DefaultCost 
            GetEventInitialDiceRequirement(action, out DiceCost defaultCost);

            DiceCostVariable c = new(defaultCost);

            string tag = action.Type.ToSenderTags(true).ToString();

            if (action.Type != ActionType.SwitchForced)
            {
                EffectTrigger(Game, TeamIndex, new SimpleSender(TeamIndex, tag), c);
            }
            return new(c.Cost);
        }
        /// <param name="defaultCost">其实只是这个action最初需要的骰子，不经过任何的减费加费</param>
        protected void GetEventInitialDiceRequirement(NetAction action, out DiceCost defaultCost)
        {
            switch (action.Type)
            {
                case ActionType.ReRollDice or ActionType.ReRollCard:
                    defaultCost = new(false);
                    break;
                case ActionType.Switch or ActionType.SwitchForced:
                    defaultCost = new(false, action.Type == ActionType.Switch ? 1 : 0);
                    break;
                case ActionType.UseSKill:
                    AbstractCardCharacter chaCard = Characters[CurrCharacter].Card;
                    AbstractCardSkill skill = chaCard.Skills[action.Index % chaCard.Skills.Length];
                    defaultCost = new(skill.CostSame, skill.Costs);
                    break;
                case ActionType.UseCard:
                    AbstractCardAction card = CardsInHand[action.Index % CardsInHand.Count].Card;
                    defaultCost = new(card.CostSame, card.Costs);
                    break;
                case ActionType.Blend:
                    int[] ints = new int[8];
                    ints[(int)Characters[CurrCharacter].Card.CharacterElement] = 1;
                    //对于Blend并不是需要该种元素，而是不能是该种元素
                    defaultCost = new(false, ints);
                    break;
                case ActionType.Pass:
                    defaultCost = new(false);
                    break;
                default:
                    throw new NotImplementedException($"PlayerTeam.NetEvent.GetEventInitialDiceRequirement():还没有实现{action.Type}的情况！");
            }
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
