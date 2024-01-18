namespace TCGBase
{
    public enum SupportTags
    {
        None,
        Place,
        Partner,
        Item,
    }
    /// <summary>
    /// 支援牌，打出后在支援区生成某种东西
    /// </summary>
    public abstract class AbstractCardSupport : AbstractCardAction
    {
        public abstract SupportTags SupportTag { get; }
        /// <summary>
        /// default do nothing for Support Card<br/>
        /// 或许可以用来加个入场效果
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs) => me.AddSupport(this, targetArgs.ElementAtOrDefault(0));

        public override bool CustomDesperated => true;
        ///<summary>
        /// 支援区不会更新，内置方法为将MaxUseTimes设置为MaxUseTimes
        /// </summary>
        public override sealed void Update<T>(PlayerTeam me, Persistent<T> persistent) => persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
        protected AbstractCardSupport()
        {
            Variant = -5;
        }
    }
}
