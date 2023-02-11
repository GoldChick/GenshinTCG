using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCGBase
{
    /// <summary>
    /// 游戏开始阶段会进行先后手判定，进行一次标号储存,
    /// <para></para>
    /// 之后每回合开始阶段进行一次判定，但是不改变标号
    /// </summary>
    public enum Side
    {
        Backward = 0,
        Front = 1,
    }
    /// <summary>
    /// 元素类型(实际上并不仅仅是元素)
    /// 0为Trival，对应物理攻击/万能元素骰
    /// 1-7为七元素
    /// </summary>
    public enum ElementType
    {
        Trival,
        Anemo,
        Geo,
        Electro,
        Dendro,
        Hydro,
        Pyro,
        Cyro
    }
    /// <summary>
    /// 某次行动的type
    /// </summary>
    public enum ActionType
    {
        None = 0,//什么也不做
        Pass = 1,//空过
        Blend = 2,//调和
        Switch = 4,
        UseAssistCard = 8,

        UseSkill = 16,

        GainDice = 32,
        GainCard = 64,

        Die = 128,

        Others = 256//虽然但是，似乎没有什么其他行动了
    }
}
