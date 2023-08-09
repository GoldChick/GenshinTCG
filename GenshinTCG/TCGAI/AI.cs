//TODO:�޸�AI��ʵ�ֻ���
//���еĻ��ƿ�����ɻ���,�����ڱ�д
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
        /// <summary>
        /// ��������ʱ����
        /// </summary>
        public abstract AIEvent ReRollDice();
        /// <summary>
        /// �滻����ʱ����
        /// </summary>
        public abstract AIEvent ReRollCard();
    }
}