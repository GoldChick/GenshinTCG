namespace TCGBase
{
    /// <summary>
    /// 受到伤害后产生的sender，可能并不一定用于直接触发<br/>
    /// </summary>
    public class HurtSender : AbstractSender
    {
        public override string SenderName => SenderTag.AfterHurt.ToString();
        public int Element { get; init; }
        public int InitialElement { get; init; }
        public int Damage { get; internal set; }//set只是为了合并
        public int TargetIndex { get; init; }
        public ReactionTags Reaction { get; init; }
        public DamageSource DirectSource { get; init; }
        public IDamageSource RootSource { get; init; }

        internal HurtSender(int teamID, DamageVariable dv, ReactionTags reaction, DamageSource directSource, IDamageSource rootSource, int initialElement) : base(teamID)
        {
            Element = dv.Element;
            Damage = dv.Damage;
            TargetIndex = dv.TargetIndex;
            Reaction = reaction;
            DirectSource = directSource;
            RootSource = rootSource;
            InitialElement = initialElement;
        }
    }
}
