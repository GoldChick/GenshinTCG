using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGInfo;
//################################################################
//双方各有一个用来储存本局游戏所有[Event]的List<IEventBase>
//同时又有一个储存当前[非快速行动]的Queue<IEventBase>
//各种行动会发送[Event],先储存到Queue中，二次确认之后Queue进行执行，并且储存到List中
//可以选择导出记录
//################################################################
namespace TCGBase
{
    public delegate void TCGEventHandler();
    public interface IEvent
    {
        Side GetSide();//是哪一方触发的事件
        ActionType GetEventType();
        bool IsFastAction();
        void Work(params IInfo[] infos);
    }
    /// <summary>
    /// Event需要绑定的参数
    /// 比如生效于某个角色/某个骰子等等
    /// </summary>
    /// <typeparam name="T">一个或者多个角色/骰子/卡牌等</typeparam>
    public interface IEvent<T> : IEvent
    {
        T GetAdditionalValue();
    }
}
