using System.Collections.Generic;
using TCGBase;
using TCGCard;
using TCGInfo;

namespace TCGGame
{
    public class Character
    {
        public ICardCharacter card;//对应的卡牌资源


        private int hp;
        private int mp;

        public bool isAlive;
        public List<Effect> effects = new List<Effect>();
        public ElementType element;

        public ICardWeapon weapon;
        public ICardAssist artifact;
        public ICardNature nature;
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
        public void Hurt(List<Effect> teamEffects, Damage damage)
        {
            //Effect.Trigger(EffectTriggerType.On, ActionType.Hurt,
            //    p => p.effect.Work(EffectTriggerType.OnHurt, new IInfo<Damage>(damage)), effects, teamEffects);

            //TODO:还没有元素反应扣血
            Hp -= damage.DamageNum;
        }
        public void Reset()
        {
            isAlive = true;
            Hp = card.MaxHP;
            Mp = 0;
            effects.Clear();
            element = ElementType.Trival;
        }
    }
}
