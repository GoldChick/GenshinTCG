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
    public abstract class AbstractCardSupport : AbstractCardAction, ICardPersistnet
    {
        public abstract SupportTags SupportTag { get; }
        /// <summary>
        /// default do nothing for Support Card<br/>
        /// 或许可以用来加个入场效果
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            if (me.Supports.Full)
            {
                me.Supports.TryRemoveAt(targetArgs[0]);
            }
            me.Supports.Add(new Persistent<AbstractCardPersistent>(new CardPersistentSupport(this)));
        }

        /// <summary>
        /// 产生时候的基础使用次数，默认和[最大次数]一样
        /// </summary>
        public virtual int InitialUseTimes { get => MaxUseTimes; }
        public abstract int MaxUseTimes { get; }
        public abstract PersistentTriggerDictionary TriggerDic { get; }
        public virtual string TextureNameSpace => "Minecraft";
        public string TextureNameID => NameID;
        public virtual bool CustomDesperated { get => false; }
        /// <summary>
        /// 用来给客户端提供使用的表现参数
        /// </summary>
        public virtual int[] Info(AbstractPersistent p) => new int[] { p.AvailableTimes };
    }
}
