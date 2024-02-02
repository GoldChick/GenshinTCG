namespace TCGBase
{
    /// <summary>
    /// 受到元素附着，但是没有伤害后产生的sender
    /// </summary>
    public class NoDamageHurtSender : AbstractSender
    {
        public override string SenderName => SenderTag.AfterHurt.ToString();
        public int Element { get; }
        public int InitialElement { get; }
        public int TargetIndex { get; }
        public ReactionTags Reaction { get; }
        public DamageSource DirectSource { get; }
        public ITriggerable RootSource { get; }

        internal NoDamageHurtSender(int teamID, int element, int targetIndex, ReactionTags reaction, DamageSource directSource, ITriggerable rootSource, int initialElement) : base(teamID)
        {
            Element = element;
            TargetIndex = targetIndex;
            Reaction = reaction;
            DirectSource = directSource;
            RootSource = rootSource;
            InitialElement = initialElement;
        }
    }
}
