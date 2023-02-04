using System;
using System.Collections.Generic;
using TCGBase;

namespace TCGGame
{
    /// <summary>
    /// 游戏开始之后，整个游戏的总线
    /// </summary>
    public class Bus
    {
        private static Bus instance;
        public static Bus Instance()
        {
            if (instance == null)
            {
                instance = new Bus();
            }
            return instance;
        }

        public List<IEventBase> events;
        public List<Team> teams;

        /// <summary>
        /// 用来为team提供event的预览
        /// </summary>
        /// <param name="team"></param>
        /// <param name="eventBase"></param>
        public void Post(Side teamSide, IEventBase eventBase)
        {
            teams[(int)teamSide].events.Enqueue(eventBase);
        }
        /// <summary>
        /// 取消行动
        /// </summary>
        public void Pop(Side teamSide, IEventBase eventBase)
        {
            teams[(int)teamSide].events.Clear();
        }
        /// <summary>
        /// 确认行动
        /// </summary>
        public void Action(Side teamSide)
        {
            start: IEventBase eventBase = teams[(int)teamSide].events.Dequeue();
            eventBase.Work();
            events.Add(eventBase);
            if (eventBase.IsFastAction())
            {
                goto start;
            }
        }
        /// <summary>
        /// 将游戏结束之后的events保存到本地
        /// </summary>
        public void Save(bool doSave)
        {
            if (doSave)
            {
                foreach (IEventBase eventBase in events)
                {
                    //if(Array.Exists(eventBase.GetType().GetInterfaces(), t => t.GetGenericTypeDefinition() == typeof(IEvent<>)))
                    {
                    }
                }
            }
            events.Clear();
        }
    }
}
