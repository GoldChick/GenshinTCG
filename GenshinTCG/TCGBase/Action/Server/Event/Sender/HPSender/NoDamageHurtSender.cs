namespace TCGBase
{
    /// <summary>
    /// 受到元素附着，但是没有伤害后产生的sender
    /// </summary>
    public class NoDamageHurtSender : AbstractSender
    {
        public override string SenderName => SenderTag.AfterHurt.ToString();
        public DamageElement Element { get; }
        public int InitialElement { get; }
        public int TargetIndex { get; }
        public ReactionTags Reaction { get; }
        public DamageSource DirectSource { get; }
        public AbstractCustomTriggerable RootSource { get; }

        internal NoDamageHurtSender(int teamID, DamageElement element, int targetIndex, ReactionTags reaction, DamageSource directSource, AbstractCustomTriggerable rootSource, int initialElement) : base(teamID)
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
