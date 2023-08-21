namespace TCGBase
{
    /// <summary>
    /// 受到伤害后产生的sender，可能并不一定用于直接触发<br/>
    /// </summary>
    internal class HurtSender : AbstractSender
    {
        public override string SenderName => Tags.SenderTags.AFTER_HURT;
        public int Element { get; init; }
        public int Damage { get; init; }
        public string Reaction { get; init; }
    }
}
