//TODO:修改AI的实现机制
//现有的机制可能造成混乱,不利于编写

namespace TCGAI
{
    public abstract class AbstractAI
    {
        public string Name { get; set; }
        public IGameInfo Program { get; init; }

        public AbstractAI(IGameInfo program)
        {
            Program = program;
        }
        public AbstractAI(AbstractAI ai)
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

        public abstract AIEvent ReRollDice();
        public abstract AIEvent ReRollCard();

        public abstract AIEvent ReplaceAssist();
        public abstract AIEvent ReplaceSummon();
    }
}