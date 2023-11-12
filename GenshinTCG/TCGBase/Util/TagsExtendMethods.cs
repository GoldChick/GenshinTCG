namespace TCGBase
{
    public static class TagsExtendMethod
    {
        /// <summary>
        /// 将actiontype转化成对应的tag，可以用来作为sender触发一些effect
        /// (注：若before，此时仍然是可撤回阶段)
        /// <br/>
        /// before_sendertag:
        /// <br/>
        /// 若带有Variable为<see cref="DiceCostVariable"/>则说明为减费
        /// 若带有Variable为<see cref="CanActionVariable"/>则说明为能否行动
        /// 若不带有任何Variable，说明只是通知一次xx行动要开始了
        /// <br/>
        /// after_sendertag:
        /// <br/>
        /// 若带有Variable为<see cref="FastActionVariable"/>则说明为是否快速行动
        /// </summary>
        public static SenderTag ToSenderTags(this ActionType type, bool before = false) => type switch
        {
            ActionType.ReRollDice  => before ? SenderTag.BeforeRerollDice : SenderTag.AfterRerollDice,
            ActionType.ReRollCard => before ? SenderTag.BeforeRerollCard : SenderTag.AfterRerollCard,
            ActionType.Switch or ActionType.SwitchForced => before ? SenderTag.BeforeSwitch : SenderTag.AfterSwitch,
            ActionType.UseSKill => before ? SenderTag.BeforeUseSkill : SenderTag.AfterUseSkill,
            ActionType.UseCard => before ? SenderTag.BeforeUseCard : SenderTag.AfterUseCard,
            ActionType.Blend => before ? SenderTag.BeforeBlend : SenderTag.AfterBlend,
            ActionType.Pass => before ? SenderTag.BeforePass : SenderTag.AfterPass,
            _ => throw new Exception("Tags.ActionTypeToSenderTag():传入了未知的ActionType!")
        };
    }

}
