using TCGBase;

namespace TCGAI
{
    public abstract class AbstractAI
    {
        public string Name { get; set; }

        /// <summary>
        /// 一般情况会调用的方法
        /// <list type="bullet">
        /// <item><see cref="Switch"/></item>
        /// <item><see cref="UseSkill"/></item>
        /// <item><see cref="UseCard"/></item>
        /// <item><see cref="Pass"/></item>
        /// </list>
        /// </summary>
        public abstract NetEvent Act(ActionType demand,string help_txt="");

        public abstract NetEvent Switch();
        public abstract NetEvent UseSkill();
        public abstract NetEvent UseCard();
        public abstract NetEvent Pass();
        public abstract NetEvent Blend();

        public abstract NetEvent ReRollDice();
        public abstract NetEvent ReRollCard();

        public abstract NetEvent ReplaceAssist();
    }
}