using TCGBase;
using TCGInfo;

namespace TCGCard
{

    public enum SkillType
    {
        /// <summary>
        /// 被动技能不需要有任何效果
        /// 只需要在写角色的时候给一个的effect即可
        /// </summary>
        Passive,
        NormalAttack,
        E,
        Q
    }

    public interface ICardSkill : ICardServer
    {
        public SkillType SkillType { get; }
        /// <summary>
        /// 需要的无色元素是否需要颜色相同
        /// </summary>
        public bool SameDice { get; }
        /// <summary>
        /// 用8位无符号数作位运算取得元素种类
        /// <br/>
        /// <see cref="TCGBase.ElementType"/>顺序即国家顺序，
        /// 为了编写方便，继承这个类的卡牌可以按照<b>从左到右</b>的顺序表示骰子种类
        /// <br/>
        /// 一个例子：<b>"1001000"表示需要普通骰子和雷骰子</b>
        /// </summary>
        public byte DiceType { get; }
        /// <summary>
        /// 只读前八位数据
        /// 可以根据需要的元素种类偷懒不补0（？）
        /// </summary>
        public int[] DiceNum { get; }

        void OnUseAction(IInfo[] infos);
    }
}
