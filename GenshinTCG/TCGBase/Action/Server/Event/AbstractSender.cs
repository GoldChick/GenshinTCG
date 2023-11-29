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
        /// 即将切换下一个回合，没有队伍参数，所有回合状态应该在这里更新回合数
        /// </summary>
        RoundStep,
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
        //附魔(重华叠霜领域)>=反应增伤(火共鸣)≈普通增伤(武器)>乘伤(泡影)>除伤(护体岩铠)>减伤(盾)
        ElementEnchant,
        /// <summary>
        /// 反应后产生物品
        /// </summary>
        ElementItemGenerate,
        DamageIncrease,
        /// <summary>
        /// 只会对自己的队伍调用，用于[护盾]
        /// </summary>
        HurtDecrease,
        DamageMul,
        /// <summary>
        /// 只会对自己的队伍调用，用于[护体岩铠]
        /// </summary>
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
        /// 受到可能有元素附着、可能有伤害的东西后，发出Sender: HurtSender或NoDamageHurtSender
        /// </summary>
        AfterHurt,
        /// <summary>
        /// 受到治疗后，发出Sender: HealSender
        /// </summary>
        AfterHeal,
        /// <summary>
        /// 有<b>别的</b>状态被弃置，发出Sender : PersistentDesperatedSender
        /// </summary>
        AfterPersistentOtherDesperated,
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
