namespace TCGBase
{
    /// <summary>
    /// default add effect to team
    /// </summary>
    public abstract class AbstractCardEventWithEffect : AbstractCardEvent
    {
        /// <summary>
        /// 默认为给队伍添加自身作为状态
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddPersistent(this);
        }
    }
}