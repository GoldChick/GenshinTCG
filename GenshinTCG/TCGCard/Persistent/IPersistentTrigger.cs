﻿using TCGBase;
using TCGGame;

namespace TCGCard
{
    /// <summary>
    /// 用来表示某buff的触发效果<br/>
    /// 为了便于复用（黄盾、紫盾）使用了接口
    /// </summary>
    public interface IPersistentTrigger
    {
        /// <summary>
        /// 结算或预结算一次效果<br/>
        /// 注意：加费、减费的效果和增伤、减伤的效果分开写
        /// </summary>
        /// <param name="persitent">当前触发效果的persistent对应的object,用来减少、增加次数</param>
        /// <param name="sender">信息的发送者,如打出的[牌],使用的[技能]</param>
        /// <param name="variable">可以被改写的东西,如[消耗的骰子们],[伤害] <b>(不应改变类型)</b></param>
        /// <returns>是否弃置</returns>
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable);

    }
}