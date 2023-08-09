
namespace TCGBase
{
    /// <summary>
    /// 事件目前处于的位置
    /// </summary>
    public enum EventStage
    {
        /// <summary>
        /// Ai产生事件时调用，由于产生的事件可能被否决，故不要在此处减少使用次数
        /// <br/>
        /// 用于技能、卡牌减费等
        /// </summary>
        BeforeAdd = 1,
        /// <summary>
        /// <see cref="Team.Push(EventBase)"/>到<see cref="Team.Events"/>时调用，此处可以修改是否快切，可以减少使用次数
        /// </summary>
        OnAdd = 2,
        /// <summary>
        /// <see cref="Team.Receive(EventBase)"/>时调用，此处可以修改伤害等，可以减少使用次数
        /// </summary>
        OnWork = 4,
        /// <summary>
        /// <see cref="Team.Receive(bool)"/>中使用完一个事件之后调用，此处可以创建其他的事件等
        /// <br/>
        /// 另一种方法是监听事件发生？TODO:TOCHECK
        /// </summary>
        AfterWork = 8,
        /// <summary>
        /// 
        /// </summary>
        OnLose = 16
    }
    public abstract class GlobalEventBase
    {

    }
}
