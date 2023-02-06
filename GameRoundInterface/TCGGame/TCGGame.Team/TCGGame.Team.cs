using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Side side;
        public List<Effect> effects;
        public Queue<IEvent> events;

        /// <summary>
        /// 默认的所有30张牌，抽出后需要remove
        /// 在初始化时是被打乱的
        /// </summary>
        public List<Assist> allCards;


        public List<Assist> cardsInHand;
        public List<Character> characters;
        /// <summary>
        /// 可读，可更改(获得/调和/使用等)
        /// </summary>
        public List<Dice> dices;
        public List<ISkill> skillOptions;
        private int currCharacter;

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
            characters.ForEach(cha => this.characters.Add(cha));
            Random rd = new Random();
            while (cards.Count > 0)
            {
                int r = rd.Next(0, cards.Count);
                allCards.Add(cards[r]);
                cards.RemoveAt(r);
            }
        }


        public void Hurt(Damage damage)
        {
            characters[damage.TargetId].Hurt(effects, damage);
        }
        public void Switch(int id)
        {
            if (currCharacter != id)
            {
                Bus.Instance().Post(new SwitchEvent(side, currCharacter, id));
            }
        }
        public void Pass()
        {
            Bus.Instance().Post(new PassEvent(side));
        }
    }
}
