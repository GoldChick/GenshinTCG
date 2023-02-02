using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//################################################################
//双方各有一个用来储存本局游戏所有[Event]的List<IEventBase>
//同时又有一个储存当前[非快速行动]的Queue<IEventBase>
//各种行动会发送[Event],先储存到Queue中，二次确认之后Queue进行执行，并且储存到List中
//可以选择导出记录
//################################################################
namespace TCGBase
{
    public enum EventType
    {
        None,//什么也不做
        Pass,//空过
        Blend,//调和
        Switch,
        UseAssistCard,
        UseNormalAttack,
        UseE,
        UseQ,

        GainDice,
        GainCard,
    }

    public interface IEventBase
    {
        Side GetSide();//是哪一方触发的事件
        EventType GetEventType();
        bool IsFastAction();
    }
    public interface IEvent<T> : IEventBase
    {
        T GetAdditionalValue();
    }
}
