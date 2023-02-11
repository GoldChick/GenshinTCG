using System.Collections.Generic;
using System.Drawing;
using TCGBase;

namespace TCGCard
{
    public enum SkillType
    {
        Passive,
        NormalAttack,
        E,
        Q
    }
    public interface ISkill
    {
        public string Name { get; }
        public string Discription { get; }
        public SkillType SkillType { get; }

        bool IsSameDice(); //sameDice控制costs中的Trival元素是否需要相同
        List<ElementType> GetCosts();//同时兼顾数量与种类
        Bitmap GetImageBmp();
        void OnUseAction(Side side);
    }
}
