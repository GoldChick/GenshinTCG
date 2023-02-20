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
        private RoundStage roundStage;
        public static Bus Instance
        {
            get
            {
                instance ??= new Bus();
                return instance;
            }
        }
        private int round;
        public int Round { get => round; set => round = value; }
        /// <summary>
        /// 只读的属性
        /// </summary>
        public RoundStage RoundStage { get => roundStage; }

        public List<IEvent> events = new();
        /// <summary>
        /// 按照开局决定的Side排列的teams
        /// </summary>
        public List<Team> teams = new();

        public int currSide;

        /// <summary>
        /// 用于创建不同的模式
        /// 比如[热斗模式-无限火力]给出8个万能骰子
        /// </summary>
        /// <param name="bus">Bus的子类的对象（DIY）</param>
        public static void CreateInstance(Bus bus)
        {
            instance = bus;
        }
        public void Init(Team teamMe, Team teamYou)
        {
            instance.teams.Add(teamMe);
            instance.teams.Add(teamYou);

            Random rd = new();
            currSide = rd.Next(2);
            teams[currSide].side = Side.Front;
        }
        /// <summary>
        /// 用来为team提供event的预览
        /// </summary>
        /// <param name="team"></param>
        /// <param name="eventBase"></param>
        public void Post(IEvent eventBase)
        {
            teams[(int)eventBase.GetSide()].events.Enqueue(eventBase);
        }
        /// <summary>
        /// 取消行动
        /// </summary>
        public void Get(IEvent eventBase)
        {
            teams[(int)eventBase.GetSide()].events.Clear();
        }
        /// <summary>
        /// 确认行动
        /// </summary>
        public void Action(Side teamSide)
        {
        start: IEvent eventBase = teams[(int)teamSide].events.Dequeue();
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
                foreach (IEvent eventBase in events)
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
