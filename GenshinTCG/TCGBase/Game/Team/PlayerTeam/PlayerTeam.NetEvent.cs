using System.Collections;
using System.Text.Json;

namespace TCGBase
{
    public partial class PlayerTeam
    {
        internal bool IsEventValid(NetEvent evt, OperationType demand = OperationType.Trival)
        {
            if (demand == OperationType.Trival)
            {
                return IsLimitValid(evt.Operation) && IsAdditionalTargetValid(evt) && IsCostValid(evt);
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 判断action.Index是否在合适范围
        /// </summary>
        private bool IsLimitValid(NetOperation action) => action.Index >= 0 && action.Type switch
        {
            OperationType.Pass => true,
            OperationType.Switch => action.Index < Characters.Length && action.Index != CurrCharacter && Characters[action.Index].Alive,
            OperationType.UseSKill => Characters[CurrCharacter].Active && action.Index < Characters[CurrCharacter].CardBase.TriggerableList.Where(t => t.Tag == SenderTagInner.UseSkill.ToString()).Count(),
            OperationType.UseCard or OperationType.Blend => action.Index < CardsInHand.Count(),
            _ => false
        };
        private bool IsAdditionalTargetValid(NetEvent evt)
        {
            bool temp = true;
            switch (evt.Operation.Type)
            {
                case OperationType.ReRollDice:
                    temp = Dices.Count == evt.AdditionalTargetArgs.Length && evt.AdditionalTargetArgs.All(i => i == 0 || i == 1);
                    break;
                case OperationType.ReRollCard:
                    temp = CardsInHand.Count() == evt.AdditionalTargetArgs.Length && evt.AdditionalTargetArgs.All(i => i == 0 || i == 1);
                    break;
                case OperationType.UseCard:
                    var card = CardsInHand[evt.Operation.Index].CardBase;
                    if (card is AbstractCardAction actioncard)
                    {
                        if (card.CardType == CardType.Support)
                        {
                            if (Supports.Full)
                            {
                                temp = 1 == evt.AdditionalTargetArgs.Length && evt.AdditionalTargetArgs[0] >= 0 && evt.AdditionalTargetArgs[0] < Supports.Count;
                            }
                        }
                        else if (actioncard is ITargetSelector se)
                        {
                            temp = evt.AdditionalTargetArgs.Length == se.TargetDemands.Count && IsManyTargetDemandValid(se.TargetDemands, evt.AdditionalTargetArgs, out _);
                        }
                        temp &= actioncard.CanBeUsed(this);
                    }
                    break;
            }
            return temp;
        }
        private bool IsCostValid(NetEvent evt)
        {
            if (evt.Operation.Type == OperationType.Blend)
            {
                return evt.CostArgs != null && evt.CostArgs.Sum() == 1 && evt.CostArgs[(int)Characters[CurrCharacter].Card.CharacterElement] == 0 && ContainsCost(evt.CostArgs);
            }
            else
            {
                bool temp = true;
                if (evt.Operation.Type == OperationType.UseCard)
                {
                    var card = CardsInHand[evt.Operation.Index].CardBase;
                    if (card is AbstractCardAction action && action.Cost.DiceCost[9] > 0)
                    {
                        var index = action is ITargetSelector se ?
                            (se.TargetDemands.Select((td, index) => td.GetPersistent(this, index)).FirstOrDefault(p => p is Character)?.PersistentRegion ?? CurrCharacter)
                            : CurrCharacter;

                        temp = Characters.ElementAtOrDefault(index) is Character c && c.MP >= action.Cost.MPCost;
                    }
                }
                else if (evt.Operation.Type == OperationType.UseSKill)
                {
                    var c = Characters[CurrCharacter];
                    if (c.CardBase.TriggerableList.TryGetValue(SenderTagInner.UseSkill.ToString(), out var t, evt.Operation.Index) && t is ICostable cost)
                    {
                        temp = c.MP >= cost.Cost.MPCost;
                    }
                }
                return temp && GetEventFinalDiceRequirement(evt.Operation).DiceEqualTo(evt.CostArgs) && ContainsCost(evt.CostArgs);
            }
        }
        /// <summary>
        /// 返回经过处理的TargetEnumForNetEvent们<br/>
        /// </summary>
        internal List<TargetEnum> GetCardTargetEnums(int cardindex)
        {
            List<TargetEnum> enums = new();
            var actioncard = CardsInHand[cardindex].CardBase;
            if (actioncard.CardType == CardType.Support && Supports.Full)
            {
                enums.Add(TargetEnum.Support_Me);
            }
            else if (actioncard is ITargetSelector se)
            {
                enums.AddRange(se.TargetDemands.Select(d => (TargetEnum)((int)d.Select * 2 + (int)d.Team)));
            }
            return enums;
        }
        /// <summary>
        /// 只检测前parameters个对不对
        /// </summary>
        protected bool IsManyTargetDemandValid(IEnumerable<TargetDemand> demands, int[] parameters, out List<Persistent> persistents)
        {
            persistents = new();
            for (int i = 0; i < parameters.Length; i++)
            {
                var demand = demands.ElementAt(i);
                if (demand.IsPersistentValid(this, parameters[i], persistents))
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
        internal CostVariable GetEventFinalDiceRequirement(NetOperation action, bool realAction = false)
        {
            CostVariable c;
            DiceModifierSender? dms = null;
            switch (action.Type)
            {
                case OperationType.Switch:
                    c = new CostCreate().Add(ElementCategory.Void, 1).ToCostInit().ToCostVariable();
                    if (CurrCharacter != -1 && Characters.ElementAtOrDefault(action.Index) is Character target)
                    {
                        dms = new(TeamIndex, Characters[CurrCharacter], target, realAction);
                    }
                    break;
                case OperationType.UseSKill:
                    CardCharacter chaCard = Characters[CurrCharacter].Card;
                    if (chaCard.TriggerableList.TryGetValue(SenderTagInner.UseSkill.ToString(), out var h, action.Index) && h is ISkillable skill)
                    {
                        c = skill is ICostable cost ? new(cost.Cost) : new();
                        if (h is ICostable skillcost)
                        {
                            dms = new(TeamIndex, Characters[CurrCharacter], skillcost, realAction);
                        }
                    }
                    else
                    {
                        throw new Exception($"角色{chaCard.NameID}并没有第{action.Index}个技能");
                    }
                    break;
                case OperationType.UseCard:
                    var card = CardsInHand[action.Index];
                    if (card.CardBase is AbstractCardAction cardaction)
                    {
                        c = new CostVariable(cardaction.Cost.DiceCost);
                        dms = new(TeamIndex, card, cardaction, realAction);
                    }
                    else
                    {
                        throw new Exception($"第{action.Index}个Card不是AbstractCardAction");
                    }
                    break;
                case OperationType.Blend:
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
                    c = new(ints);
                    break;
                default:
                    c = new();
                    break;
            }
            if (dms != null)
            {
                c.Mod(this, dms);
            }
            return c;
        }
        /// <summary>
        /// 通过client调用，返回下一些可用目标
        /// </summary>
        internal List<int> GetNextValidTargets(int cardindex, int[] parameters_already)
        {
            List<int> ints = new();
            var card = CardsInHand[cardindex].CardBase;
            if (card is ITargetSelector se)
            {
                if (se.TargetDemands.Count > parameters_already.Length)
                {
                    if (IsManyTargetDemandValid(se.TargetDemands, parameters_already, out var persistents_already))
                    {
                        var curr_d = se.TargetDemands[parameters_already.Length];
                        for (int i = 0; i < GetTargetEnumMaxCount(curr_d.Select, curr_d.Team); i++)
                        {
                            if (curr_d.IsPersistentValid(this, i, persistents_already.ToList()))
                            {
                                ints.Add(i);
                            }
                        }
                    }
                }
            }
            return ints;
        }
        internal IEnumerable<IEnumerable<Persistent>> GetTargetValid(IEnumerable<TargetDemand> demands, int depth = 0, IEnumerable<Persistent>? curr = null)
        {
            curr ??= new List<Persistent>();
            if (depth == demands.Count())
            {
                yield return curr;
                yield break;
            }
            var ps = GetTargetEnumEnumerable((int)demands.ElementAt(depth).ToEnum());
            foreach (var p in ps)
            {
                if (demands.ElementAt(depth).Condition.Invoke(this, curr, p))
                {
                    foreach (var item in GetTargetValid(demands, depth + 1, curr.Append(p)))
                    {
                        yield return item;
                    }
                }
            }
        }
        protected int GetTargetEnumMaxCount(TargetType select, TargetTeam team)
            => GetTargetEnumMaxCount(((int)select * 2 + (int)team));
        protected int GetTargetEnumMaxCount(TargetEnum e)
            => GetTargetEnumMaxCount((int)e);

        protected int GetTargetEnumMaxCount(int region) => region switch
        {
            0 => Enemy.Characters.Length,
            1 => Characters.Length,
            2 => Enemy.Summons.Count,
            3 => Summons.Count,
            4 => Enemy.Supports.Count,
            5 => Supports.Count,
            _ => throw new Exception("PlayerTeam.NetEvent.TargetEnumToEnumrable():不支持的TargetEnum!")
        };
        protected IEnumerable<Persistent> GetTargetEnumEnumerable(int region) => region switch
        {
            0 => Enemy.Characters,
            1 => Characters,
            2 => Enemy.Summons,
            3 => Summons,
            4 => Enemy.Supports,
            5 => Supports,
            _ => throw new Exception("PlayerTeam.NetEvent.GetTargetEnumEnumerable():不支持的TargetEnum!")
        };
    }
}
