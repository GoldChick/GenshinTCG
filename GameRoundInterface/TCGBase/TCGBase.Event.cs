using TCGInfo;
//################################################################
//各种行动会发送[Event],先储存到Stack中，二次确认之后进行执行，并且储存到List中
//可以选择导出记录
//################################################################
namespace TCGBase
{
    public delegate void TCGEventHandler();
    public abstract class IEvent
    {
        public Side Side { get; }//由哪一方触发，注意！并不是Bus中的索引！
        public IEvent(Side side)
        {
            Side = side;
        }

        public virtual bool IsFastAction()
        {
            return true;
        }
        public abstract ActionType GetEventType();
        public abstract void Work(params IInfo[] infos);
    }
    /// <summary>
    /// Event需要绑定的参数
    /// 比如生效于某个角色/某个骰子等等
    /// </summary>
    /// <typeparam name="T">一个或者多个角色/骰子/卡牌等</typeparam>
    public abstract class IEvent<T> : IEvent
    {
        public IEvent(Side side) : base(side)
        {
        }
        public abstract T GetAdditionalValue();
    }
}
