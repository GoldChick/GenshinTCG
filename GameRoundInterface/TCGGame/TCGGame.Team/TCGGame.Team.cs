using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TCGBase;
using TCGCard;

namespace TCGGame
{
    /// <summary>
    /// 对方的攻击直接作用于Team上
    /// 然后再进行细分
    /// </summary>
    public class Team
    {
        public AIBase AI { get; protected set; }

        private int currCharacter;

        public Side side;
        public List<Effect> teamEffects = new();
        public Stack<IEvent> events = new();

        /// <summary>
        /// 默认的所有30张牌，抽出后需要remove
        /// 在初始化时是被打乱的
        /// </summary>
        public List<Assist> allCards = new();

        public List<Character> characters = new();

        public ObservableCollection<Assist> cardsInHand = new();
        public ObservableCollection<Dice> dices = new();//capped at 16
        public List<ICardSkill> skillOptions = new();


        public int CurrCharacter
        {
            get => currCharacter;
            set
            {
                currCharacter = value;
                skillOptions = characters[currCharacter].card.GetSkills();
            }
        }

        public Team(List<Assist> cards, List<Character> characters)
        {
            currCharacter = -1;

            characters.ForEach(cha => this.characters.Add(cha));
            Random rd = new();
            while (cards.Count > 0)
            {
                int r = rd.Next(0, cards.Count);
                allCards.Add(cards[r]);
                cards.RemoveAt(r);
            }
        }


        public void Hurt(Damage damage)
        {
            int pos = damage.TargetId + CurrCharacter;
            if (pos == -1)
            {
                pos = 2;
            }
            else if (pos == 3)
            {
                pos = 0;
            }
            //绝对位置
            characters[pos].Hurt(teamEffects, damage);
        }
        public void Post()
        {
            IEvent e = AI.GetEvent(side);

            events.Push(e);
            if (!AI.NeedReconfirm())
            {
                Action();
            }
        }
        public void Post(IEvent e)
        {
            events.Push(e);
            if (!AI.NeedReconfirm())
            {
                Action();
            }
        }
        public void Action(IEvent e)
        {
            e.Work();
            Bus.Instance.events.Add(e);
        }
        /// <summary>
        /// 确认行动
        /// </summary>
        public void Action()
        {
        start: IEvent eventBase = events.Pop();
            eventBase.Work();
            Bus.Instance.events.Add(eventBase);
            if (eventBase.IsFastAction())
            {
                goto start;
            }
        }

    }
}
