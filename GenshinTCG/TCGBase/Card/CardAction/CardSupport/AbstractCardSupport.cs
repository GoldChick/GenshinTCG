namespace TCGBase
{
    public enum SupportTags
    {
        Location,
        Partner,
        Item,
    }
    /// <summary>
    /// 支援牌，打出后在支援区生成某种东西
    /// </summary>
    public abstract class AbstractCardSupport : AbstractCardAction
    {
        protected AbstractCardSupport()
        {
            Variant = -5;
        }
        protected private AbstractCardSupport(CardRecordSupport record) : base(record)
        {

        }
    }
}
