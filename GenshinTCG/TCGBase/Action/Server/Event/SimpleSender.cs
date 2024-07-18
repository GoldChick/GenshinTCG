namespace TCGBase
{
    /// <summary>
    /// 仅供内部使用，外部不可调用
    /// </summary>
    internal enum SenderTagInner
    {
        UseSkill,//使用技能，用于触发效果
        UseCard,//打出卡牌，用于触发效果
        Prepare,//触发准备技能，自带一个空过
        DuringUseCard,//正在打出卡牌，用于判断是否能够成功打出
    }
    public enum SenderTag
    {
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
        /// <summary>
        /// 行动阶段中，如[准备技能]
        /// </summary>
        RoundDuring,
        /// <summary>
        /// 角色上场时（开局上场，或者复活）
        /// </summary>
        OnCharacterOn,
        #region 用于减费
        UseDiceFromCard,
        UseDiceFromSkill,
        UseDiceFromSwitch,
        #endregion
        #region 某个Player Action前
        BeforeRerollDice,
        BeforeRerollCard,
        //附魔(重华叠霜领域)>=反应增伤(火共鸣)≈普通增伤(武器)>乘伤(泡影)>除伤(护体岩铠)>减伤(盾)
        ElementEnchant,
        /// <summary>
        /// 反应后产生物品
        /// </summary>
        ElementItemGenerate,
        DamageIncrease, //加、乘、除，但乘除作用于倍率乘区
        /// <summary>
        /// 只会对自己的队伍调用，用于[护盾]
        /// </summary>
        HurtDecrease,
        #endregion
        #region 某个Player Action结算后
        AfterSwitch,
        AfterUseCard,
        DuringUseSkill,
        AfterUseSkill,

        AfterOperation,//用于结算是否快速行动
        AfterHurt,
        AfterElement,
        AfterHeal,
        /// <summary>
        /// 有<b>别的</b>状态被弃置，发出Sender : PersistentDesperatedSender
        /// </summary>
        AfterEffectDesperated,
        #endregion
        /// <summary>
        /// 免于被击倒，注意：必须有治疗效果，要不然会导致0血活角色
        /// </summary>
        AntiDie,
    }

    /// <summary>
    /// 用于给EffectAct传递actioninfo的sender,
    /// </summary>
    public  class SimpleSender
    {
        public int TeamID { get; }

        public SimpleSender(int teamID = -1)
        {
            TeamID = teamID;
        }
    }
}
