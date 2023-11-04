namespace TCGBase
{
    public enum SenderTag
    {
        GameStart,
        /// <summary>
        /// 投掷阶段，此sendertag只会触发自己Team中的效果
        /// </summary>
        RollingStart,
        /// <summary>
        /// 行动阶段开始时，没有队伍参数
        /// </summary>
        RoundStart,
        /// <summary>
        /// 结束阶段，没有队伍参数
        /// </summary>
        RoundOver,
        /// <summary>
        /// 我方行动开始前，如[天狐霆雷]
        /// </summary>
        RoundMeStart,
        #region 用于减费
        UseDiceFromCard,
        UseDiceFromSkill,
        UseDiceFromSwitch,
        #endregion
        #region 某个Player Action前
        BeforeRerollDice,
        BeforeRerollCard,
        BeforeSwitch,
        /// <summary>
        /// 并没有实际作用，只是占位符
        /// </summary>
        BeforeBlend,
        BeforeUseCard,
        BeforeUseSkill,
        /// <summary>
        /// 并没有实际作用，只是占位符
        /// </summary>
        BeforePass,
        //附魔>=增伤(火共鸣) 增伤>乘伤(护体岩铠)
        ElementEnchant,
        DamageIncrease,
        HurtDecrease,
        DamageMul,
        HurtMul,
        #endregion
        #region 某个Player Action结算后
        AfterRerollDice,
        AfterRerollCard,
        AfterSwitch,
        AfterBlend,
        AfterUseCard,
        AfterUseSkill,
        AfterPass,
        /// <summary>
        /// 仅用于触发effect，而且仅在[双方进行任意行动后]的时候触发
        /// </summary>
        AfterAnyAction,
        /// <summary>
        /// 受到伤害后
        /// </summary>
        AfterHurt,
        /// <summary>
        /// 受到治疗后
        /// </summary>
        AfterHeal,
        #endregion
        #region 处理击倒
        /// <summary>
        /// 击倒预处理，在此可以判定出一些“免于被击倒”之类的状态
        /// </summary>
        PreDie,
        /// <summary>
        /// 击倒处理，预处理后生命值为0才触发。
        /// </summary>
        Die
        #endregion
    }

    /// <summary>
    /// 用于给EffectAct传递actioninfo的sender,
    /// </summary>
    public abstract class AbstractSender
    {
        /// <summary>
        /// 高度可自定义化的结算机制<br/>
        /// 如果你不想自定义，see <see cref="SenderTag"/>
        /// </summary>
        public abstract string SenderName { get; }
        public  int TeamID { get; }

        public AbstractSender(int teamID)
        {
            TeamID = teamID;
        }
    }
}
