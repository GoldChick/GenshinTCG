using System;
using System.Collections.Generic;
using TCGBase;

namespace TCGGame
{
    /// <summary>
    /// 游戏开始之后，整个游戏的总线
    /// 在unity中创建instance
    /// </summary>
    public class Bus
    {
        public List<IEventBase> events;
        /// <summary>
        /// 用来为team提供event的预览
        /// </summary>
        /// <param name="team"></param>
        /// <param name="eventBase"></param>
        public void Post(Team team, IEventBase eventBase)
        {
            team.events.Enqueue(eventBase);
        }
        /// <summary>
        /// 取消行动
        /// </summary>
        /// <param name="team"></param>
        /// <param name="eventBase"></param>
        public void Pop(Team team, IEventBase eventBase)
        {
            team.events.Clear();
        }
        /// <summary>
        /// 确认行动
        /// </summary>
        /// <param name="team"></param>
        public void Action(Team team)
        {
            IEventBase eventBase = team.events.Dequeue();
            eventBase.Work();
            events.Add(eventBase);
        }
        /// <summary>
        /// 将游戏结束之后的events保存到本地
        /// </summary>
        public void Save()
        {
            foreach (IEventBase eventBase in events)
            {
                //if(Array.Exists(eventBase.GetType().GetInterfaces(), t => t.GetGenericTypeDefinition() == typeof(IEvent<>)))
                {
                }
            }
        }
    }
}
