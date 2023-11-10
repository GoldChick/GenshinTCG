namespace TCGBase
{
    /// <summary>
    /// 受到元素附着，但是没有伤害后产生的sender
    /// </summary>
    public class NoDamageHurtSender : AbstractSender
    {
        public override string SenderName => SenderTag.AfterNoDamageHurt.ToString();
        public int Element { get; init; }
        public int TargetIndex { get; init; }
        public ReactionTags Reaction { get; init; }
        public DamageSource DirectSource { get; init; }
        public IDamageSource RootSource { get; init; }

        internal NoDamageHurtSender(int teamID, DamageVariable dv, ReactionTags reaction, DamageSource directSource, IDamageSource rootSource) : base(teamID)
        {
            Element = dv.Element;
            TargetIndex = dv.TargetIndex;
            Reaction = reaction;
            DirectSource = directSource;
            RootSource = rootSource;
        }
    }
}
