//TODO:�޸�AI��ʵ�ֻ���
//���еĻ��ƿ�����ɻ���,�����ڱ�д

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
        /// һ���������õķ���
        /// <list type="bullet">
        /// <item><see cref="Switch"/></item>
        /// <item><see cref="UseSkill"/></item>
        /// <item><see cref="UseCard"/></item>
        /// <item><see cref="Pass"/></item>
        /// </list>
        /// </summary>
        public abstract AIEvent Act();
        /// <returns>
        /// �����л����Ľ�ɫ�ľ���index
        /// </returns>
        public abstract AIEvent Switch();
        /// <returns>
        /// ����ʹ�õļ��ܵľ���index
        /// </returns>
        public abstract AIEvent UseSkill();
        /// <returns>
        /// ����ʹ�õĿ��Ƶľ���index
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