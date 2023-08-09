//TODO:修改AI的实现机制
//现有的机制可能造成混乱,不利于编写
using TCGBase;
using TCGGame;

namespace TCGAI
{
    public abstract class AI
    {
        public string Name { get; set; }
        public IGameInfo Program { get; init; }

        public AI(IGameInfo program)
        {
            Program = program;
        }
        public AI(AI ai)
        {
            Name = ai.Name;
            Program = ai.Program;
        }
        /// <summary>
        /// 一般情况会调用的方法
        /// <list type="bullet">
        /// <item><see cref="Switch"/></item>
        /// <item><see cref="UseSkill"/></item>
        /// <item><see cref="UseCard"/></item>
        /// <item><see cref="Pass"/></item>
        /// </list>
        /// </summary>
        public abstract AIEvent Act();

        /// <returns>
        /// 返回切换到的角色的绝对index
        /// </returns>
        public abstract AIEvent Switch();
        /// <returns>
        /// 返回使用的技能的绝对index
        /// </returns>
        public abstract AIEvent UseSkill();
        /// <returns>
        /// 返回使用的卡牌的绝对index
        /// </returns>
        public abstract AIEvent UseCard();
        public abstract AIEvent Pass();
        public abstract AIEvent Blend();
        /// <summary>
        /// 重骰骰子时调用
        /// </summary>
        public abstract AIEvent ReRollDice();
        /// <summary>
        /// 替换卡牌时调用
        /// </summary>
        public abstract AIEvent ReRollCard();
    }
}