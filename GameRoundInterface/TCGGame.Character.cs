using System.Collections.Generic;
using TCGBase;
using TCGCard;

namespace TCGGame
{
    public class Character
    {
        public ICardCharacter card;//对应的卡牌资源


        private int hp;
        private int mp;

        public bool isAlive;
        public List<Effect> effects;
        public ElementType element;

        public int Hp
        {
            get => hp;
            set
            {
                if (value <= 0)
                {
                    hp = 0;
                    isAlive = false;
                }
                else
                {
                    hp = value;
                }
            }
        }
        public int Mp
        {
            get => mp;
            set
            {
                mp = value;
            }
        }

        public Character(ICardCharacter card)
        {
            this.card = card;
            Reset();
        }
        /// <summary>
        /// 优先结算本人格挡特效，再计算团队格挡特效
        /// </summary>
        /// <param name="teamEffects"></param>
        public void Hurt(List<Effect> teamEffects)
        {

        }
        public void Reset()
        {
            isAlive = true;
            Hp = card.GetMaxHP();
            Mp = 0;
            effects.Clear();
            element = ElementType.Trival;
        }
    }
}
