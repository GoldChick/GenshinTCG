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
}
