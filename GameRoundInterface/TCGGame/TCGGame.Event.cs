using System;
using System.Collections.Generic;
using TCGBase;
using TCGCard;
using TCGInfo;
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
        public NoneEvent(PlayerInfo player) : base(player)
        {
        }

        public override ActionType GetEventType()
        {
            return ActionType.None;
        }

        public override bool IsFastAction()
        {
            return true;
        }

        public override void Work(params IInfo[] infos)
        {
            throw new System.NotImplementedException();
        }
    }
    /// <summary>
    /// 空过事件 结束回合
    /// </summary>
    public class PassEvent : IEvent
    {
        public PassEvent(PlayerInfo player) : base(player)
        {
        }

        public override ActionType GetEventType()
        {
            return ActionType.Pass;
        }


        public override bool IsFastAction()
        {
            return false;
        }

        public override void Work(params IInfo[] infos)
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
        public BlendEvent(PlayerInfo player) : base(player)
        {
        }

        public override (int card, int diceOrigin, int diceToward) GetAdditionalValue()
        {
            throw new System.NotImplementedException();
        }

        public override ActionType GetEventType()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsFastAction()
        {
            throw new System.NotImplementedException();
        }

        public override void Work(params IInfo[] infos)
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
        private int before;
        private int after;
        public SwitchEvent(PlayerInfo player, int before, int after) : base(player)
        {
            this.before = before;
            this.after = after;
        }

        public override (int before, int after) GetAdditionalValue()
        {
            return (this.before, this.after);
        }

        public override ActionType GetEventType()
        {
            return ActionType.Switch;
        }
        public override bool IsFastAction()
        {
            return true;
        }

        public override void Work(params IInfo[] infos)
        {
            throw new System.NotImplementedException();
        }
    }
    public class UseAssistCardEvent : IEvent<(int card, int[] dice)>
    {
        public UseAssistCardEvent(PlayerInfo player) : base(player)
        {
        }

        public override (int card, int[] dice) GetAdditionalValue()
        {
            throw new System.NotImplementedException();
        }

        public override ActionType GetEventType()
        {
            throw new System.NotImplementedException();
        }


        public override bool IsFastAction()
        {
            throw new System.NotImplementedException();
        }

        public override void Work(params IInfo[] infos)
        {
            throw new System.NotImplementedException();
        }
    }
    public class UseSkillEvent : IEvent<(ICardSkill skill, int[] dices)>
    {
        private ICardSkill skill;
        private int[] dices;
        public UseSkillEvent(PlayerInfo player, ICardSkill skill, int[] dices) : base(player)
        {
            this.skill = skill;
            this.dices = dices;
        }
        public override (ICardSkill skill, int[] dices) GetAdditionalValue()
        {
            return (skill, dices);
        }

        public override ActionType GetEventType()
        {
            return ActionType.UseSkill;
        }


        public override bool IsFastAction()
        {
            return false;
        }

        public override void Work(params IInfo[] infos)
        {
            //skill.OnUseAction(Side);
        }
    }
    public class HurtEvent : IEvent<Damage>
    {
        private Damage damage;
        public HurtEvent(Damage damage, PlayerInfo player) : base(player)
        {
            this.damage = damage;
        }

        public override Damage GetAdditionalValue()
        {
            return damage;
        }

        public override ActionType GetEventType()
        {
            return ActionType.Hurt;
        }


        public override bool IsFastAction()
        {
            return true;
        }

        public override void Work(params IInfo[] infos)
        {
            foreach (var info in infos)
            {
                //if (info is IInfo<Team> team)
                {
                    //   team.Info.Hurt(damage);
                }
            }
        }
    }
    /// <summary>
    /// 获得骰子事件
    /// 可选参数为指定获得的骰子种类
    /// </summary>
    public class GainDiceEvent : IEvent<ElementType>
    {
        private ElementType elementType;
        public GainDiceEvent(PlayerInfo player) : base(player)
        {
            elementType = Dice.GetRandomElementType();
        }
        public GainDiceEvent(PlayerInfo player, ElementType elementType) : base(player)
        {
            this.elementType = elementType;
        }
        public override ElementType GetAdditionalValue()
        {
            return elementType;
        }

        public override ActionType GetEventType()
        {
            return ActionType.GainDice;
        }

        public override bool IsFastAction()
        {
            return true;
        }

        public override void Work(params IInfo[] infos)
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
        public GainCardEvent(PlayerInfo player) : base(player)
        {
        }

        public override ICardAssist GetAdditionalValue()
        {
            throw new System.NotImplementedException();
        }

        public override ActionType GetEventType()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsFastAction()
        {
            throw new System.NotImplementedException();
        }

        public override void Work(params IInfo[] infos)
        {
            throw new System.NotImplementedException();
        }
    }

    public class DieEvent : IEvent<int>
    {
        public DieEvent(PlayerInfo player) : base(player)
        {
        }

        public override int GetAdditionalValue()
        {
            throw new NotImplementedException();
        }

        public override ActionType GetEventType()
        {
            throw new NotImplementedException();
        }
        public override bool IsFastAction()
        {
            throw new NotImplementedException();
        }

        public override void Work(params IInfo[] infos)
        {
            throw new NotImplementedException();
        }
    }
}
