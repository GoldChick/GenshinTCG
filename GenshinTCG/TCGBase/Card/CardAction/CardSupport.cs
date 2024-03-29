﻿namespace TCGBase
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
    public class CardSupport : AbstractCardAction
    {
        public CardSupport(CardRecordAction record) : base(record)
        {
        }
    }
}
