namespace TCGBase
{
    public partial class PlayerTeam
    {
        internal bool IsEventValid(NetEvent evt) => IsLimitValid(evt.Action) && IsAdditionalTargetValid(evt) && IsCostValid(evt);
        /// <summary>
        /// 判断action.Index是否在合适范围
        /// </summary>
        private bool IsLimitValid(NetAction action) => action.Index >= 0 && action.Type switch
        {
            ActionType.ReRollDice or ActionType.ReRollCard or ActionType.Pass => true,
            ActionType.Switch or ActionType.SwitchForced =>
                action.Index < Characters.Length && action.Index != CurrCharacter && Characters[action.Index].Alive,
            ActionType.UseSKill => Characters[CurrCharacter].Active && action.Index < Characters[CurrCharacter].Card.Skills.Length,
            ActionType.UseCard or ActionType.Blend => action.Index < CardsInHand.Count,
            _ => false
        };
        private bool IsAdditionalTargetValid(NetEvent evt)
        {
            bool temp = true;
            switch (evt.Action.Type)
            {
                case ActionType.ReRollDice:
                    temp = Dices.Count == evt.AdditionalTargetArgs.Length && evt.AdditionalTargetArgs.All(i => i == 0 || i == 1);
                    break;
                case ActionType.ReRollCard:
                    temp = CardsInHand.Count == evt.AdditionalTargetArgs.Length && evt.AdditionalTargetArgs.All(i => i == 0 || i == 1);
                    break;
                case ActionType.UseCard:
                    var actioncard = CardsInHand[evt.Action.Index];
                    if (actioncard is ITargetSelector se)
                    {
                        temp = se.TargetDemands.Length == evt.AdditionalTargetArgs.Length && IsManyTargetDemandValid(se.TargetDemands, evt.AdditionalTargetArgs);
                    }
                    else if (actioncard is AbstractCardSupport && Supports.Full)
                    {
                        temp = 1 == evt.AdditionalTargetArgs.Length && evt.AdditionalTargetArgs[0] >= 0 && evt.AdditionalTargetArgs[0] < Supports.Count;
                    }
                    temp &= actioncard.CanBeUsed(this, evt.AdditionalTargetArgs);
                    break;
            }
            return temp;
        }
        private bool IsCostValid(NetEvent evt)
        {
            if (evt.Action.Type == ActionType.Blend)
            {
                return evt.CostArgs != null && evt.CostArgs.Sum() == 1 && evt.CostArgs[(int)Characters[CurrCharacter].Card.CharacterElement] == 0 && ContainsCost(evt.CostArgs);
            }
            else
            {
                bool temp = true;
                if (evt.Action.Type == ActionType.UseCard)
                {
                    var card = CardsInHand[evt.Action.Index];
                    if (card is IEnergyConsumerCard ec)
                    {
                        temp = evt.AdditionalTargetArgs.Length > ec.CostMPFromCharacterIndexInArgs && Characters[evt.AdditionalTargetArgs[ec.CostMPFromCharacterIndexInArgs]].MP >= card.Cost.MPCost;
                    }
                    else
                    {
                        temp = CurrCharacter >= 0 && Characters[CurrCharacter].MP >= card.Cost.MPCost;
                    }
                }
                else if (evt.Action.Type == ActionType.UseSKill)
                {
                    var c = Characters[CurrCharacter];
                    temp = c.MP >= c.Card.Skills[evt.Action.Index].Cost.MPCost;
                }
                return temp && GetEventFinalDiceRequirement(evt.Action).DiceEqualTo(evt.CostArgs) && ContainsCost(evt.CostArgs);
            }
        }
        /// <summary>
        /// 返回经过处理的TargetEnumForNetEvent们<br/>
        /// </summary>
        internal List<TargetEnum> GetCardTargetEnums(int cardindex)
        {
            List<TargetEnum> enums = new();
            var actioncard = CardsInHand[cardindex];
            if (actioncard is ITargetSelector se)
            {
                enums.AddRange(se.TargetDemands.Select(d => d.Target));
            }
            else if (actioncard is AbstractCardSupport && Supports.Full)
            {
                enums.Add(TargetEnum.Support_Me);
            }
            return enums;
        }
        protected bool IsManyTargetDemandValid(IEnumerable<TargetDemand> demands, int[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                var d = demands.ElementAt(i);
                if (parameters[i] >= 0 && parameters[i] < GetTargetEnumMaxCount(d.Target) && d.Condition(this, parameters[..(i + 1)]))
                {
                    continue;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 返回有效的使用对象们<br/>
        /// </summary>
        //internal List<TargetValid> GetTargetValids(NetAction action)
        //{
        //    List<TargetValid> enums = new();
        //    switch (action.Type)
        //    {
        //        case ActionType.ReRollDice:
        //            for (int i = 0; i < Dices.Count; i++)
        //            {
        //                enums.Add(new(TargetEnumEvent.Dice_Optional, Enumerable.Range(0, Dices.Count)));
        //            }
        //            break;
        //        case ActionType.ReRollCard:
        //            for (int i = 0; i < CardsInHand.Count; i++)
        //            {
        //                enums.Add(new(TargetEnumEvent.Card_Optional, Enumerable.Range(0, CardsInHand.Count)));
        //            }
        //            break;
        //        case ActionType.UseSKill:
        //            if (Characters[CurrCharacter].Card.Skills[action.Index] is ITargetSelector selector)
        //            {
        //                enums.AddRange(TargetDemandToAllTargetValid(selector.TargetDemands));
        //            }
        //            break;
        //        case ActionType.UseCard:
        //            var actioncard = CardsInHand[action.Index];
        //            if (actioncard is ITargetSelector se1)
        //            {
        //                enums.AddRange(TargetDemandToAllTargetValid(se1.TargetDemands));
        //            }
        //            if (actioncard is AbstractCardSupport && Supports.Full)
        //            {
        //                enums.AddRange(TargetDemandToAllTargetValid(new TargetDemand[] { new(TargetEnumEvent.Support_Me, (me, indexs) => true) }));
        //            }
        //            break;
        //    }
        //    return enums;
        //}

        /// <summary>
        /// 返回经过各种减费结算的
        /// </summary>
        internal CostVariable GetEventFinalDiceRequirement(NetAction action, bool realAction = false)
        {
            CostVariable c;
            switch (action.Type)
            {
                case ActionType.Switch:
                    c = new CostCreate().Void(1).ToCostInit().ToCostVariable();
                    Game.EffectTrigger(new UseDiceFromSwitchSender(TeamIndex, CurrCharacter, action.Index % Characters.Length, realAction), c, false);
                    break;
                case ActionType.UseSKill:
                    AbstractCardCharacter chaCard = Characters[CurrCharacter].Card;
                    AbstractCardSkill skill = chaCard.Skills[action.Index % chaCard.Skills.Length];
                    c = new(skill.Cost);
                    Game.EffectTrigger(new UseDiceFromSkillSender(TeamIndex, Characters[CurrCharacter], skill, realAction), c, false);
                    break;
                case ActionType.UseCard:
                    var card = CardsInHand[action.Index % CardsInHand.Count];
                    c = new(card.Cost);
                    Game.EffectTrigger(new UseDiceFromCardSender(TeamIndex, card, realAction), c, false);
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
                    c = new(ints, 0);
                    break;
                default:
                    c = new();
                    break;
            }
            return c;
        }
        /// <summary>
        /// 通过client调用，返回下一些可用目标
        /// </summary>
        internal List<int> GetNextValidTargets(int cardindex, int[] parameters_already)
        {
            List<int> ints = new();
            if (CardsInHand[cardindex] is ITargetSelector se && se.TargetDemands.Length > parameters_already.Length)
            {
                var curr_d = se.TargetDemands[parameters_already.Length];
                for (int i = 0; i < GetTargetEnumMaxCount(curr_d.Target); i++)
                {
                    if (curr_d.Condition(this, parameters_already.Append(i).ToArray()))
                    {
                        ints.Add(i);
                    }
                }
            }
            return ints;
        }
        internal List<TargetValid> TargetDemandToAllTargetValid(IEnumerable<TargetDemand> demand)
            => GetTargetValid(demand).Select((ints, index) => new TargetValid(demand.ElementAt(index).Target, ints)).ToList();
        private IEnumerable<List<int>> GetTargetValid(IEnumerable<TargetDemand> demand, int depth = 0, List<int>? curr = null)
        {
            curr ??= new();
            if (depth == demand.Count())
            {
                yield return curr;
                yield break;
            }
            var nums = Enumerable.Range(0, GetTargetEnumMaxCount(demand.ElementAt(depth).Target));
            foreach (var d in nums)
            {
                curr.Add(d);
                if (demand.ElementAt(depth).Condition.Invoke(this, curr.ToArray()))
                {
                    foreach (var item in GetTargetValid(demand, depth + 1, curr))
                    {
                        yield return item;
                    }
                }
                curr.Remove(d);
            }
        }
        protected int GetTargetEnumMaxCount(TargetEnum e) => e switch
        {
            //TargetEnum.Card_Me => CardsInHand.Count,
            TargetEnum.Character_Enemy => Enemy.Characters.Length,
            TargetEnum.Character_Me => Characters.Length,
            TargetEnum.Summon_Enemy => Enemy.Summons.Count,
            TargetEnum.Summon_Me => Summons.Count,
            TargetEnum.Support_Enemy => Enemy.Supports.Count,
            TargetEnum.Support_Me => Supports.Count,
            _ => throw new Exception("PlayerTeam.NetEvent.TargetEnumToEnumrable():不支持的TargetEnum!")
        };
    }
}
