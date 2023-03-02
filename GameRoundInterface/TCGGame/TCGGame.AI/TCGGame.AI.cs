using System;
using TCGBase;

namespace TCGGame
{
    public abstract class AIBase
    {
        /// <summary>
        /// 通过读入的数据返回做出的行动
        /// </summary>
        /// <param name="side">开局决定的先后手,用作index</param>
        /// <returns>返回需要Post的IEvent</returns>
        public virtual IEvent GetEvent(Side side)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 是否需要再次确认才能做出行动
        /// 即是否有[预览]环节
        /// </summary>
        public virtual bool NeedReconfirm()
        {
            return false;
        }
    }
}
