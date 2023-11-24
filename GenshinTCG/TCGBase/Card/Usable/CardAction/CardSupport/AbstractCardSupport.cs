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
    public abstract class AbstractCardSupport : AbstractCardAction, ICardPersistent
    {
        public abstract SupportTags SupportTag { get; }
        /// <summary>
        /// default do nothing for Support Card<br/>
        /// 或许可以用来加个入场效果
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            if (me.Supports.Full)
            {
                me.Supports.TryRemoveAt(targetArgs[0]);
            }
            me.Supports.Add(new Persistent<ICardPersistent>(this));
        }

        public int InitialUseTimes { get => MaxUseTimes; }
        public abstract int MaxUseTimes { get; }
        public abstract PersistentTriggerDictionary TriggerDic { get; }
        public virtual bool CustomDesperated { get => true; }
        /// <summary>
        /// 支援区默认不会有变种
        /// </summary>
        public int Variant => -1;
        /// <summary>
        /// 支援区默认不会更新
        /// </summary>
        public void Update<T>(PlayerTeam me, Persistent<T> persistent) where T : ICardPersistent => persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);

        public virtual void OnDesperated(PlayerTeam me, int region)
        {
        }
    }
}
