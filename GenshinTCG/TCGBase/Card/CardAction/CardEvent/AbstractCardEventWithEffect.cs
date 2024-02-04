namespace TCGBase
{
    /// <summary>
    /// default add effect to team
    /// </summary>
    public abstract class AbstractCardEventWithEffect : AbstractCardAction
    {
        /// <summary>
        /// 默认为给队伍添加自身作为状态
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddEffect(this);
        }
    }
}