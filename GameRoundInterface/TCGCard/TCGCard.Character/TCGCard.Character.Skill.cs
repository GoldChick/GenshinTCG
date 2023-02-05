using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        string GetSkillName();
        string GetSkillText();
        bool IsSameDice(); //sameDice控制costs中的Trival元素是否需要相同
        SkillType GetSkillType();
        List<ElementType> GetCosts();//同时兼顾数量与种类
        Bitmap GetImageBmp();
        void OnUseAction();
    }
}
