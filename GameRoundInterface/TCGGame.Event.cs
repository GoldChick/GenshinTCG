using System.Collections.Generic;
using TCGBase;
using TCGCard.CardInterface;
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
    public class NoneEvent : IEventBase
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
    public class PassEvent : IEventBase
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
    public class GainDiceEvent : IEvent<ElementType>
    {
        public ElementType GetAdditionalValue()
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

}
