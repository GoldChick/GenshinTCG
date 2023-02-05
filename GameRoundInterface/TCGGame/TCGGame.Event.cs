using System;
using System.Collections.Generic;
using TCGBase;
using TCGCard;
//################################################################
//这里记录游戏自带的一些事件
//如有需求可以制作自己的事件，或者重写这些事件
//################################################################
namespace TCGGame
{
    /// <summary>
    /// 什么也不做事件
    /// 可以理解为“准备一个行动轮”
    /// </summary>
    public class NoneEvent : IEvent
    {
        private Side side;
        public NoneEvent(Side side)
        {
            this.side = side;
        }
        public ActionType GetEventType()
        {
            return ActionType.None;
        }

        public Side GetSide()
        {
            return side;
        }

        public virtual bool IsFastAction()
        {
            return false;
        }

        public void Work()
        {
            throw new System.NotImplementedException();
        }
    }
    /// <summary>
    /// 空过事件 结束回合
    /// </summary>
    public class PassEvent : IEvent
    {
        private Side side;
        public PassEvent(Side side)
        {
            this.side = side;
        }
        public ActionType GetEventType()
        {
            return ActionType.Pass;
        }

        public Side GetSide()
        {
            return side;
        }

        public bool IsFastAction()
        {
            return false;
        }

        public void Work()
        {
            throw new System.NotImplementedException();
        }
    }
    /// <summary>
    /// 调和事件
    /// 消耗一张牌，将某个非当前角色元素骰子转化为当前角色颜色骰子
    /// </summary>
    public class BlendEvent : IEvent<(int card, int diceOrigin, int diceToward)>
    {
        public (int card, int diceOrigin, int diceToward) GetAdditionalValue()
        {
            throw new System.NotImplementedException();
        }

        public ActionType GetEventType()
        {
            throw new System.NotImplementedException();
        }

        public Side GetSide()
        {
            throw new System.NotImplementedException();
        }

        public bool IsFastAction()
        {
            throw new System.NotImplementedException();
        }

        public void Work()
        {
            throw new System.NotImplementedException();
        }
    }
    /// <summary>
    /// 切换角色事件
    /// 第一个int为之前的角色id，第二个int为切换后的角色id
    /// </summary>
    public class SwitchEvent : IEvent<(int before, int after)>
    {
        private Side side;
        private int before;
        private int after;
        public SwitchEvent(Side side, int before, int after)
        {
            this.side = side;
            this.before = before;
            this.after = after;
        }

        public (int before, int after) GetAdditionalValue()
        {
            return (this.before, this.after);
        }

        public ActionType GetEventType()
        {
            return ActionType.Switch;
        }

        public Side GetSide()
        {
            return side;
        }

        public virtual bool IsFastAction()
        {
            return true;
        }

        public virtual void Work()
        {
            throw new System.NotImplementedException();
        }
    }
    public class UseAssistCardEvent : IEvent<(int card, int[] dice)>
    {
        public (int card, int[] dice) GetAdditionalValue()
        {
            throw new System.NotImplementedException();
        }

        public ActionType GetEventType()
        {
            throw new System.NotImplementedException();
        }

        public Side GetSide()
        {
            throw new System.NotImplementedException();
        }

        public bool IsFastAction()
        {
            throw new System.NotImplementedException();
        }

        public void Work()
        {
            throw new System.NotImplementedException();
        }
    }
    public class UseNormalAttackEvent : IEvent<int[]>
    {
        public int[] GetAdditionalValue()
        {
            throw new System.NotImplementedException();
        }

        public ActionType GetEventType()
        {
            throw new System.NotImplementedException();
        }

        public Side GetSide()
        {
            throw new System.NotImplementedException();
        }

        public bool IsFastAction()
        {
            throw new System.NotImplementedException();
        }

        public void Work()
        {
            throw new System.NotImplementedException();
        }
    }
    public class UseEEvent : IEvent<int[]>
    {
        public int[] GetAdditionalValue()
        {
            throw new System.NotImplementedException();
        }

        public ActionType GetEventType()
        {
            throw new System.NotImplementedException();
        }

        public Side GetSide()
        {
            throw new System.NotImplementedException();
        }

        public bool IsFastAction()
        {
            throw new System.NotImplementedException();
        }

        public void Work()
        {
            throw new System.NotImplementedException();
        }
    }
    public class UseQEvent : IEvent<int[]>
    {
        public int[] GetAdditionalValue()
        {
            throw new System.NotImplementedException();
        }

        public ActionType GetEventType()
        {
            throw new System.NotImplementedException();
        }

        public Side GetSide()
        {
            throw new System.NotImplementedException();
        }

        public bool IsFastAction()
        {
            throw new System.NotImplementedException();
        }

        public void Work()
        {
            throw new System.NotImplementedException();
        }
    }
    /// <summary>
    /// 获得骰子事件
    /// 可选参数为指定获得的骰子种类
    /// </summary>
    public class GainDiceEvent : IEvent<ElementType>
    {
        private ElementType elementType;
        private Side side;
        public GainDiceEvent(Side side)
        {
            elementType = Dice.RandomElementType();
            this.side = side;
        }
        public GainDiceEvent(Side side, ElementType elementType)
        {
            this.elementType = elementType;
            this.side = side;
        }
        public ElementType GetAdditionalValue()
        {
            return elementType;
        }

        public ActionType GetEventType()
        {
            return ActionType.GainDice;
        }

        public Side GetSide()
        {
            return side;
        }

        public bool IsFastAction()
        {
            return true;
        }

        public void Work()
        {
            throw new System.NotImplementedException();
        }
    }
    /// <summary>
    /// 获得卡牌事件
    /// 默认提供[抽卡]和[印卡]两种
    /// </summary>
    public class GainCardEvent : IEvent<ICardAssist>
    {
        public ICardAssist GetAdditionalValue()
        {
            throw new System.NotImplementedException();
        }

        public ActionType GetEventType()
        {
            throw new System.NotImplementedException();
        }

        public Side GetSide()
        {
            throw new System.NotImplementedException();
        }

        public bool IsFastAction()
        {
            throw new System.NotImplementedException();
        }

        public void Work()
        {
            throw new System.NotImplementedException();
        }
    }

    public class DieEvent : IEvent<int>
    {
        public int GetAdditionalValue()
        {
            throw new NotImplementedException();
        }

        public ActionType GetEventType()
        {
            throw new NotImplementedException();
        }

        public Side GetSide()
        {
            throw new NotImplementedException();
        }

        public bool IsFastAction()
        {
            throw new NotImplementedException();
        }

        public void Work()
        {
            throw new NotImplementedException();
        }
    }
}
