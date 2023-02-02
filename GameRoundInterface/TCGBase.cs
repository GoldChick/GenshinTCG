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
}
